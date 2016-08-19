using UnityEngine;
using System.Collections;

//THIS ONLY WORKS FOR A PATROL BETWEEN TWO POINTS
//FOR NOW

public class Patrol : MonoBehaviour {
	public Transform[] targets;
	NavMeshAgent agent;
	Vector3 currentTarget;

	void Start () {
		agent = gameObject.GetComponent<NavMeshAgent> ();
		currentTarget = targets [1].position;
		agent.SetDestination (currentTarget);
	}

	void Update () {
		if (Vector3.Distance (transform.position, currentTarget) < 1) {
			if (currentTarget == targets [1].position) {
				currentTarget = targets [0].position;
			} else {
				currentTarget = targets [1].position;
			}
			agent.SetDestination (currentTarget);
		}
	}
}
