﻿using UnityEngine;
using System;
using System.Collections.Generic;
public class GenHelpers
{
    public static void Shuffle<T>(ref List<T> pList)
    {
        int aN = pList.Count;
        while (aN > 1)
        {
            aN--;
            int aK = UnityEngine.Random.Range(0,aN + 1);
            T aValue = pList[aK];
            pList[aK] = pList[aK];
            pList[aK] = aValue;
        }
    }
    public static Vector2 GetClosestPoint(Vector2 pOrigin, Vector2 pEnd, Vector2 pPoint)
    {
        Vector2 aDirection = pEnd - pOrigin;
        float aMaxMag = aDirection.magnitude;
        aDirection.Normalize();
        Vector2 aDistFromOrigin = pPoint - pOrigin;
        float aDotProduct = Vector2.Dot(aDistFromOrigin, aDirection);
        aDotProduct = Mathf.Clamp(aDotProduct, 0f, aMaxMag);
        return pOrigin + aDirection * aDotProduct;
    }

}

/// <summary>
/// Used for pooled objects that need to set some default functionality while they are inactive 
/// </summary>
public interface IInitable
{
    void Init();
}