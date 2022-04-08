using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public void OnDrawGizmos()
    {
        Gizmos.DrawSphere(gameObject.transform.position, 0.25f);
    }
}
