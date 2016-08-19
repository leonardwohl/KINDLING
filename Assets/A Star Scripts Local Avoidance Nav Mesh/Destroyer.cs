using UnityEngine;
using System.Collections;

public class Destroyer : MonoBehaviour {

	public Grid g;

	public float viewRadius;
	[Range(0,360)]
	public float viewAngle;

	public LayerMask targetMask;
	public LayerMask obstacleMask;
	public GameObject spray;

	//private float timeLimit = 0f;
	//private bool useful = true; 



	void Start(){
		g = GameObject.Find("A*").GetComponent<Grid>();
		StartCoroutine ("FindTargetsWithDelay", 0.001f);
	}	


	IEnumerator FindTargetsWithDelay(float delay) {
		while (true) {
			yield return new WaitForSeconds (delay);
			FindVisibleTargets ();
		}
	}

	void FindVisibleTargets() {
		//visibleTargets.Clear ();
		Collider[] targetsInViewRadius = Physics.OverlapSphere (transform.position, viewRadius, targetMask);
		if(gameObject.GetComponent<Unit>().haveExtinguisher){
			for (int i = 0; i < targetsInViewRadius.Length; i++) {
				Transform target = targetsInViewRadius [i].transform;
				Vector3 dirToTarget = (target.position - transform.position).normalized;
				if (Vector3.Angle (transform.forward, dirToTarget) < viewAngle / 2) {
					float dstToTarget = Vector3.Distance (transform.position, target.position);
					if (!Physics.Raycast (transform.position, dirToTarget, dstToTarget, obstacleMask)) {

						//timeLimit += Time.deltaTime; 
						Node fuego = g.NodeFromWorldPoint(target.position);
						fuego.burning = false;
						fuego.extinguished = true;
						Destroy(g.fireForExtinguisher[fuego.gridX,fuego.gridY]);
						Instantiate (spray, new Vector3 (fuego.worldPosition.x, 0.5f, fuego.worldPosition.z), Quaternion.identity);

					}
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