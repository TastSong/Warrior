using UnityEngine;
using System.Collections;

public class SpawnPointPlayer : SpawnPoint
{
    // Use this for initialization
    void Start()
    {
        enabled = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }

}
