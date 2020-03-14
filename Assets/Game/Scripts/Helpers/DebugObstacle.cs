using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugObstacle : MonoBehaviour
{
	void OnDrawGizmos()
	{
		DebugExtension.DebugWireSphere(transform.position, Color.white);
	}
}
