using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FieldOfView : MonoBehaviour {

	public bool extinguisherTime = false;

	public float viewRadius;
	[Range(0,360)]
	public float viewAngle;

	public LayerMask targetMask;
	public LayerMask obstacleMask;
	public bool timeToGo;

	/*void Update() {
		RaycastHit hit;
		if (Physics.Raycast (transform.position, Vector3.forward, out hit, targetMask)) {
			Vector3 dirToTarget = (hit.transform.position - transform.position).normalized;
			float dstToTarget = Vector3.Distance (transform.position, hit.transform.position);
			if (!Physics.Raycast (transform.position, dirToTarget, dstToTarget, obstacleMask)) {
				if (gameObject.GetComponent<Patroling> () != null) {
					gameObject.GetComponent<Patroling> ().fire = true;
				}
				gameObject.GetComponent<Unit> ().repath = true;
			}
		}
	}*/

	//public List<Transform> visibleTargets = new List<Transform> ();

	void Start() {
		StartCoroutine ("FindTargetsWithDelay", 0.2f);
	}

	IEnumerator FindTargetsWithDelay(float delay) {
		while (!extinguisherTime) {
			yield return new WaitForSeconds (delay);
			FindVisibleTargets ();
		}
	}

	void FindVisibleTargets() {
		//visibleTargets.Clear ();
		Collider[] targetsInViewRadius = Physics.OverlapSphere (transform.position, viewRadius, targetMask);

		for (int i = 0; i < targetsInViewRadius.Length; i++) {
			Transform target = targetsInViewRadius [i].transform;
			Vector3 dirToTarget = (target.position - transform.position).normalized;
			if (Vector3.Angle (transform.forward, dirToTarget) < viewAngle / 2) {
				float dstToTarget = Vector3.Distance (transform.position, target.position);
				if (!Physics.Raycast (transform.position, dirToTarget, dstToTarget, obstacleMask)) {
					gameObject.GetComponent<Unit> ().repath = true;
					timeToGo = true;
					if (gameObject.GetComponent<Patroling> () != null) {
						gameObject.GetComponent<Patroling> ().fire = true;
					}
					break;
					//visibleTargets.Add (target);
				}
			}
		}
	}

	public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) {
		if (!angleIsGlobal) {
			angleInDegrees += transform.eulerAngles.y;
		}
		return new Vector3 (Mathf.Sin (angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos (angleInDegrees * Mathf.Deg2Rad));
	}
}
