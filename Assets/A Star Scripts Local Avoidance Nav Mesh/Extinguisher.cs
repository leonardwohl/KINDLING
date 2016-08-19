using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Extinguisher : MonoBehaviour {

	public List <GameObject> wannabeFireman;
	public bool got = false;
	public GameObject hasMe;

	// Use this for initialization
	void Start () {
		wannabeFireman = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		if (got) {
			foreach (GameObject fireman in wannabeFireman) {
				fireman.GetComponent<Unit> ().extinguisherTaken = true;
			}
			if (hasMe != null) {
				gameObject.transform.position = hasMe.transform.position;
			}
		}
	}
}
