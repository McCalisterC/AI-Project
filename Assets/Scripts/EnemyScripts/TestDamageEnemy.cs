using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDamageEnemy : MonoBehaviour, IEnemy
{
    public void DealDamage()
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(int dealtDamage)
    {
        Debug.Log(dealtDamage);
    }
}
