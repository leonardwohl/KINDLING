using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Grid : MonoBehaviour {

	public bool displayGridGizmos;
	public LayerMask unwalkableMask;
	public Vector2 gridWorldSize;
	public float nodeRadius;
	public Node[,] grid;
	public GameObject[,] fireForExtinguisher;
	public List<Transform> targetExits = new List<Transform>();

	private RaycastHit hit;
	public LayerMask blockedLayer;
	public bool oneFire = false;
	public int twoFire = 0;
	public GameObject block;

	float nodeDiameter;
	public int gridSizeX, gridSizeY;
	private Vector3 worldBottomLeft;

	//float time = 0;

	public LayerMask doorLayer;

	public GameObject fireParticle;

	//List<Node> onFire = new List<Node>();

	void Start() {
		//StartCoroutine ("SpreadFire");
	}

	void ShuffleArray<T>(T[] array)
	{
		int n = array.Length;
		for (int i = 0; i < n; i++) {
			// Pick a new index higher than current for each item in the array
			int r = i + Random.Range(0, n - i);

			// Swap item into new spot
			T t = array[r];
			array[r] = array[i];
			array[i] = t;
		}
	}

	void Awake() {
		nodeDiameter = nodeRadius*2;
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter);
		fireForExtinguisher = new GameObject[gridSizeX, gridSizeY];
		CreateGrid();
	}

	IEnumerator SpreadFire(List<Node> onFire) {
		float time = 0;
		while (true) {
			time += Time.deltaTime;
			int breakTime = 0;
			if (time >= 1) {
				List <Node> spread = new List<Node> ();
				List <Node> onFireCopy = new List<Node> (onFire);
				List <Node> stayAway = new List<Node> ();
				foreach (Node hope in onFire) {
					if (hope.door) {
						continue;
					}
					if (hope.extinguished) {
						onFireCopy.Remove (hope);
						continue;
					}
					spread = GetNeighboursFire (hope);
					foreach (Node blah in spread) {
						if (!blah.burning && !blah.extinguished) {
							fireForExtinguisher[blah.gridX,blah.gridY] = (GameObject)Instantiate (fireParticle, new Vector3 (blah.worldPosition.x, 0.5f, blah.worldPosition.z), Quaternion.identity);
							blah.burning = true;
							blah.walkable = false;
							stayAway = GetNeighboursFire (blah);
							foreach (Node test in stayAway) {
								test.walkable = false;
							}
						}
						if (!blah.extinguished) {
							onFireCopy.Add (blah);
						} else {
							onFireCopy.Remove (blah);
						}
					}
					onFireCopy.Remove (hope);
					breakTime++;
					if (breakTime >= 20) {
						yield return null;
					}
				}
				ShuffleArray (onFireCopy.ToArray ());
				onFire = onFireCopy;
				time = 0;
			}
			yield return null;
		}
	}

	public int MaxSize {
		get {
			return gridSizeX * gridSizeY;
		}
	}

	public void Update() {
		/*for (int x = 0; x < gridSizeX; x++) {
			for (int y = 0; y < gridSizeY; y++) {
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
				bool walkable = !(Physics.CheckSphere (worldPoint, nodeRadius, unwalkableMask));
				grid [x, y].walkable = walkable;
			}
		}*/
		/*time += Time.deltaTime;
		if (time >= 1) {
			List <Node> spread = new List<Node> ();
			List <Node> onFireCopy = new List<Node> (onFire);
			foreach (Node hope in onFire) {
				spread = GetNeighboursFire (hope);
				foreach (Node blah in spread) {
					if (!blah.burning) {
						Instantiate (fireParticle, new Vector3 (blah.worldPosition.x, 0.5f, blah.worldPosition.z), Quaternion.identity);
						blah.burning = true;
						blah.walkable = false;
					}
					onFireCopy.Add (blah);
				}
				onFireCopy.Remove (hope);
			}
			onFire = onFireCopy;
			time = 0;
		}*/
		gameObject.GetComponent<Pathfinding> ().initialPathsDone = true;
		if (twoFire < 2 && Input.GetMouseButtonDown(0) && SceneManager.GetActiveScene ().buildIndex != 3) {
			if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, 100)) {
				if (hit.transform.tag != "wall" && hit.transform.tag != "fire" && hit.transform.tag != "agent" && hit.transform.tag != "noBurn" && !NodeFromWorldPoint(hit.point).door) {
					Node test = NodeFromWorldPoint (hit.point);
					gameObject.GetComponent<GameOver> ().leaveTest = true;
					if (twoFire == 0) {
						gameObject.GetComponent<GameOver> ().fireSpawn1 = hit.point;
					} else if (twoFire == 1) {
						gameObject.GetComponent<GameOver> ().fireSpawn2 = hit.point;
					}
					test.burning = true;
					test.walkable = false;
					fireForExtinguisher[test.gridX,test.gridY] = (GameObject)Instantiate(fireParticle, new Vector3(test.worldPosition.x, 0.5f, test.worldPosition.z), Quaternion.identity);
					List<Node> onFire = new List<Node> ();
					onFire.Add (test);
					StartCoroutine ("SpreadFire", onFire);
					twoFire++;
					//oneFire = true;

					/*if (!hit.transform.GetComponent<FireSpread> ().burning) {
						twoFire++;
						hit.transform.GetComponent<FireSpread> ().Health = 0;
					}*/
				}
			}
		}
	}

	public void CreateGrid() {
		grid = new Node[gridSizeX,gridSizeY];
		worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2;

		for (int x = 0; x < gridSizeX; x ++) {
			for (int y = 0; y < gridSizeY; y ++) {
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
				bool walkable = !(Physics.CheckSphere(worldPoint,nodeRadius,unwalkableMask));
				grid[x,y] = new Node(walkable,worldPoint, x,y);
				/*grid[x,y].block = (GameObject)Instantiate (block, worldPoint, Quaternion.identity);
				grid [x, y].block.GetComponent<FireSpread> ().x = x;
				grid [x, y].block.GetComponent<FireSpread> ().y = y;*/
				grid [x, y].blocked = Physics.CheckSphere (worldPoint, nodeRadius, blockedLayer);
				Collider[] hitColliders = Physics.OverlapSphere (worldPoint, nodeRadius, doorLayer);
				for (int i = 0; i < hitColliders.Length; i++) {
					grid [x, y].door = true;
					hitColliders [i].gameObject.GetComponent<DoorGridLocation> ().gridLocation.Add (grid [x, y]);
				}
			}
		}
	}

	public List<Node> GetNeighbours(Node node) {
		List<Node> neighbours = new List<Node>();

		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if (x == 0 && y == 0)
					continue;

				int checkX = node.gridX + x;
				int checkY = node.gridY + y;

				if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) {
					neighbours.Add(grid[checkX,checkY]);
				}
			}
		}

		return neighbours;
	}

	public List<Node> GetNeighboursFire(Node node) {
		List<Node> neighbours = new List<Node>();

		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if (x == 0 && y == 0)
					continue;

				int checkX = node.gridX + x;
				int checkY = node.gridY + y;

				if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) {
					if (!grid [checkX, checkY].burning && !grid[checkX, checkY].blocked) {
						neighbours.Add (grid [checkX, checkY]);
					}
				}
			}
		}

		return neighbours;
	}

	public Node NodeFromWorldPoint(Vector3 worldPosition) {
		float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
		float percentY = (worldPosition.z + gridWorldSize.y/2) / gridWorldSize.y;
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.RoundToInt((gridSizeX-1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY-1) * percentY);
		return grid[x,y];
	}

	void OnDrawGizmos() {
		Gizmos.DrawWireCube(transform.position,new Vector3(gridWorldSize.x,1,gridWorldSize.y));
		if (grid != null && displayGridGizmos) {
			foreach (Node n in grid) {
				Gizmos.color = (n.walkable)?Color.white:Color.red;
				Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter-.1f));
			}
		}
	}
}