using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {

	public bool pathFinished = false;
	public bool extinguisherTaken = false;
	public bool haveExtinguisher = false;
	public bool currentlyActive = true;

	public Transform[] targets;
	public Transform target;
	public float speed = 50;
	Vector3[] path;
	int targetIndex;
	public bool repath = false;
	public bool done = false;
	private NavMeshAgent agent;
	bool updateNavPath = true;
	float time = 0;
	float heatMapTime = 0;
	bool foundTarget = false;
	float pathLength = float.MaxValue;
	int closestTarget;
	int currentTarget = 0;
	public bool nearExit = false;
	public bool SignalOnceNearExit = false;

	public LayerMask extinguisherMask;
	public LayerMask fireMask;
	public Transform extinguisher;
	bool gettingExtinguisher = false;

	void Start() {
		if (targets.Length == 0) {
			targets = new Transform[GameObject.Find ("A*").GetComponent<Grid> ().targetExits.ToArray ().Length];
			targets = GameObject.Find ("A*").GetComponent<Grid> ().targetExits.ToArray();
		}
		agent = gameObject.GetComponent<NavMeshAgent> ();
		closestTarget = 0;
		PathRequestManager.RequestPath(gameObject,targets[currentTarget].position, OnPathFound);
	}

	void Update() {
		time += Time.deltaTime;
		heatMapTime += Time.deltaTime;

		if (heatMapTime > 3) {
			heatMapTime = 0;
			GameObject.Find ("A*").GetComponent<GameOver> ().heatmapPoints.Add (transform.position);
		}

		if (gettingExtinguisher && !extinguisherTaken && !done) {
			if (Physics.CheckSphere (transform.position, gameObject.GetComponent<CapsuleCollider> ().radius, fireMask)) {
				done = true;
				if (agent != null) {
					agent.Stop ();
				}
				GameObject.Find ("A*").GetComponent<GameOver> ().done++;
				GameObject.Find ("A*").GetComponent<GameOver> ().count++;
			} else if (Vector3.Distance (transform.position, new Vector3 (extinguisher.position.x, 1, extinguisher.position.z)) < 3) {
				if (!extinguisher.gameObject.GetComponent<Extinguisher> ().got) {
					extinguisher.gameObject.GetComponent<Extinguisher> ().got = true;
					gameObject.GetComponent<Renderer> ().material.color = Color.blue;
					agent.SetDestination (target.position);
					haveExtinguisher = true;
					extinguisher.gameObject.GetComponent<Extinguisher> ().hasMe = gameObject;
				}
			}
		}

		if (haveExtinguisher) {
			agent.SetDestination (target.position);
		}

		if (gettingExtinguisher && extinguisherTaken && !haveExtinguisher && !done) {
			if (Physics.CheckSphere (transform.position, gameObject.GetComponent<CapsuleCollider> ().radius, fireMask)) {
				done = true;
				if (agent != null) {
					agent.Stop ();
				}
				GameObject.Find ("A*").GetComponent<GameOver> ().done++;
				GameObject.Find ("A*").GetComponent<GameOver> ().count++;
			} else {
				agent.SetDestination (extinguisher.position);
			}
		}

		if (nearExit && !SignalOnceNearExit && pathFinished && !gettingExtinguisher && !done) {
			nearExit = false;
			SignalOnceNearExit = true;
			StopCoroutine ("FollowPath");
			targetIndex = 0;
			pathFinished = false;
			PathRequestManager.RequestPath(gameObject,target.position, OnPathFound);
			updateNavPath = true;
			time = 0;
		}

		if (repath && !done && time >= 1 && pathFinished && !done) {
			repath = false;
			pathFinished = false;
			StopCoroutine ("FollowPath");
			targetIndex = 0;
			PathRequestManager.RequestPath(gameObject,target.position, OnPathFound);
			updateNavPath = true;
			time = 0;
		}
		if (Vector3.Distance(transform.position, target.position) < 3 && !done)
        {
            done = true;
			GameObject.Find("A*").GetComponent<GameOver>().done++;
			if (extinguisher != null) {
				extinguisher.gameObject.GetComponent<Extinguisher> ().wannabeFireman.Remove (gameObject);
			}
			StopCoroutine ("FollowPath");
			Destroy (gameObject);
        }
	}

	public void OnPathFound(Vector3[] newPath, bool pathSuccessful) {
		pathFinished = true;
		if (pathSuccessful && foundTarget) {
            path = newPath;
			if (path != null && agent != null) {
				StopCoroutine ("FollowPath");
				targetIndex = 0;
				StartCoroutine ("FollowPath");
			}
		} else if (foundTarget && !gettingExtinguisher) {
			path = newPath;
			bool noExtinguisher = true;
			if (path.Length == 0 && agent != null) {
				agent.Stop ();
				Collider[] targetsInViewRadius = Physics.OverlapSphere (transform.position, gameObject.GetComponent<FieldOfView>().viewRadius, extinguisherMask);
				for (int i = 0; i < targetsInViewRadius.Length; i++) {
					Transform extinguisherTarget = targetsInViewRadius [i].transform;
					Vector3 dirToTarget = (extinguisherTarget.position - transform.position).normalized;
					float dstToTarget = Vector3.Distance (transform.position, extinguisherTarget.position);
					if (!Physics.CheckSphere (transform.position, gameObject.GetComponent<CapsuleCollider> ().radius, fireMask)) {
						if (!Physics.Raycast (transform.position, dirToTarget, dstToTarget, fireMask)) {
							/*agent.SetDestination (extinguisherTarget.position);
							agent.Resume ();
							extinguisherTarget.gameObject.GetComponent<Extinguisher> ().wannabeFireman.Add (gameObject);
							noExtinguisher = false;
							extinguisher = extinguisherTarget;
							gettingExtinguisher = true;
							gameObject.GetComponent<FieldOfView> ().extinguisherTime = true;
							break;*/
							StopCoroutine ("FollowPath");
							extinguisherTarget.gameObject.GetComponent<Extinguisher> ().wannabeFireman.Add (gameObject);
							noExtinguisher = false;
							extinguisher = extinguisherTarget;
							gettingExtinguisher = true;
							gameObject.GetComponent<FieldOfView> ().extinguisherTime = true;
							pathFinished = false;
							//PathRequestManager.RequestPath (gameObject, extinguisher.position, OnPathFound);
							agent.SetDestination (extinguisher.transform.position);
							agent.Resume ();
							break;
						}
					} else {
						StopCoroutine ("FollowPath");
					}
				}
				if (noExtinguisher) {
					StopCoroutine("FollowPath");
					done = true;
					GameObject.Find ("A*").GetComponent<GameOver> ().done++;
					GameObject.Find ("A*").GetComponent<GameOver> ().count++;
				}
			}
        }
		if (!foundTarget) {
			float tempPathLength = 0;
			for (int i = 0; i < newPath.Length-1; i++) {
				tempPathLength += Vector3.Distance(newPath[i], newPath[i+1]);
			}
			if (tempPathLength < pathLength) {
				pathLength = tempPathLength;
				closestTarget = currentTarget;
			}
			currentTarget++;
			if (currentTarget < targets.Length) {
				PathRequestManager.RequestPath (gameObject, targets [currentTarget].position, OnPathFound);
			} else {
				target = targets [closestTarget];
				PathRequestManager.RequestPath (gameObject, target.position, OnPathFound);
				foundTarget = true;
			}
		}
	}

	IEnumerator FollowPath() {
		while(!gameObject.GetComponent<FieldOfView>().timeToGo){
		//while (!GameObject.Find("A*").GetComponent<GameOver>().leaveTest) {
			yield return new WaitForSeconds(0.000000001f);
		}
        if (path.Length > 0)
        {
            Vector3 currentWaypoint = path[0];
            targetIndex = 0;
			while (true && currentlyActive)
            {
				if (Vector3.Distance(transform.position, new Vector3(currentWaypoint.x, 1, currentWaypoint.z)) < 3) 
				//if (transform.position == new Vector3(currentWaypoint.x, 1, currentWaypoint.z))
                {
                    targetIndex++;
                    if (targetIndex >= path.Length)
                    {
                        targetIndex = 0;
                        path = new Vector3[0];
                        yield break;
                    }
                    currentWaypoint = path[targetIndex];
					updateNavPath = true;
                }
				if (updateNavPath && currentlyActive) {
					if (agent == null) {
						yield return null;
					} else {
						agent.SetDestination (new Vector3 (currentWaypoint.x, 1, currentWaypoint.z));
						updateNavPath = false;
					}
				}
                //transform.position = Vector3.MoveTowards(transform.position, new Vector3(currentWaypoint.x, 1, currentWaypoint.z), speed * Time.deltaTime);
                yield return null;
            }
        }
	}

	public void OnDrawGizmos() {
		if (path != null) {
			for (int i = targetIndex; i < path.Length; i ++) {
				Gizmos.color = Color.black;
				Gizmos.DrawCube(path[i], Vector3.one);

				if (i == targetIndex) {
					Gizmos.DrawLine(transform.position, path[i]);
				}
				else {
					Gizmos.DrawLine(path[i-1],path[i]);
				}
			}
		}
	}
	void OnDisable() {
		currentlyActive = false;
	}
}
