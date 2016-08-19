using UnityEngine;
using System.Collections;

public class PassingExit : MonoBehaviour {

	public Transform target;
	
	void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag == "agent") {
			col.gameObject.GetComponent<Unit> ().target = target;
			col.gameObject.GetComponent<Unit> ().nearExit = true;
		}
	}
	void OnTriggerExit(Collider col) {
		if (col.gameObject.tag == "agent") {
			col.gameObject.GetComponent<Unit> ().SignalOnceNearExit = false;
		}
	}
}
