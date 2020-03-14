using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : Singleton<TerrainManager>
{
    private List<Vector3> treePositions;
    public List<Transform> mPathFollowPaths;
    public Bounds mInnerBounds;
    public Bounds mOuterBounds;
    [SerializeField] BoxCollider mInnerCollider;
    [SerializeField] BoxCollider mOuterCollider;

    void Start()
    {
        Vector3 myVec = Vector3.zero;
        mInnerBounds = mInnerCollider.bounds;
        mOuterBounds = mOuterCollider.bounds;
        TerrainData terrainData = Terrain.activeTerrain.terrainData;
        treePositions = new List<Vector3>(terrainData.treeInstances.Length);
        for (int i = 0; i < Terrain.activeTerrain.terrainData.treeInstances.Length; i++)
        {
            Vector3 treePosition = Terrain.activeTerrain.terrainData.treeInstances[i].position;

            treePosition.x = treePosition.x * terrainData.size.x + Terrain.activeTerrain.gameObject.transform.position.x;
            treePosition.y = 0.0f;
            treePosition.z = treePosition.z * terrainData.size.z + Terrain.activeTerrain.gameObject.transform.position.z;
            treePosition.y = Terrain.activeTerrain.SampleHeight(treePosition);

            treePositions.Add(treePosition);
        }
    }

    public Vector3 findClosestTreePosition(Vector3 position)
    {
        Vector3 closestPosition = Vector3.zero;
        float closestDistance = Mathf.Infinity;

        for (int i = 0; i < treePositions.Count; i++)
        {
            float distance = (treePositions[i] - position).sqrMagnitude;
            if (distance < closestDistance)
            {
                closestPosition = treePositions[i];
                closestDistance = distance;
            }
        }
        return closestPosition;
    }

    public int GetClosestPathIndex(Vector3 pPosition)
    {
        int aClosestIx = -1;
        float aClosestDistance = Mathf.Infinity;

        for (int aI = 0; aI < mPathFollowPaths.Count; aI++)
        {
            float aDistance = (mPathFollowPaths[aI].position - pPosition).sqrMagnitude;
            if (aDistance < aClosestDistance)
            {
                aClosestIx = aI;
                aClosestDistance = aDistance;
            }
        }
        return aClosestIx;
    }

    public bool IsTerrain(Collider pCollider)
    {
        return Terrain.activeTerrain.gameObject.GetInstanceID() == pCollider.gameObject.GetInstanceID();
    }
}
