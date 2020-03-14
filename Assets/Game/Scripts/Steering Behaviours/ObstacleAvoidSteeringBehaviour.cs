using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidSteeringBehaviour : SteeringBehaviourBase
{
    [System.Serializable]
    public struct Feeler
    {
        public float mDistance;
        public Vector3 mOffset;
    }


    [SerializeField] List<Feeler> mFeelers;

    [SerializeField] LayerMask mObstacleLayers;

    public override Vector3 CalculateForce()
    {
        RaycastHit aHit;
        Vector3 aFinalForce = Vector3.zero;
        foreach(Feeler aFeeler in mFeelers)
        {
            Vector3 aFPos = transform.rotation * aFeeler.mOffset + transform.position;
            if(Physics.Raycast(new Ray(aFPos, transform.forward), out aHit, aFeeler.mDistance,mObstacleLayers))
            {
                Vector3 aColliderPosition = aHit.collider.transform.position;
                if (TerrainManager.Instance.IsTerrain(aHit.collider))
                {
                    aColliderPosition = TerrainManager.Instance.findClosestTreePosition(aHit.point);
                }
                Vector3 aCollisionPoint = Vector3.Project(aColliderPosition - transform.position, transform.forward) + transform.position;
                float aAvoidanceStrength = 1.0f + (aCollisionPoint.magnitude - aFeeler.mDistance) / aFeeler.mDistance;
                aFinalForce += (aCollisionPoint - aColliderPosition).normalized * aAvoidanceStrength;
            }
        }
        return aFinalForce;
    }


    void OnDrawGizmos()
    {
        foreach(Feeler aFeeler in mFeelers)
        {
            Vector3 aFPos = transform.rotation * aFeeler.mOffset + transform.position;
            Debug.DrawLine(aFPos, aFPos + transform.forward * aFeeler.mDistance, Color.blue);
        }
    }
}
