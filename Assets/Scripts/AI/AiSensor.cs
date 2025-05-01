using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class AiSensor : MonoBehaviour
{
    [SerializeField] private float distance = 10;
    [SerializeField] private float angle = 30;
    [SerializeField] private float startHeight = 1.0f;
    [SerializeField] private float endHeight = 2.0f;
    [SerializeField] private Color meshColor = Color.red;
    [SerializeField] private int scanFrequency = 30;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask occlusionLayers;

    private HashSet<GameObject> objects = new HashSet<GameObject>();
    private bool playerInSight;

    private Collider[] colliders = new Collider[50];
    private Mesh mesh;
    private int count;
    private float scanInterval;
    private float scanTimer;
    private bool playerIsClose;

    private Transform cachedTransform;

    // Start is called before the first frame update
    void Start()
    {
        scanInterval = 1.0f / scanFrequency;
        cachedTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        scanTimer -= Time.deltaTime;
        if (scanTimer < 0)
        {
            scanTimer += scanInterval;
            Scan();
        }

        bool checkPlayerInSight = false;
        foreach (GameObject obj in objects)
        {
            if (playerLayer == (playerLayer | (1 << obj.layer)))
            {
                checkPlayerInSight = true;
                break;
            }
        }
        playerInSight = checkPlayerInSight;
    }

    public GameObject GetClosestObject(){
        GameObject closestObject = null;
        float closestDistance = Mathf.Infinity;
        Vector3 aiPosition = cachedTransform.position;

        foreach (GameObject obj in objects)
        {
            float distance = Vector3.Distance(aiPosition, obj.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestObject = obj;
            }
        }

        return closestObject;
    }

    private void Scan(){
        colliders = new Collider[50];
        count = Physics.OverlapSphereNonAlloc(cachedTransform.position, distance, colliders, playerLayer, QueryTriggerInteraction.Collide);

        HashSet<GameObject> newObjects = new HashSet<GameObject>();
        for (int i = 0; i < count; ++i)
        {
            GameObject obj = colliders[i]?.gameObject;
            if (obj != null && IsInSight(obj))
            {
                if(obj.GetComponentInParent<AIAgentInterface>() == null)
                    newObjects.Add(obj);
                else{
                    if(!obj.GetComponentInParent<AIAgentInterface>().HasDied()){
                        newObjects.Add(obj);
                    }
                }
            }
        }

        // Remove objects that are no longer in sight
        foreach (GameObject obj in objects.ToList())
        {
            if (!newObjects.Contains(obj) || !IsInSight(obj))
            {
                objects.Remove(obj);
            }
            else if(obj.GetComponentInParent<AIAgentInterface>() != null){
                if(!obj.GetComponentInParent<AIAgentInterface>().HasDied()){
                    objects.Remove(obj);
                }
            }
        }

        // Add new objects in sight
        objects.UnionWith(newObjects);
    }

    public bool IsInSight(GameObject obj)
    {
        Vector3 origin = cachedTransform.position + cachedTransform.up * startHeight;
        Vector3 direction = obj.transform.position - origin;

        float deltaAngle = Vector3.Angle(direction, cachedTransform.forward);
        if (deltaAngle > angle)
        {
            return false;
        }

        RaycastHit hit;
        if (Physics.Linecast(origin, obj.transform.position, out hit, occlusionLayers))
        {
            if (hit.collider.gameObject != obj)
            {
                return false;
            }
        }

        if(direction.magnitude < distance / 5){
            playerIsClose = true;
        }

        else{
            playerIsClose = false;
        }

        if (direction.magnitude > distance)
        {
            return false;
        }

        return true;
    }

    Mesh CreateWedgeMesh(){
        Mesh mesh = new Mesh();

        int segments = 10;
        int numTrianges = (segments * 4) + 2 + 2;
        int numVertices = numTrianges * 3;

        Vector3[] vertices = new Vector3[numVertices];
        int[] trinagles = new int[numVertices];

        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward * distance;
        Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;

        Vector3 topCenter = bottomCenter + Vector3.up * startHeight;
        Vector3 topLeft = bottomLeft + Vector3.up * endHeight;
        Vector3 topRight = bottomRight + Vector3.up * endHeight;

        int vert = 0;

        //Left side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;
        
        vertices[vert++] = topLeft;
        vertices[vert++] = topCenter;
        vertices[vert++] = bottomCenter;

        //Right side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;
        
        vertices[vert++] = topRight;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCenter;

        float currentAngle = -angle;
        float deltaAngle = (angle * 2) / segments;
        for (int i = 0; i < segments; ++i)
        {
            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance;
            bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * distance;

            topLeft = bottomLeft + Vector3.up * endHeight;
            topRight = bottomRight + Vector3.up * endHeight;

             //Far side
            vertices[vert++] = bottomLeft;
            vertices[vert++] = bottomRight;
            vertices[vert++] = topRight;
            
            vertices[vert++] = topRight;
            vertices[vert++] = topLeft;
            vertices[vert++] = bottomLeft;

            //Top
            vertices[vert++] = topCenter;
            vertices[vert++] = topLeft;
            vertices[vert++] = topRight;

            //Bottom
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomLeft;

            currentAngle += deltaAngle;
        }
       

        for (int i = 0; i < numVertices; i++)
        {
            trinagles[i] = i;
        }

        mesh.vertices = vertices;
        mesh.triangles = trinagles;
        mesh.RecalculateNormals();

        return mesh;
    }

    private void OnValidate() {
        mesh = CreateWedgeMesh();
        scanInterval = 1.0f / scanFrequency;
    }

    private void OnDrawGizmos() {
        if (mesh){
            Gizmos.color = meshColor;
            Gizmos.DrawMesh(mesh, transform.position, transform.rotation);
        }

        Gizmos.DrawWireSphere(transform.position, distance);
        for(int i = 0; i< count; ++i){
            Gizmos.DrawSphere(colliders[i].transform.position, 0.2f);
        }

        Gizmos.color = Color.green;
        foreach (var obj in objects) {
            Gizmos.DrawSphere(obj.transform.position, 0.2f);
        }
    }

    public bool GetPlayerIsClose(){
        return playerIsClose;
    }

    public bool PlayerInSight => playerInSight;
}
