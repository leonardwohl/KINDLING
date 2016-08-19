using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

public class GameOver : MonoBehaviour {

	public int previousScene = 0;

	public GameObject wallPrefab;
	public GameObject agentPrefab;
	public GameObject doorPrefab;
	public GameObject goalPrefab;

	public string fileName = "saveFile.txt";
	public bool leaveTest = false;

    public int agentcount;
	public int done = 0;
	public Text Score;
	public Text Done;
	public float count = 0;
	public GameObject canvas;

	public bool finish = false;
	public List<Vector3> heatmapPoints;
	public GameObject heatPoint;
	public Vector3 fireSpawn1;
	public Vector3 fireSpawn2;
	public GameObject firePoint;
	public bool HeatMapMode = false;

	void Awake() {
		DontDestroyOnLoad (gameObject);
		heatmapPoints = new List<Vector3> ();
		if (GameObject.Find("ShowedHeatMap(Clone)") != null && (GameObject.Find("A*").GetComponent<GameOver>().previousScene == 2 || GameObject.Find("A*").GetComponent<GameOver>().previousScene == 4)) {
			//Destroy (GameObject.Find ("ShowedHeatMap(Clone)"));
			GameObject temp = null;
			if (File.Exists ("localOnlineFile.txt")) {
				string line;
				StreamReader theReader = new StreamReader("localOnlineFile.txt", Encoding.Default);
				using (theReader)
				{
					// While there's lines left in the text file, do this:
					do
					{
						line = theReader.ReadLine();

						if (line != null)
						{
							if (line == "13") {
								float xPos = float.Parse(theReader.ReadLine());
								float yPos = float.Parse(theReader.ReadLine());
								float zPos = float.Parse(theReader.ReadLine());
								float xRot = float.Parse(theReader.ReadLine());
								float yRot = float.Parse(theReader.ReadLine());
								float zRot = float.Parse(theReader.ReadLine());
								float xScale = float.Parse(theReader.ReadLine());
								float yScale = float.Parse(theReader.ReadLine());
								float zScale = float.Parse(theReader.ReadLine());
								temp = (GameObject)Instantiate (wallPrefab, new Vector3 (xPos, yPos, zPos), Quaternion.identity);
								temp.transform.rotation = Quaternion.Euler(xRot, yRot, zRot);
								temp.transform.localScale = new Vector3(xScale, yScale, zScale);
							} else if (line == "14") {
								float xPos = float.Parse(theReader.ReadLine());
								float yPos = float.Parse(theReader.ReadLine());
								float zPos = float.Parse(theReader.ReadLine());
								float xRot = float.Parse(theReader.ReadLine());
								float yRot = float.Parse(theReader.ReadLine());
								float zRot = float.Parse(theReader.ReadLine());
								float xScale = float.Parse(theReader.ReadLine());
								float yScale = float.Parse(theReader.ReadLine());
								float zScale = float.Parse(theReader.ReadLine());
								temp = (GameObject)Instantiate (agentPrefab, new Vector3 (xPos, yPos, zPos), Quaternion.identity);
								temp.transform.rotation = Quaternion.Euler(xRot, yRot, zRot);
								temp.transform.localScale = new Vector3(xScale, yScale, zScale);
							} else if (line == "15") {
								float xPos = float.Parse(theReader.ReadLine());
								float yPos = float.Parse(theReader.ReadLine());
								float zPos = float.Parse(theReader.ReadLine());
								float xRot = float.Parse(theReader.ReadLine());
								float yRot = float.Parse(theReader.ReadLine());
								float zRot = float.Parse(theReader.ReadLine());
								float xScale = float.Parse(theReader.ReadLine());
								float yScale = float.Parse(theReader.ReadLine());
								float zScale = float.Parse(theReader.ReadLine());
								temp = (GameObject)Instantiate (doorPrefab, new Vector3 (xPos, yPos, zPos), Quaternion.identity);
								temp.transform.rotation = Quaternion.Euler(xRot, yRot, zRot);
								temp.transform.localScale = new Vector3(xScale, yScale, zScale);
							} else if (line == "16") {
								float xPos = float.Parse(theReader.ReadLine());
								float yPos = float.Parse(theReader.ReadLine());
								float zPos = float.Parse(theReader.ReadLine());
								float xRot = float.Parse(theReader.ReadLine());
								float yRot = float.Parse(theReader.ReadLine());
								float zRot = float.Parse(theReader.ReadLine());
								float xScale = float.Parse(theReader.ReadLine());
								float yScale = float.Parse(theReader.ReadLine());
								float zScale = float.Parse(theReader.ReadLine());
								temp = (GameObject)Instantiate (goalPrefab, new Vector3 (xPos, yPos, zPos), Quaternion.identity);
								temp.transform.rotation = Quaternion.Euler(xRot, yRot, zRot);
								temp.transform.localScale = new Vector3(xScale, yScale, zScale);
							}
						}
					}
					while (line != null);
					theReader.Close();
				}
			}
		}
	}
	// Use this for initialization
	void Start () {
		Done.text = "";
	}
	
	// Update is called once per frame
	void Update () {
		if (finish) {
			/*finish = false;
			HeatMapMode = false;
			heatmapPoints.Clear ();
			gameObject.GetComponent<Grid> ().twoFire = 0;
			gameObject.GetComponent<Grid> ().enabled = false;
			gameObject.GetComponent<resize> ().enabled = true;
			gameObject.GetComponent<GameOver> ().enabled = false;*/
		}
		if (!finish) {
			GameObject temp = null;
			if (HeatMapMode) {
				Score = (Text)GameObject.Find ("Canvas").transform.FindChild ("Text").GetComponent<Text>();
				Score.text = "Score: " + count + " / " + agentcount;
				heatmapPoints.Distinct ().ToList ();
				StreamWriter sw = File.CreateText ("LocalHeatMap.txt");
				sw.Write (heatmapPoints.Count + "$");
				foreach (Vector3 heatSpot in heatmapPoints) {
					sw.Write (heatSpot.x + "$");
					sw.Write (heatSpot.y + "$");
					sw.Write (heatSpot.z + "$");
					temp = (GameObject)Instantiate (heatPoint, new Vector3 (heatSpot.x, heatSpot.y, heatSpot.z), Quaternion.identity);
					if (SceneManager.GetActiveScene ().buildIndex == 3) {
						GameObject.Find("ShowedHeatMap(Clone)").GetComponent<HeatMapData> ().heatList.Add (temp.transform.position);
					}
				}
				if (gameObject.GetComponent<Grid> ().twoFire != 0) {
					sw.Write (fireSpawn1.x + "$");
					sw.Write (fireSpawn1.y + "$");
					sw.Write (fireSpawn1.z + "$");
					temp = (GameObject)Instantiate (firePoint, new Vector3 (fireSpawn1.x, fireSpawn1.y, fireSpawn1.z), Quaternion.identity);
					if (SceneManager.GetActiveScene ().buildIndex == 3) {
						GameObject.Find("ShowedHeatMap(Clone)").GetComponent<HeatMapData> ().fireSpawn.Add (temp.transform.position);
					}
					if (gameObject.GetComponent<Grid> ().twoFire == 2) {
						sw.Write (fireSpawn2.x + "$");
						sw.Write (fireSpawn2.y + "$");
						sw.Write (fireSpawn2.z + "$");
						temp = (GameObject)Instantiate (firePoint, new Vector3 (fireSpawn2.x, fireSpawn2.y, fireSpawn2.z), Quaternion.identity);
						if (SceneManager.GetActiveScene ().buildIndex == 3) {
							GameObject.Find("ShowedHeatMap(Clone)").GetComponent<HeatMapData> ().fireSpawn.Add (temp.transform.position);
						}
					}
				}
				sw.Close();
				finish = true;
			}
			if (!HeatMapMode) {
				//float fps = 1 / Time.deltaTime;
				//print (fps);
				Score.text = "Score: " + count + " / " + agentcount;
				if (Input.GetKeyDown (KeyCode.Return) && Time.timeScale == 0) {
					SceneManager.LoadScene (0);
					//SceneManager.GetActiveScene ().buildIndex
					Time.timeScale = 1;
					done = 0;
					count = 0;
				}
			}
			if (agentcount == 0) {
				if (gameObject.GetComponent<resize> () != null) {
					agentcount = gameObject.GetComponent<resize> ().agentCounter;
				} else {
					agentcount = gameObject.GetComponent<OnlinePLay> ().agentCounter;
				}
			}
			if (done == agentcount && !HeatMapMode) {
				Done.text = "Game Over\n Press Enter to Continue";
				HeatMapMode = true;
				gameObject.GetComponent<Grid> ().StopCoroutine ("SpreadFire");
				if (SceneManager.GetActiveScene ().buildIndex == 2) {
					gameObject.GetComponent<resize> ().enabled = false;
				}
				previousScene = SceneManager.GetActiveScene ().buildIndex;
				Destroy (GameObject.Find ("ShowedHeatMap(Clone)"));
				SceneManager.LoadScene (3);
				/*gameObject.GetComponent<Grid> ().targetExits.Clear();
				HeatMapMode = true;
				gameObject.GetComponent<Grid> ().StopCoroutine ("SpreadFire");
				GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
				foreach (GameObject go in allObjects) {
					canvas.SetActive (true);
					if (go.tag != "MainCamera" && go.tag != "noSave" && go.tag != "void") {
						if (go.layer == 13) {
							Destroy (go);
						}
						if (go.layer == 14) {
							Destroy (go);
						}
						if (go.layer == 15) {
							Destroy (go);
						}
						if (go.layer == 16) {
							Destroy (go);
						}
						if (go.tag == "fire") {
							Destroy (go);
						}
					}
				}
				if (File.Exists (fileName)) {
					string line;
					StreamReader theReader = new StreamReader(fileName, Encoding.Default);
					using (theReader)
					{
						// While there's lines left in the text file, do this:
						do
						{
							line = theReader.ReadLine();

							if (line != null)
							{
								if (line == "13") {
									float xPos = float.Parse(theReader.ReadLine());
									float yPos = float.Parse(theReader.ReadLine());
									float zPos = float.Parse(theReader.ReadLine());
									float xRot = float.Parse(theReader.ReadLine());
									float yRot = float.Parse(theReader.ReadLine());
									float zRot = float.Parse(theReader.ReadLine());
									float xScale = float.Parse(theReader.ReadLine());
									float yScale = float.Parse(theReader.ReadLine());
									float zScale = float.Parse(theReader.ReadLine());
									temp = (GameObject)Instantiate (wallPrefab, new Vector3 (xPos, yPos, zPos), Quaternion.identity);
									temp.transform.rotation = Quaternion.Euler(xRot, yRot, zRot);
									temp.transform.localScale = new Vector3(xScale, yScale, zScale);
								} else if (line == "15") {
									float xPos = float.Parse(theReader.ReadLine());
									float yPos = float.Parse(theReader.ReadLine());
									float zPos = float.Parse(theReader.ReadLine());
									float xRot = float.Parse(theReader.ReadLine());
									float yRot = float.Parse(theReader.ReadLine());
									float zRot = float.Parse(theReader.ReadLine());
									float xScale = float.Parse(theReader.ReadLine());
									float yScale = float.Parse(theReader.ReadLine());
									float zScale = float.Parse(theReader.ReadLine());
									temp = (GameObject)Instantiate (doorPrefab, new Vector3 (xPos, yPos, zPos), Quaternion.identity);
									temp.transform.rotation = Quaternion.Euler(xRot, yRot, zRot);
									temp.transform.localScale = new Vector3(xScale, yScale, zScale);
								} else if (line == "16") {
									float xPos = float.Parse(theReader.ReadLine());
									float yPos = float.Parse(theReader.ReadLine());
									float zPos = float.Parse(theReader.ReadLine());
									float xRot = float.Parse(theReader.ReadLine());
									float yRot = float.Parse(theReader.ReadLine());
									float zRot = float.Parse(theReader.ReadLine());
									float xScale = float.Parse(theReader.ReadLine());
									float yScale = float.Parse(theReader.ReadLine());
									float zScale = float.Parse(theReader.ReadLine());
									temp = (GameObject)Instantiate (goalPrefab, new Vector3 (xPos, yPos, zPos), Quaternion.identity);
									temp.transform.rotation = Quaternion.Euler(xRot, yRot, zRot);
									temp.transform.localScale = new Vector3(xScale, yScale, zScale);
								}
							}
						}
						while (line != null);
						theReader.Close();
					}
				}*/
			}
		}
	}
}