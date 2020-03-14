using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public enum NPCMode
    {
        PathFollowNavMesh,
        PathFollowObstacleAvoid,
        WanderObstacleAvoid
    }

    [SerializeField] NPCMode mNPCMode;
    NPCMode mCurrentMode;
    SteeringAgent mAgent;
    [SerializeField] ObstacleAvoidSteeringBehaviour mObstacleAvoider;
    [SerializeField] ArriveSteeringBehaviour mSeekAndArrive;
    [SerializeField] WanderSteeringBehaviour mWander;
    [SerializeField] PathFollowSteeringBehaviour mNavMeshPather;

    int mCurrentPathIndex = -1;

    void Start()
    {
        mAgent = GetComponent<SteeringAgent>();
        mCurrentMode = mNPCMode;
        ChangeMode();
    }

    void Update()
    {
        if(mCurrentMode != mNPCMode)
        {
            mCurrentMode = mNPCMode;
            ChangeMode();
        }
        switch(mCurrentMode)
        {
            case NPCMode.PathFollowNavMesh:
                {
                    NavMeshPatherUpdate();
                    break;
                }
            case NPCMode.PathFollowObstacleAvoid:
                {
                    PathFollowAvoiderUpdate();
                    break;
                }
            case NPCMode.WanderObstacleAvoid:
                {
                    WandererUpdate();
                    break;
                }
        }
    }

    void ChangeMode()
    {
        mAgent.mActive = false;
        mObstacleAvoider.gameObject.SetActive(false);
        mSeekAndArrive.gameObject.SetActive(false);
        mWander.gameObject.SetActive(false);
        mNavMeshPather.gameObject.SetActive(false);
        switch (mCurrentMode)
        {
            case NPCMode.PathFollowNavMesh:
                {
                    ActivateNavMeshPather();
                    break;
                }
            case NPCMode.PathFollowObstacleAvoid:
                {
                    ActivatePathFollowAvoider();
                    break;
                }
            case NPCMode.WanderObstacleAvoid:
                {
                    ActivateWanderer();
                    break;
                }
        }
        mAgent.mActive = true;
    }


    void ActivateNavMeshPather()
    {
        mNavMeshPather.gameObject.SetActive(true);
        mCurrentPathIndex = TerrainManager.Instance.GetClosestPathIndex(transform.position);
        mNavMeshPather.CalculateNewPath(TerrainManager.Instance.mPathFollowPaths[mCurrentPathIndex].position);
    }

    void ActivatePathFollowAvoider()
    {
        mSeekAndArrive.gameObject.SetActive(true);
        mObstacleAvoider.gameObject.SetActive(true);
        mCurrentPathIndex = TerrainManager.Instance.GetClosestPathIndex(transform.position);
        mSeekAndArrive.CalculateNewPath(TerrainManager.Instance.mPathFollowPaths[mCurrentPathIndex].position);
    }

    void ActivateWanderer()
    {

    }

    void NavMeshPatherUpdate()
    {
        if(mNavMeshPather.mPath == null)
        {
            mCurrentPathIndex = (mCurrentPathIndex + 1) % TerrainManager.Instance.mPathFollowPaths.Count;
            mNavMeshPather.CalculateNewPath(TerrainManager.Instance.mPathFollowPaths[mCurrentPathIndex].position);
        }
    }

    void PathFollowAvoiderUpdate()
    {
        if(mSeekAndArrive.mPathComplete)
        {
            mCurrentPathIndex = (mCurrentPathIndex + 1) % TerrainManager.Instance.mPathFollowPaths.Count;
            mSeekAndArrive.CalculateNewPath(TerrainManager.Instance.mPathFollowPaths[mCurrentPathIndex].position);
        }
    }

    void WandererUpdate()
    {

    }

}
