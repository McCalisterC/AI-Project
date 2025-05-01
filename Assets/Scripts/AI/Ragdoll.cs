using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    public Rigidbody[] rigidbodies;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        animator = GetComponent<Animator>();

        DeactivateRagdoll();
    }

    public void DeactivateRagdoll(){
        foreach(var rigidBody in rigidbodies){
            rigidBody.isKinematic = true;
        }
        animator.enabled = true;
    }

    public void ActivateRagdoll(){
        foreach(var rigidBody in rigidbodies){
            rigidBody.isKinematic = false;
        }
        animator.enabled = false;
    }

    public void ApplyForce(Vector3 force){
        var rigidBody = animator.GetBoneTransform(HumanBodyBones.Hips).GetComponent<Rigidbody>();
        rigidBody.AddForce(force, ForceMode.VelocityChange);
    }

    public void ApplyForce(Vector3 grenadePos, float grenadeRadius)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, grenadeRadius);
        foreach (var collider in colliders)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(50, transform.position, grenadeRadius, 2, ForceMode.Impulse);
            }
        }
    }
}
