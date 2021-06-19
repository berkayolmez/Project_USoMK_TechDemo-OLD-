using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFieldOfView : MonoBehaviour
{
	public float viewRadius;
	[Range(0, 360)]
	public float viewAngle;

	public LayerMask targetMask;
	public LayerMask obstacleMask;

	[HideInInspector]
	public Transform currentTarget;

	public List<Transform> visibleTargets = new List<Transform>();

	[Range(1, 5)]
	public float meshResolution;
	public int edgeResolveIterations;
	public float edgeDstThreshold;

	public GameObject viewMeshObj;
	private MeshFilter viewMeshFilter;
	private Mesh viewMesh;

	void Start()
	{

		StartCoroutine("FindTargetsWithDelay", .3f);
	}

	IEnumerator FindTargetsWithDelay(float delay)
	{
		while (true)
		{
			yield return new WaitForSeconds(delay);
			FindVisibleTargets();
		}
	}
	public void FindVisibleTargets()
	{
		visibleTargets.Clear();
		currentTarget = null;

		Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask); //360 derece icindeki hedefleri buluyor			

		for (int i = 0; i < targetsInViewRadius.Length; i++)
		{
			Transform target = targetsInViewRadius[i].transform;
			Vector3 dirToTarget = (target.position - transform.position).normalized;
			if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
			{
				float dstToTarget = Vector3.Distance(transform.position, target.position);
				if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
				{
					visibleTargets.Add(target);
					currentTarget = target;
				}
			}
		}
	}

}


