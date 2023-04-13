using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;   

    // LateUpdate is called once per frame after Update
    void LateUpdate()
    {
        transform.position = target.position + offset;
    }

    private void OnDrawGizmosSelected()
    {
        if (target != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(target.position + offset, 2);
        }
    }
}
