﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SteeringBehaviourBase : MonoBehaviour
{
	public float weight = 1.0f;
	public Vector3 target = Vector3.zero;
	public bool useMouseInput = true;

	public abstract Vector3 calculateForce();

	[HideInInspector] public SteeringAgent steeringAgent;
}
