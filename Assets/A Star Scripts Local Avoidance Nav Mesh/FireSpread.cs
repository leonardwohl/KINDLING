using UnityEngine;
using System.Collections;

public class FireSpread : MonoBehaviour {

	public float Health = 2f;
	public bool burning = false;
	public bool colorOnce = false;
	public int x;
	public int y;
	private bool countdown = true;
	public float timeLeft = 20f;
	public float healthLoss;
	public GameObject fireParticle;
	GameObject spawnedFire;
	Node[,] grid;
	int gridSizeX, gridSizeY;
	public bool blocked = false;
	//have grid provide the indexes once the thing is burning

	void Start() {
		grid = GameObject.Find ("A*").GetComponent<Grid> ().grid;
		gridSizeX = GameObject.Find ("A*").GetComponent<Grid> ().gridSizeX;
		gridSizeY = GameObject.Find ("A*").GetComponent<Grid> ().gridSizeY;
	}

	void Update () {
		if (!countdown) {
			
			//no renderer attached to prefab
			//gameObject.GetComponent<Renderer> ().material.color = Color.gray;
			Destroy (spawnedFire);
		}
		if (burning && countdown) {
			timeLeft -= Time.deltaTime;
			if (timeLeft < 0) {
				countdown = false;
			}
		}
		if (Health <= 0) {
			burning = true;
		}
		if (burning && !colorOnce) {
			//gameObject.GetComponent<Renderer> ().material.color = Color.red;
			spawnedFire = (GameObject)Instantiate(fireParticle, new Vector3(gameObject.transform.position.x, 0.5f, gameObject.transform.position.z), Quaternion.identity);
			colorOnce = true;
		}
		if (burning && countdown) {
			if (x > 0 && x < gridSizeX - 1) {
				if (y > 0 && y < gridSizeY - 1) {
					if (!grid [x - 1, y].block.GetComponent<FireSpread> ().blocked) {
						grid [x - 1, y].block.GetComponent<FireSpread> ().Health -= Time.deltaTime * Time.deltaTime * healthLoss;
					}
					if (!grid [x + 1, y].block.GetComponent<FireSpread> ().blocked) {
						grid [x + 1, y].block.GetComponent<FireSpread> ().Health -= Time.deltaTime * healthLoss;
					}
					if (!grid [x, y - 1].block.GetComponent<FireSpread> ().blocked) {
						grid [x, y - 1].block.GetComponent<FireSpread> ().Health -= Time.deltaTime * healthLoss;
					}
					if (!grid [x, y + 1].block.GetComponent<FireSpread> ().blocked) {
						grid [x, y + 1].block.GetComponent<FireSpread> ().Health -= Time.deltaTime * healthLoss;
					}
					if (!grid [x - 1, y - 1].block.GetComponent<FireSpread> ().blocked) {
						grid [x - 1, y - 1].block.GetComponent<FireSpread> ().Health -= Time.deltaTime * healthLoss;
					}
					if (!grid [x - 1, y + 1].block.GetComponent<FireSpread> ().blocked) {
						grid [x - 1, y + 1].block.GetComponent<FireSpread> ().Health -= Time.deltaTime * healthLoss;
					}
					if (!grid [x + 1, y - 1].block.GetComponent<FireSpread> ().blocked) {
						grid [x + 1, y - 1].block.GetComponent<FireSpread> ().Health -= Time.deltaTime * healthLoss;
					}
					if (!grid [x + 1, y + 1].block.GetComponent<FireSpread> ().blocked) {
						grid [x + 1, y + 1].block.GetComponent<FireSpread> ().Health -= Time.deltaTime * healthLoss;
					}
				} else if (y == 0) {
					if (!grid [x - 1, y].block.GetComponent<FireSpread> ().blocked) {
						grid [x - 1, y].block.GetComponent<FireSpread> ().Health -= Time.deltaTime * healthLoss;
					}
					if (!grid [x + 1, y].block.GetComponent<FireSpread> ().blocked) {
						grid [x + 1, y].block.GetComponent<FireSpread> ().Health -= Time.deltaTime * healthLoss;
					}
					if (!grid [x, y + 1].block.GetComponent<FireSpread> ().blocked) {
						grid [x, y + 1].block.GetComponent<FireSpread> ().Health -= Time.deltaTime * healthLoss;
					}
					if (!grid [x - 1, y + 1].block.GetComponent<FireSpread> ().blocked) {
						grid [x - 1, y + 1].block.GetComponent<FireSpread> ().Health -= Time.deltaTime * healthLoss;
					}
					if (!grid [x + 1, y + 1].block.GetComponent<FireSpread> ().blocked) {
						grid [x + 1, y + 1].block.GetComponent<FireSpread> ().Health -= Time.deltaTime * healthLoss;
					}
				} else {
					if (!grid [x - 1, y].block.GetComponent<FireSpread> ().blocked) {
						grid [x - 1, y].block.GetComponent<FireSpread> ().Health -= Time.deltaTime * healthLoss;
					}
					if (!grid [x + 1, y].block.GetComponent<FireSpread> ().blocked) {
						grid [x + 1, y].block.GetComponent<FireSpread> ().Health -= Time.deltaTime * healthLoss;
					}
					if (!grid [x, y - 1].block.GetComponent<FireSpread> ().blocked) {
						grid [x, y - 1].block.GetComponent<FireSpread> ().Health -= Time.deltaTime * healthLoss;
					}
					if (!grid [x - 1, y - 1].block.GetComponent<FireSpread> ().blocked) {
						grid [x - 1, y - 1].block.GetComponent<FireSpread> ().Health -= Time.deltaTime * healthLoss;
					}
					if (!grid [x + 1, y - 1].block.GetComponent<FireSpread> ().blocked) {
						grid [x + 1, y - 1].block.GetComponent<FireSpread> ().Health -= Time.deltaTime * healthLoss;
					}
				}
			} else if (x == 0) {
				if (y > 0 && y < gridSizeY - 1) {
					if (!grid [x + 1, y].block.GetComponent<FireSpread> ().blocked) {
						grid [x + 1, y].block.GetComponent<FireSpread> ().Health -= Time.deltaTime * healthLoss;
					}
					if (!grid [x, y - 1].block.GetComponent<FireSpread> ().blocked) {
						grid [x, y - 1].block.GetComponent<FireSpread> ().Health -= Time.deltaTime * healthLoss;
					}
					if (!grid [x, y + 1].block.GetComponent<FireSpread> ().blocked) {
						grid [x, y + 1].block.GetComponent<FireSpread> ().Health -= Time.deltaTime * healthLoss;
					}
					if (!grid [x + 1, y - 1].block.GetComponent<FireSpread> ().blocked) {
						grid [x + 1, y - 1].block.GetComponent<FireSpread> ().Health -= Time.deltaTime * healthLoss;
					}
					if (!grid [x + 1, y + 1].block.GetComponent<FireSpread> ().blocked) {
						grid [x + 1, y + 1].block.GetComponent<FireSpread> ().Health -= Time.deltaTime * healthLoss;
					}
				} else if (y == 0) {
					if (!grid [x + 1, y].block.GetComponent<FireSpread> ().blocked) {
						grid [x + 1, y].block.GetComponent<FireSpread> ().Health -= Time.deltaTime * healthLoss;
					}
					if (!grid [x, y + 1].block.GetComponent<FireSpread> ().blocked) {
						grid [x, y + 1].block.GetComponent<FireSpread> ().Health -= Time.deltaTime * healthLoss;
					}
					if (!grid [x + 1, y + 1].block.GetComponent<FireSpread> ().blocked) {
						grid [x + 1, y + 1].block.GetComponent<FireSpread> ().Health -= Time.deltaTime * healthLoss;
					}
				} else {
					if (!grid [x + 1, y].block.GetComponent<FireSpread> ().blocked) {
						grid [x + 1, y].block.GetComponent<FireSpread> ().Health -= Time.deltaTime * healthLoss;
					}
					if (!grid [x, y - 1].block.GetComponent<FireSpread> ().blocked) {
						grid [x, y - 1].block.GetComponent<FireSpread> ().Health -= Time.deltaTime * healthLoss;
					}
					if (!grid [x + 1, y - 1].block.GetComponent<FireSpread> ().blocked) {
						grid [x + 1, y - 1].block.GetComponent<FireSpread> ().Health -= Time.deltaTime * healthLoss;
					}
				}
			} else {
				if (y > 0 && y < gridSizeY - 1) {
					if (!grid [x - 1, y].block.GetComponent<FireSpread> ().blocked) {
						grid [x - 1, y].block.GetComponent<FireSpread> ().Health -= Time.deltaTime * healthLoss;
					}
					if (!grid [x, y - 1].block.GetComponent<FireSpread> ().blocked) {
						grid [x, y - 1].block.GetComponent<FireSpread> ().Health -= Time.deltaTime * healthLoss;
					}
					if (!grid [x, y + 1].block.GetComponent<FireSpread> ().blocked) {
						grid [x, y + 1].block.GetComponent<FireSpread> ().Health -= Time.deltaTime * healthLoss;
					}
					if (!grid [x - 1, y - 1].block.GetComponent<FireSpread> ().blocked) {
						grid [x - 1, y - 1].block.GetComponent<FireSpread> ().Health -= Time.deltaTime * healthLoss;
					}
					if (!grid [x - 1, y + 1].block.GetComponent<FireSpread> ().blocked) {
						grid [x - 1, y + 1].block.GetComponent<FireSpread> ().Health -= Time.deltaTime * healthLoss;
					}
				} else if (y == 0) {
					if (!grid [x - 1, y].block.GetComponent<FireSpread> ().blocked) {
						grid [x - 1, y].block.GetComponent<FireSpread> ().Health -= Time.deltaTime * healthLoss;
					}
					if (!grid [x, y + 1].block.GetComponent<FireSpread> ().blocked) {
						grid [x, y + 1].block.GetComponent<FireSpread> ().Health -= Time.deltaTime * healthLoss;
					}
					if (!grid [x - 1, y + 1].block.GetComponent<FireSpread> ().blocked) {
						grid [x - 1, y + 1].block.GetComponent<FireSpread> ().Health -= Time.deltaTime * healthLoss;
					}
				} else {
					if (!grid [x - 1, y].block.GetComponent<FireSpread> ().blocked) {
						grid [x - 1, y].block.GetComponent<FireSpread> ().Health -= Time.deltaTime * healthLoss;
					}
					if (!grid [x, y - 1].block.GetComponent<FireSpread> ().blocked) {
						grid [x, y - 1].block.GetComponent<FireSpread> ().Health -= Time.deltaTime * healthLoss;
					}
					if (!grid [x - 1, y - 1].block.GetComponent<FireSpread> ().blocked) {
						grid [x - 1, y - 1].block.GetComponent<FireSpread> ().Health -= Time.deltaTime * healthLoss;
					}
				}
			}
		}
	}
}
