using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider))]
public class TriggerCameraShowSomething : MonoBehaviour 
{
    [System.Serializable]
    public class Waypoint
    {
        public Transform Transform;
        public float Time = 1;
        public GameObject TrackObject;
    }

    public List<Waypoint> WayPoints;
    public bool DisableAfterUse = false;


    // We'll draw a gizmo in the scene view, so it can be found....
    void OnDrawGizmos()
    {
        if (WayPoints == null)
            return;
        
        Gizmos.color = Color.white;


        Gizmos.DrawWireCube((GetComponent<Collider>() as BoxCollider).center + transform.position, (GetComponent<Collider>() as BoxCollider).size);

        for (int i = 0; i < WayPoints.Count; i++)
        {
            Gizmos.DrawSphere(WayPoints[i].Transform.position, 0.5f);

            if (WayPoints[i].TrackObject != null)
            {
                Gizmos.DrawWireSphere(WayPoints[i].TrackObject.transform.position, 0.2f);
                Gizmos.DrawLine(WayPoints[i].Transform.position, WayPoints[i].TrackObject.transform.position);
            }

        }

    }

    void OnDrawGizmosSelected()
    {
        if (WayPoints == null)
            return;

        Gizmos.color = Color.yellow;


        Gizmos.DrawWireCube((GetComponent<Collider>() as BoxCollider).center + transform.position, (GetComponent<Collider>() as BoxCollider).size);

        for (int i = 0; i < WayPoints.Count; i++)
        {
            Gizmos.DrawSphere(WayPoints[i].Transform.position, 0.5f);

            if (WayPoints[i].TrackObject != null)
            {
                Gizmos.DrawSphere(WayPoints[i].TrackObject.transform.position, 0.2f);
                Gizmos.DrawLine(WayPoints[i].Transform.position, WayPoints[i].TrackObject.transform.position);
            }

        }

    }


    public void Disable()
    {
        gameObject.SetActive(false);
    }


}
