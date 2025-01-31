﻿using System.Collections;
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
        mObstacleAvoider.mWeight = 0.0f;
        mSeekAndArrive.mWeight = 0.0f;
        mWander.mWeight = 0.0f;
        mNavMeshPather.mWeight = 0.0f;
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
        mNavMeshPather.mWeight = 1.5f;
        mCurrentPathIndex = TerrainManager.Instance.GetClosestPathIndex(transform.position);
        mNavMeshPather.CalculateNewPath(TerrainManager.Instance.mPathFollowPaths[mCurrentPathIndex].position);
    }

    void ActivatePathFollowAvoider()
    {
        mSeekAndArrive.gameObject.SetActive(true);
        mObstacleAvoider.gameObject.SetActive(true);
        mSeekAndArrive.mWeight = 1.5f;
        mObstacleAvoider.mWeight = 2.5f;
        mCurrentPathIndex = TerrainManager.Instance.GetClosestPathIndex(transform.position);
        mSeekAndArrive.CalculateNewPath(TerrainManager.Instance.mPathFollowPaths[mCurrentPathIndex].position);
    }

    void ActivateWanderer()
    {
        mWander.gameObject.SetActive(true);
        mSeekAndArrive.gameObject.SetActive(true);
        mObstacleAvoider.gameObject.SetActive(true);
        SetSeekMode();
    }

    void NavMeshPatherUpdate()
    {
        if(mNavMeshPather.mPath == null)
        {
            mCurrentPathIndex = (mCurrentPathIndex + 1) % TerrainManager.Instance.mPathFollowPaths.Length;
            mNavMeshPather.CalculateNewPath(TerrainManager.Instance.mPathFollowPaths[mCurrentPathIndex].position);
        }
    }

    void PathFollowAvoiderUpdate()
    {
        if(mSeekAndArrive.mPathComplete)
        {
            mCurrentPathIndex = (mCurrentPathIndex + 1) % TerrainManager.Instance.mPathFollowPaths.Length;
            mSeekAndArrive.CalculateNewPath(TerrainManager.Instance.mPathFollowPaths[mCurrentPathIndex].position);
        }
    }

    void WandererUpdate()
    {
        if(mSeekAndArrive.mWeight > 0.0f)
        {
            if (mSeekAndArrive.mPathComplete)
            {
                mWander.mWeight = 1.5f;
                mSeekAndArrive.mWeight = 0.0f;
                mWander.SetWanderTarget();
            }
        }
        else
        {
            if(!TerrainManager.Instance.mOuterBounds.Contains(transform.position))
            {
                SetSeekMode();
            }
        }
    }

    void SetSeekMode()
    {
        mWander.mWeight = 0.5f;
        mSeekAndArrive.mWeight = 1.5f;
        mObstacleAvoider.mWeight = 2.5f;
        Vector3 aRandomSeekPoint = TerrainManager.Instance.GetRandomInnerBoundPoint();
        mSeekAndArrive.CalculateNewPath(aRandomSeekPoint);
        mWander.CalculateNewPath(aRandomSeekPoint);
    }

}
