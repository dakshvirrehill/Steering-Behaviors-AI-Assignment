using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : Singleton<TerrainManager>
{
    private List<Vector3> treePositions;

    void Start()
    {
        Vector3 myVec = Vector3.zero;

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
}
