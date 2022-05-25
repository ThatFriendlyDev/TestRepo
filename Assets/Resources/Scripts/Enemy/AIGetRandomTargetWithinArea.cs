using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGetRandomTargetWithinArea : MonoBehaviour
{
	[SerializeField]
	private Transform area;

	[SerializeField]
	private Transform target;

	public Vector3 GetRandomTargetFromArea()
	{
		target.position = area.position - new Vector3(Random.Range(area.localScale.x, -area.localScale.x), 0, Random.Range(area.localScale.x, -area.localScale.x));
		return target.position;
	}
}
