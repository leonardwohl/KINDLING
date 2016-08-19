using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class resize : MonoBehaviour {

	public GameObject heatPoint;
	public GameObject firePoint;

	public InputField inputField;

	public RawImage rawImageHold;
	public bool inGameMode = false;

	public GameObject holder;
	public List<GameObject> heatMapObjects = new List<GameObject>();
	//public List<GameObject> heatList = new List<GameObject>();

	public Text Score;
	public Text Done;

	public bool heatOn = false;

	public GameObject heatPrefab;
	public GameObject fireSpawnPrefab;
	public GameObject secondCamera;

	public int agentCounter = 0;
	public int goalCounter = 0; 

	public string fileName = "saveFile.txt";

	public GameObject wallPrefab;
	public GameObject agentPrefab;
	public GameObject doorPrefab;
	public GameObject goalPrefab;
	public GameObject extinguisherPrefab;
	public GameObject smokeAlarm;

	public GameObject voidPlane;
	public GameObject topLeftPivot;
	public GameObject bottomLeftPivot;
	public GameObject bottomRightPivot;
	public GameObject topRightPivot;
	public GameObject topPivot;
	public GameObject bottomPivot;
	public GameObject leftPivot;
	public GameObject rightPivot;

	private Vector3 mousePosition;
	private RaycastHit hit;
	private float timePassed;
	private RaycastHit selectedObject;
	private GameObject spawnedObject;

	public bool agentSelected = false;
	public bool selected = false;
	public bool spawningMode = false;

	public Material goalMaterial; 
	public Rect wR = new Rect(20,20,120,50);

	//1 is top left and increment clockwise
	public int sizingdirection = 0;

	public void DeleteObject(){
		selected = false; 
		agentSelected = false;

		if(selectedObject.transform != null){
		if(selectedObject.transform.gameObject.tag == "agent"){
			agentCounter--;
		}
		if(selectedObject.transform.gameObject.tag == "goal"){
			goalCounter--;
		}

		Destroy(selectedObject.transform.gameObject);
	}
	}

	public void Start() {
		Done.text = "";
		Score.text = "";
	}

	IEnumerator LoadRoutine() {
		gameObject.GetComponent<Pathfinding> ().levelEdited = true;
		gameObject.GetComponent<OnlinePLay>().levelFileName = inputField.text;
		File.WriteAllText ("localLevelName.txt", inputField.text);
		WWW myWWW = new WWW ("lenwohl.noip.me/" + inputField.text+".txt");
		yield return myWWW;
		if (myWWW.error == null) {
			string file = myWWW.text;
			file = file.Replace ('$', '\n');
			print (file);
			File.WriteAllText ("localOnlineFile.txt", file);
			if (File.Exists ("localOnlineFile.txt")) {
				string line;
				StreamReader theReader = new StreamReader ("localOnlineFile.txt", Encoding.Default);
				using (theReader) {
					// While there's lines left in the text file, do this:
					do {
						line = theReader.ReadLine ();
						if (line != null) {
							if (line == "13") {
								float xPos = float.Parse (theReader.ReadLine ());
								float yPos = float.Parse (theReader.ReadLine ());
								float zPos = float.Parse (theReader.ReadLine ());
								float xRot = float.Parse (theReader.ReadLine ());
								float yRot = float.Parse (theReader.ReadLine ());
								float zRot = float.Parse (theReader.ReadLine ());
								float xScale = float.Parse (theReader.ReadLine ());
								float yScale = float.Parse (theReader.ReadLine ());
								float zScale = float.Parse (theReader.ReadLine ());
								GameObject temp = (GameObject)Instantiate (wallPrefab, new Vector3 (xPos, yPos, zPos), Quaternion.identity);
								temp.transform.rotation = Quaternion.Euler (xRot, yRot, zRot);
								temp.transform.localScale = new Vector3 (xScale, yScale, zScale);
							} else if (line == "14") {
								float xPos = float.Parse (theReader.ReadLine ());
								float yPos = float.Parse (theReader.ReadLine ());
								float zPos = float.Parse (theReader.ReadLine ());
								float xRot = float.Parse (theReader.ReadLine ());
								float yRot = float.Parse (theReader.ReadLine ());
								float zRot = float.Parse (theReader.ReadLine ());
								float xScale = float.Parse (theReader.ReadLine ());
								float yScale = float.Parse (theReader.ReadLine ());
								float zScale = float.Parse (theReader.ReadLine ());
								GameObject temp = (GameObject)Instantiate (agentPrefab, new Vector3 (xPos, yPos, zPos), Quaternion.identity);
								temp.transform.rotation = Quaternion.Euler (xRot, yRot, zRot);
								temp.transform.localScale = new Vector3 (xScale, yScale, zScale);
								agentCounter++;
							} else if (line == "15") {
								float xPos = float.Parse (theReader.ReadLine ());
								float yPos = float.Parse (theReader.ReadLine ());
								float zPos = float.Parse (theReader.ReadLine ());
								float xRot = float.Parse (theReader.ReadLine ());
								float yRot = float.Parse (theReader.ReadLine ());
								float zRot = float.Parse (theReader.ReadLine ());
								float xScale = float.Parse (theReader.ReadLine ());
								float yScale = float.Parse (theReader.ReadLine ());
								float zScale = float.Parse (theReader.ReadLine ());
								GameObject temp = (GameObject)Instantiate (doorPrefab, new Vector3 (xPos, yPos, zPos), Quaternion.identity);
								temp.transform.rotation = Quaternion.Euler (xRot, yRot, zRot);
								temp.transform.localScale = new Vector3 (xScale, yScale, zScale);
							} else if (line == "16") {
								float xPos = float.Parse (theReader.ReadLine ());
								float yPos = float.Parse (theReader.ReadLine ());
								float zPos = float.Parse (theReader.ReadLine ());
								float xRot = float.Parse (theReader.ReadLine ());
								float yRot = float.Parse (theReader.ReadLine ());
								float zRot = float.Parse (theReader.ReadLine ());
								float xScale = float.Parse (theReader.ReadLine ());
								float yScale = float.Parse (theReader.ReadLine ());
								float zScale = float.Parse (theReader.ReadLine ());
								GameObject temp = (GameObject)Instantiate (goalPrefab, new Vector3 (xPos, yPos, zPos), Quaternion.identity);
								temp.transform.rotation = Quaternion.Euler (xRot, yRot, zRot);
								temp.transform.localScale = new Vector3 (xScale, yScale, zScale);
								goalCounter++;
							} else if (line == "17") {
								float xPos = float.Parse (theReader.ReadLine ());
								float yPos = float.Parse (theReader.ReadLine ());
								float zPos = float.Parse (theReader.ReadLine ());
								float xRot = float.Parse (theReader.ReadLine ());
								float yRot = float.Parse (theReader.ReadLine ());
								float zRot = float.Parse (theReader.ReadLine ());
								float xScale = float.Parse (theReader.ReadLine ());
								float yScale = float.Parse (theReader.ReadLine ());
								float zScale = float.Parse (theReader.ReadLine ());
								GameObject temp = (GameObject)Instantiate (extinguisherPrefab, new Vector3 (xPos, yPos, zPos), Quaternion.identity);
								temp.transform.rotation = Quaternion.Euler (xRot, yRot, zRot);
								temp.transform.localScale = new Vector3 (xScale, yScale, zScale);
							} else if (line == "18") {
								float xPos = float.Parse (theReader.ReadLine ());
								float yPos = float.Parse (theReader.ReadLine ());
								float zPos = float.Parse (theReader.ReadLine ());
								float xRot = float.Parse (theReader.ReadLine ());
								float yRot = float.Parse (theReader.ReadLine ());
								float zRot = float.Parse (theReader.ReadLine ());
								float xScale = float.Parse (theReader.ReadLine ());
								float yScale = float.Parse (theReader.ReadLine ());
								float zScale = float.Parse (theReader.ReadLine ());
								GameObject temp = (GameObject)Instantiate (smokeAlarm, new Vector3 (xPos, yPos, zPos), Quaternion.identity);
								temp.transform.rotation = Quaternion.Euler (xRot, yRot, zRot);
								temp.transform.localScale = new Vector3 (xScale, yScale, zScale);
							}
						}
					} while (line != null);
					theReader.Close ();
				}
			}
		}
	}

	public void Load() {
		foreach (GameObject temp in Object.FindObjectsOfType<GameObject>()) {
			if (temp.layer == 13 || temp.layer == 14 || temp.layer == 15 || temp.layer == 16) {
				Destroy (temp);
			}
		}
		StartCoroutine ("LoadRoutine");
		/*StreamReader nameReader = new StreamReader("localLevelName.txt", Encoding.Default);
		gameObject.GetComponent<OnlinePLay> ().levelFileName = nameReader.ReadToEnd ();
		foreach (GameObject temp in Object.FindObjectsOfType<GameObject>()) {
			if (temp.layer == 13 || temp.layer == 14 || temp.layer == 15 || temp.layer == 16) {
				Destroy (temp);
			}
		}
		if (File.Exists ("localOnlineFile.txt")) {
			string line;
			StreamReader tempReader = new StreamReader("localOnlineFile.txt", Encoding.Default);
			string tempString = tempReader.ReadToEnd ();
			tempString = tempString.Replace ('$', '\n');
			tempReader.Close();
			File.WriteAllText ("localOnlineFile.txt", tempString);
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
							GameObject temp = (GameObject)Instantiate (wallPrefab, new Vector3 (xPos, yPos, zPos), Quaternion.identity);
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
							GameObject temp = (GameObject)Instantiate (agentPrefab, new Vector3 (xPos, yPos, zPos), Quaternion.identity);
							temp.transform.rotation = Quaternion.Euler(xRot, yRot, zRot);
							temp.transform.localScale = new Vector3(xScale, yScale, zScale);
							agentCounter++;
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
							GameObject temp = (GameObject)Instantiate (doorPrefab, new Vector3 (xPos, yPos, zPos), Quaternion.identity);
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
							GameObject temp = (GameObject)Instantiate (goalPrefab, new Vector3 (xPos, yPos, zPos), Quaternion.identity);
							temp.transform.rotation = Quaternion.Euler(xRot, yRot, zRot);
							temp.transform.localScale = new Vector3(xScale, yScale, zScale);
							goalCounter++;
						}else if (line == "17") {
							float xPos = float.Parse(theReader.ReadLine());
							float yPos = float.Parse(theReader.ReadLine());
							float zPos = float.Parse(theReader.ReadLine());
							float xRot = float.Parse(theReader.ReadLine());
							float yRot = float.Parse(theReader.ReadLine());
							float zRot = float.Parse(theReader.ReadLine());
							float xScale = float.Parse(theReader.ReadLine());
							float yScale = float.Parse(theReader.ReadLine());
							float zScale = float.Parse(theReader.ReadLine());
							GameObject temp = (GameObject)Instantiate (extinguisherPrefab, new Vector3 (xPos, yPos, zPos), Quaternion.identity);
							temp.transform.rotation = Quaternion.Euler(xRot, yRot, zRot);
							temp.transform.localScale = new Vector3(xScale, yScale, zScale);
						}else if (line == "18") {
							float xPos = float.Parse(theReader.ReadLine());
							float yPos = float.Parse(theReader.ReadLine());
							float zPos = float.Parse(theReader.ReadLine());
							float xRot = float.Parse(theReader.ReadLine());
							float yRot = float.Parse(theReader.ReadLine());
							float zRot = float.Parse(theReader.ReadLine());
							float xScale = float.Parse(theReader.ReadLine());
							float yScale = float.Parse(theReader.ReadLine());
							float zScale = float.Parse(theReader.ReadLine());
							GameObject temp = (GameObject)Instantiate (smokeAlarm, new Vector3 (xPos, yPos, zPos), Quaternion.identity);
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

	private bool show = false;

	void OnGUI()
	{
		if (show)
		{
			GUI.Window(0, new Rect((Screen.width/2)-150, (Screen.height/2)-75
				, 300, 250), ShowGUI, "Warning");

		}
	}

	void ShowGUI(int windowID)
	{
		// You may put a label to show a message to the player

		GUI.Label(new Rect(80, 80, 200, 80), "You need to have at least 1 agent and 1 goal before you can save your level!");

		// You may put a button to close the pop up too

		if (GUI.Button(new Rect(50, 150, 75, 30), "OK"))
		{
			show = false;
			// you may put other code to run according to your game too
		}

	}

	public void Play() {
		if (goalCounter == 0 || agentCounter == 0) {
			//pop up can't save without one goal
			show = true;
			OnGUI();
		} else {
			show = false;
			inGameMode = true;
			GameObject.Find ("CameraTarget").SetActive (false);
			secondCamera.SetActive (true);
			topLeftPivot.SetActive (false);
			topRightPivot.SetActive (false);
			topPivot.SetActive (false);
			rightPivot.SetActive (false);
			bottomRightPivot.SetActive (false);
			bottomPivot.SetActive (false);
			bottomLeftPivot.SetActive (false);
			leftPivot.SetActive (false);
			voidPlane.SetActive (false);
			holder.SetActive (false);
			//if (File.Exists(fileName)) {
			//	StreamReader theReader = new StreamReader(fileName, Encoding.Default);
			//	print("exists");
			//} else {
			//	StreamWriter sw = File.CreateText (fileName);
			//}
			GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject> ();
			foreach (GameObject go in allObjects) {
				if (go.layer == 5 && go.name == "Canvas") {
					go.SetActive (false);
				} else if (go.tag != "MainCamera" && go.tag != "noSave" && go.tag != "void") {
					if (go.layer == 13) {
						go.transform.GetChild (0).GetComponent<BoxCollider> ().enabled = true;
						go.transform.GetChild (1).GetComponent<BoxCollider> ().enabled = true;
						go.GetComponent<BoxCollider> ().enabled = false;
					}
					if (go.layer == 15) {
						go.transform.GetChild (0).GetComponent<BoxCollider> ().enabled = true;
						go.transform.GetChild (1).GetComponent<BoxCollider> ().enabled = true;
						go.GetComponent<BoxCollider> ().enabled = false;
					}
					if (go.layer == 14) {
						go.GetComponent<Unit> ().enabled = true;
						go.GetComponent<FieldOfView> ().enabled = true;
						go.GetComponent<Destroyer> ().enabled = true;
						go.GetComponent<NavMeshAgent> ().enabled = true;
					}
					if (go.layer == 16) { //goal
						gameObject.GetComponent<Grid> ().targetExits.Add (go.transform);
					}
					if (go.layer == 17) { //extinguisher

					}
					if (go.layer == 18) { //smokeAlarm

					}
				}
			}
			gameObject.GetComponent<GameOver> ().enabled = true;
			gameObject.GetComponent<Grid> ().enabled = true;
			gameObject.GetComponent<Grid> ().CreateGrid ();
			gameObject.GetComponent<resize> ().enabled = false;
		}
	}

	IEnumerator SaveLevel() {
		File.WriteAllText ("localLevelName.txt", inputField.text);
		StreamReader theReader = new StreamReader("saveFile.txt", Encoding.Default);
		string tempRead = theReader.ReadToEnd ();
		WWW survey_info = new WWW ("lenwohl.noip.me/weblog.php?name=" + inputField.text + "&text=" + tempRead);
		gameObject.GetComponent<OnlinePLay> ().levelFileName = inputField.text;
		// Wait for the request to send
		yield return survey_info;

		if (survey_info.error != null) {
			Debug.Log (survey_info.error); // You should probably check your page for errors
			// Optionally print survey_info.text to get a corresponding webpage with error messages
		}
	}

	public void Save() {
		gameObject.GetComponent<Pathfinding> ().levelEdited = true;
		if (goalCounter == 0 || agentCounter == 0) {
			//pop up can't save without one goal 
			show = true;
			OnGUI ();
			/*} else {
			show = false;
			//if (File.Exists(fileName)) {
			//	StreamReader theReader = new StreamReader(fileName, Encoding.Default);
			//	print("exists");
			//} else {
			//	StreamWriter sw = File.CreateText (fileName);
			//}
			StreamWriter sw = File.CreateText (fileName);
			GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject> ();
			foreach (GameObject go in allObjects) {
				if (go.tag != "MainCamera" && go.tag != "noSave" && go.tag != "void") {
					sw.WriteLine (go.layer);
					sw.WriteLine (go.transform.position.x);
					sw.WriteLine (go.transform.position.y);
					sw.WriteLine (go.transform.position.z);
					sw.WriteLine (go.transform.rotation.eulerAngles.x);
					sw.WriteLine (go.transform.rotation.eulerAngles.y);
					sw.WriteLine (go.transform.rotation.eulerAngles.z);
					sw.WriteLine (go.transform.localScale.x);
					sw.WriteLine (go.transform.localScale.y);
					sw.WriteLine (go.transform.localScale.z);
				}
			}
			sw.Close ();
		}*/
		} else {
			show = false;
			//if (File.Exists(fileName)) {
			//	StreamReader theReader = new StreamReader(fileName, Encoding.Default);
			//	print("exists");
			//} else {
			//	StreamWriter sw = File.CreateText (fileName);
			//}
			StreamWriter sw = File.CreateText (fileName);
			GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject> ();
			foreach (GameObject go in allObjects) {
				if (go.tag != "MainCamera" && go.tag != "noSave" && go.tag != "void") {
					sw.Write (go.layer + "$");
					sw.Write (go.transform.position.x + "$");
					sw.Write (go.transform.position.y + "$");
					sw.Write (go.transform.position.z + "$");
					sw.Write (go.transform.rotation.eulerAngles.x + "$");
					sw.Write (go.transform.rotation.eulerAngles.y + "$");
					sw.Write (go.transform.rotation.eulerAngles.z + "$");
					sw.Write (go.transform.localScale.x + "$");
					sw.Write (go.transform.localScale.y + "$");
					sw.Write (go.transform.localScale.z + "$");
				}
			}
			sw.Close ();
			StartCoroutine ("SaveLevel");
		}
	}

	public void SmokeAlarmClick(){
		Ray tempRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit tempHit;
		Physics.Raycast (tempRay, out tempHit);
		spawnedObject = (GameObject)Instantiate (smokeAlarm, new Vector3 (tempHit.point.x, 1f, tempHit.point.z), Quaternion.identity);
		spawningMode = true;
	}

	public void ExtinguisherClick(){
		Ray tempRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit tempHit;
		Physics.Raycast (tempRay, out tempHit);
		spawnedObject = (GameObject)Instantiate (extinguisherPrefab, new Vector3 (tempHit.point.x, 1f, tempHit.point.z), Quaternion.identity);
		spawningMode = true;
	}

	public void WallClick() {
		Ray tempRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit tempHit;
		Physics.Raycast (tempRay, out tempHit);
		spawnedObject = (GameObject)Instantiate (wallPrefab, new Vector3 (tempHit.point.x, 0.5f, tempHit.point.z), Quaternion.identity);
		spawningMode = true;
	}

	public void AgentClick() {
		Ray tempRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit tempHit;
		Physics.Raycast (tempRay, out tempHit);
		spawnedObject = (GameObject)Instantiate (agentPrefab, new Vector3 (tempHit.point.x, 1f, tempHit.point.z), Quaternion.identity);
		spawnedObject.GetComponent<Renderer> ().material.color = Color.green;
		spawningMode = true;
		agentCounter++;
	}

	public void DoorClick() {
		Ray tempRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit tempHit;
		Physics.Raycast (tempRay, out tempHit);
		spawnedObject = (GameObject)Instantiate (doorPrefab, new Vector3 (tempHit.point.x, 0.5f, tempHit.point.z), Quaternion.identity);
		spawningMode = true;
	}

	public void GoalClick() {
		Ray tempRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit tempHit;
		Physics.Raycast (tempRay, out tempHit);
		spawnedObject = (GameObject)Instantiate (goalPrefab, new Vector3 (tempHit.point.x, 0.5f, tempHit.point.z), Quaternion.identity);
		goalCounter++;
		spawningMode = true;
	}

	IEnumerator HeatClickRoutine() {
		if (!heatOn) {
			heatMapObjects.Clear ();
			string line;
			print (gameObject.GetComponent<OnlinePLay> ().levelFileName);
			WWW myWWW = new WWW ("lenwohl.noip.me/" + gameObject.GetComponent<OnlinePLay> ().levelFileName + "HEAT.txt");
			yield return myWWW;
			print (myWWW.error);
			string heatString = myWWW.text;
			print (heatString);
			heatString = heatString.Replace ('$', '\n');
			File.WriteAllText ("LoadableHeat.txt", heatString);
			StreamReader spawningHeat = new StreamReader ("LoadableHeat.txt", Encoding.Default);
			using (spawningHeat) {
				// While there's lines left in the text file, do this:
				line = spawningHeat.ReadLine ();
				for (int i = 0; i < int.Parse (line); i++) {
					heatMapObjects.Add ((GameObject)Instantiate (heatPoint, new Vector3 (float.Parse (spawningHeat.ReadLine ()), float.Parse (spawningHeat.ReadLine ()), float.Parse (spawningHeat.ReadLine ())), Quaternion.identity));
				}
				heatMapObjects.Add ((GameObject)Instantiate (firePoint, new Vector3 (float.Parse (spawningHeat.ReadLine ()), float.Parse (spawningHeat.ReadLine ()), float.Parse (spawningHeat.ReadLine ())), Quaternion.identity));

				string testFlame = spawningHeat.ReadLine ();
				if (testFlame != null) {
					heatMapObjects.Add ((GameObject)Instantiate (firePoint, new Vector3 (float.Parse(testFlame), float.Parse (spawningHeat.ReadLine ()), float.Parse (spawningHeat.ReadLine ())), Quaternion.identity));
				}
				spawningHeat.Close ();
			}
			heatOn = true;
		} else {
			foreach (GameObject dead in heatMapObjects) {
				Destroy (dead);
			}
			heatOn = false;
		}
	}

	public void HeatClick() {
		StartCoroutine ("HeatClickRoutine");
		//Remove /* to make show png work
		/*if (!heatOn && System.IO.File.Exists(Application.dataPath + "screenshot.png")) {
			rawImageHold.enabled = true;
			heatOn = true;
			string url = Application.dataPath + "screenshot.png";
			var bytes = File.ReadAllBytes( url );
			Texture2D texture = new Texture2D(690,400);
			texture.LoadImage( bytes );
			rawImageHold.texture = texture;
			/*if (GameObject.Find ("ShowedHeatMap(Clone)") != null && GameObject.Find ("ShowedHeatMap(Clone)").GetComponent<HeatMapData> ().heatList.Count > 0) {
				foreach (Vector3 temp in GameObject.Find("ShowedHeatMap(Clone)").GetComponent<HeatMapData>().heatList) {
					//temp.SetActive (!temp.activeInHierarchy);
					heatMapObjects.Add((GameObject)Instantiate (heatPrefab, temp, Quaternion.identity));
				}
				if (GameObject.Find ("ShowedHeatMap(Clone)").GetComponent<HeatMapData> ().fireSpawn.Count > 0) {
					foreach (Vector3 temp in GameObject.Find("ShowedHeatMap(Clone)").GetComponent<HeatMapData>().fireSpawn) {
						heatMapObjects.Add((GameObject)Instantiate (fireSpawnPrefab, temp, Quaternion.identity));
					}
				}
			}*/
		//Remove /* to make png work
		/*	heatOn = true;
		} else {
			/*foreach (GameObject temp in heatMapObjects) {
				Destroy (temp);
			}*/
		//Remove /* to make png work
		/*	rawImageHold.enabled = false;
			heatOn = false;
		}
		*/
	}



	void Update () {
		if (!spawningMode) {
			if (!selected || agentSelected) {
				topLeftPivot.SetActive (false);
				topRightPivot.SetActive (false);
				topPivot.SetActive (false);
				rightPivot.SetActive (false);
				bottomRightPivot.SetActive (false);
				bottomPivot.SetActive (false);
				bottomLeftPivot.SetActive (false);
				leftPivot.SetActive (false);
			} else {
				topLeftPivot.SetActive (true);
				topRightPivot.SetActive (true);
				topPivot.SetActive (true);
				rightPivot.SetActive (true);
				bottomRightPivot.SetActive (true);
				bottomPivot.SetActive (true);
				bottomLeftPivot.SetActive (true);
				leftPivot.SetActive (true);
			}
			if (selected && !agentSelected) {
				topLeftPivot.transform.position = new Vector3 (selectedObject.transform.gameObject.transform.position.x - selectedObject.transform.gameObject.GetComponent<Renderer> ().bounds.size.x / 2, selectedObject.transform.gameObject.transform.position.y, selectedObject.transform.gameObject.transform.position.z + selectedObject.transform.gameObject.GetComponent<Renderer> ().bounds.size.z / 2);
				topRightPivot.transform.position = new Vector3 (selectedObject.transform.gameObject.transform.position.x + selectedObject.transform.gameObject.GetComponent<Renderer> ().bounds.size.x / 2, selectedObject.transform.gameObject.transform.position.y, selectedObject.transform.gameObject.transform.position.z + selectedObject.transform.gameObject.GetComponent<Renderer> ().bounds.size.z / 2);
				bottomLeftPivot.transform.position = new Vector3 (selectedObject.transform.gameObject.transform.position.x - selectedObject.transform.gameObject.GetComponent<Renderer> ().bounds.size.x / 2, selectedObject.transform.gameObject.transform.position.y, selectedObject.transform.gameObject.transform.position.z - selectedObject.transform.gameObject.GetComponent<Renderer> ().bounds.size.z / 2);
				bottomRightPivot.transform.position = new Vector3 (selectedObject.transform.gameObject.transform.position.x + selectedObject.transform.gameObject.GetComponent<Renderer> ().bounds.size.x / 2, selectedObject.transform.gameObject.transform.position.y, selectedObject.transform.gameObject.transform.position.z - selectedObject.transform.gameObject.GetComponent<Renderer> ().bounds.size.z / 2);
				topPivot.transform.position = new Vector3 (selectedObject.transform.gameObject.transform.position.x, selectedObject.transform.gameObject.transform.position.y, selectedObject.transform.gameObject.transform.position.z + selectedObject.transform.gameObject.GetComponent<Renderer> ().bounds.size.z / 2);
				bottomPivot.transform.position = new Vector3 (selectedObject.transform.gameObject.transform.position.x, selectedObject.transform.gameObject.transform.position.y, selectedObject.transform.gameObject.transform.position.z - selectedObject.transform.gameObject.GetComponent<Renderer> ().bounds.size.z / 2);
				leftPivot.transform.position = new Vector3 (selectedObject.transform.gameObject.transform.position.x - selectedObject.transform.gameObject.GetComponent<Renderer> ().bounds.size.x / 2, selectedObject.transform.gameObject.transform.position.y, selectedObject.transform.gameObject.transform.position.z);
				rightPivot.transform.position = new Vector3 (selectedObject.transform.gameObject.transform.position.x + selectedObject.transform.gameObject.GetComponent<Renderer> ().bounds.size.x / 2, selectedObject.transform.gameObject.transform.position.y, selectedObject.transform.gameObject.transform.position.z);
			}
			if (selected && Input.GetKeyDown (KeyCode.R)) {
				selectedObject.transform.gameObject.transform.rotation = Quaternion.Euler (0, selectedObject.transform.gameObject.transform.rotation.eulerAngles.y + 90, 0);
			}

			if ((selected && Input.GetMouseButtonDown(1)) || (agentSelected && Input.GetMouseButtonDown(1))) {
				selected = false;
				agentSelected = false;
				Destroy (selectedObject.transform.gameObject);
			}
			if (Input.GetMouseButtonDown (0)) {
				//if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit)) {
				if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit)) {
					if (hit.transform.gameObject.tag == "extinguisher") {
						if(agentSelected){
							if(selectedObject.transform.gameObject.tag == "extinguisher"){
								selectedObject.transform.GetComponent<Renderer>().material.color = Color.red;
							}else if(selectedObject.transform.gameObject.tag == "goal"){
								selectedObject.transform.GetComponent<Renderer>().material = goalMaterial;
							}else if(selectedObject.transform.gameObject.tag == "agent"){
								selectedObject.transform.GetComponent<Renderer>().material.color = Color.green;
							}
						}

						agentSelected = true;
						hit.transform.GetComponent<Renderer> ().material.color = Color.blue;
						if (selectedObject.transform != null && selectedObject.transform.gameObject == hit.transform.gameObject) {
							sizingdirection = 9;
						} else {
							sizingdirection = 0;
							selected = true;
							if (selectedObject.transform != null && selectedObject.transform.gameObject.tag == "extinguisher") {
								selectedObject.transform.GetComponent<Renderer> ().material.color = Color.red;
							}

							selectedObject = hit;
						}
					}else if (hit.transform.gameObject.tag == "agent") {
						if(agentSelected){
							if(selectedObject.transform.gameObject.tag == "extinguisher"){
								selectedObject.transform.GetComponent<Renderer>().material.color = Color.red;
							}else if(selectedObject.transform.gameObject.tag == "goal"){
								selectedObject.transform.GetComponent<Renderer>().material = goalMaterial;
							}else if(selectedObject.transform.gameObject.tag == "agent"){
								selectedObject.transform.GetComponent<Renderer>().material.color = Color.green;
							}

						}
						agentSelected = true;
						hit.transform.GetComponent<Renderer> ().material.color = Color.red;
						if (selectedObject.transform != null && selectedObject.transform.gameObject == hit.transform.gameObject) {
							sizingdirection = 9;
						} else {
							sizingdirection = 0;
							selected = true;
							if (selectedObject.transform != null && selectedObject.transform.gameObject.tag == "agent") {
								selectedObject.transform.GetComponent<Renderer> ().material.color = Color.green;
							}
							selectedObject = hit;
						}
					}else if (hit.transform.gameObject.tag == "goal") {
						if(agentSelected){
							if(selectedObject.transform.gameObject.tag == "extinguisher"){
								selectedObject.transform.GetComponent<Renderer>().material.color = Color.red;
							}else if(selectedObject.transform.gameObject.tag == "goal"){
								selectedObject.transform.GetComponent<Renderer>().material = goalMaterial;
							}else if(selectedObject.transform.gameObject.tag == "agent"){
								selectedObject.transform.GetComponent<Renderer>().material.color = Color.green;
							}

						}

						agentSelected = true;
						hit.transform.GetComponent<Renderer> ().material.color = Color.yellow;
						if (selectedObject.transform != null && selectedObject.transform.gameObject == hit.transform.gameObject) {
							sizingdirection = 9;
						} else {
							sizingdirection = 0;
							selected = true;
							if (selectedObject.transform != null && selectedObject.transform.gameObject.tag == "goal") {
								selectedObject.transform.GetComponent<Renderer> ().material = goalMaterial;
							}
							selectedObject = hit;
						}
					} else if (hit.transform.gameObject.tag != "void") {
						agentSelected = false;
						if (selectedObject.transform != null && selectedObject.transform.gameObject.tag == "agent") {
							selectedObject.transform.GetComponent<Renderer> ().material.color = Color.green;
						}
						if (selectedObject.transform != null && selectedObject.transform.gameObject.tag == "extinguisher") {
							selectedObject.transform.GetComponent<Renderer> ().material.color = Color.red;
						}
						if (selectedObject.transform != null && selectedObject.transform.gameObject.tag == "goal") {
							selectedObject.transform.GetComponent<Renderer> ().material = goalMaterial;
						}
						if (hit.transform.gameObject == topLeftPivot) {
							sizingdirection = 1;
						} else if (hit.transform.gameObject == topPivot) {
							sizingdirection = 2;
						} else if (hit.transform.gameObject == topRightPivot) {
							sizingdirection = 3;
						} else if (hit.transform.gameObject == rightPivot) {
							sizingdirection = 4;
						} else if (hit.transform.gameObject == bottomRightPivot) {
							sizingdirection = 5;
						} else if (hit.transform.gameObject == bottomPivot) {
							sizingdirection = 6;
						} else if (hit.transform.gameObject == bottomLeftPivot) {
							sizingdirection = 7;
						} else if (hit.transform.gameObject == leftPivot) {
							sizingdirection = 8;
						} else if (!selected) {
							sizingdirection = 0;
							selected = true;
							selectedObject = hit;
						} else if (selected && selectedObject.transform.gameObject == hit.transform.gameObject) {
							sizingdirection = 9;
						} else {
							sizingdirection = 0;
							selected = true;
							selectedObject = hit;
						}
					} else {
						if (selectedObject.transform != null && selectedObject.transform.gameObject.tag == "agent") {
							selectedObject.transform.GetComponent<Renderer> ().material.color = Color.green;
						}
						if (selectedObject.transform != null && selectedObject.transform.gameObject.tag == "extinguisher") {
							selectedObject.transform.GetComponent<Renderer> ().material.color = Color.red;
						}
						if (selectedObject.transform != null && selectedObject.transform.gameObject.tag == "goal") {
							selectedObject.transform.GetComponent<Renderer> ().material = goalMaterial;
						}
						agentSelected = false;
						sizingdirection = 0;
						selected = false;
					}
				}
			}
			if (Input.GetMouseButtonUp (0)) {
				if (selected) {
					selectedObject.transform.gameObject.transform.parent = null;
				}
				holder.transform.localScale = Vector3.one;
			}
			if (Input.GetMouseButton (0)) {
				if (sizingdirection == 1) {
					holder.transform.position = bottomRightPivot.transform.position;
					selectedObject.transform.gameObject.transform.parent = holder.transform;
					if (holder.transform.localScale.x > 0.04f) {
						if (Input.GetAxis ("Mouse X") > 0) {
							holder.transform.localScale = new Vector3 (holder.transform.localScale.x - 0.02f, holder.transform.localScale.y, holder.transform.localScale.z);
						}
					}
					if (Input.GetAxis ("Mouse X") < 0) {
						holder.transform.localScale = new Vector3 (holder.transform.localScale.x + 0.02f, holder.transform.localScale.y, holder.transform.localScale.z);
					}
					if (Input.GetAxis ("Mouse Y") > 0) {
						holder.transform.localScale = new Vector3 (holder.transform.localScale.x, holder.transform.localScale.y, holder.transform.localScale.z + 0.02f);
					}
					if (holder.transform.localScale.z > 0.04f) {
						if (Input.GetAxis ("Mouse Y") < 0) {
							holder.transform.localScale = new Vector3 (holder.transform.localScale.x, holder.transform.localScale.y, holder.transform.localScale.z - 0.02f);
						}
					}
				} else if (sizingdirection == 9) {
					Ray tempRay = Camera.main.ScreenPointToRay (Input.mousePosition);
					RaycastHit tempHit;
					Physics.Raycast (tempRay, out tempHit);
					selectedObject.transform.position = new Vector3 (tempHit.point.x, selectedObject.transform.position.y, tempHit.point.z);
				} else if (sizingdirection == 2) {
					holder.transform.position = bottomPivot.transform.position;
					selectedObject.transform.gameObject.transform.parent = holder.transform;
					if (Input.GetAxis ("Mouse Y") > 0) {
						holder.transform.localScale = new Vector3 (holder.transform.localScale.x, holder.transform.localScale.y, holder.transform.localScale.z + 0.02f);
					}
					if (holder.transform.localScale.z > 0.04f) {
						if (Input.GetAxis ("Mouse Y") < 0) {
							holder.transform.localScale = new Vector3 (holder.transform.localScale.x, holder.transform.localScale.y, holder.transform.localScale.z - 0.02f);
						}
					}
				} else if (sizingdirection == 3) {
					holder.transform.position = bottomLeftPivot.transform.position;
					selectedObject.transform.gameObject.transform.parent = holder.transform;
					if (Input.GetAxis ("Mouse X") > 0) {
						holder.transform.localScale = new Vector3 (holder.transform.localScale.x + 0.02f, holder.transform.localScale.y, holder.transform.localScale.z);
					}
					if (holder.transform.localScale.x > 0.04f) {
						if (Input.GetAxis ("Mouse X") < 0) {
							holder.transform.localScale = new Vector3 (holder.transform.localScale.x - 0.02f, holder.transform.localScale.y, holder.transform.localScale.z);
						}
					}
					if (Input.GetAxis ("Mouse Y") > 0) {
						holder.transform.localScale = new Vector3 (holder.transform.localScale.x, holder.transform.localScale.y, holder.transform.localScale.z + 0.02f);
					}
					if (holder.transform.localScale.z > 0.04f) {
						if (Input.GetAxis ("Mouse Y") < 0) {
							holder.transform.localScale = new Vector3 (holder.transform.localScale.x, holder.transform.localScale.y, holder.transform.localScale.z - 0.02f);
						}
					}
				} else if (sizingdirection == 4) {
					holder.transform.position = leftPivot.transform.position;
					selectedObject.transform.gameObject.transform.parent = holder.transform;
					if (Input.GetAxis ("Mouse X") > 0) {
						holder.transform.localScale = new Vector3 (holder.transform.localScale.x + 0.02f, holder.transform.localScale.y, holder.transform.localScale.z);
					}
					if (holder.transform.localScale.x > 0.04f) {
						if (Input.GetAxis ("Mouse X") < 0) {
							holder.transform.localScale = new Vector3 (holder.transform.localScale.x - 0.02f, holder.transform.localScale.y, holder.transform.localScale.z);
						}
					}
				} else if (sizingdirection == 5) {
					holder.transform.position = topLeftPivot.transform.position;
					selectedObject.transform.gameObject.transform.parent = holder.transform;
					if (Input.GetAxis ("Mouse X") > 0) {
						holder.transform.localScale = new Vector3 (holder.transform.localScale.x + 0.02f, holder.transform.localScale.y, holder.transform.localScale.z);
					}
					if (holder.transform.localScale.x > 0.04f) {
						if (Input.GetAxis ("Mouse X") < 0) {
							holder.transform.localScale = new Vector3 (holder.transform.localScale.x - 0.02f, holder.transform.localScale.y, holder.transform.localScale.z);
						}
					}
					if (holder.transform.localScale.z > 0.04f) {
						if (Input.GetAxis ("Mouse Y") > 0) {
							holder.transform.localScale = new Vector3 (holder.transform.localScale.x, holder.transform.localScale.y, holder.transform.localScale.z - 0.02f);
						}
					}
					if (Input.GetAxis ("Mouse Y") < 0) {
						holder.transform.localScale = new Vector3 (holder.transform.localScale.x, holder.transform.localScale.y, holder.transform.localScale.z + 0.02f);
					}
				} else if (sizingdirection == 6) {
					holder.transform.position = topPivot.transform.position;
					selectedObject.transform.gameObject.transform.parent = holder.transform;
					if (holder.transform.localScale.z > 0.04f) {
						if (Input.GetAxis ("Mouse Y") > 0) {
							holder.transform.localScale = new Vector3 (holder.transform.localScale.x, holder.transform.localScale.y, holder.transform.localScale.z - 0.02f);
						}
					}
					if (Input.GetAxis ("Mouse Y") < 0) {
						holder.transform.localScale = new Vector3 (holder.transform.localScale.x, holder.transform.localScale.y, holder.transform.localScale.z + 0.02f);
					}
				} else if (sizingdirection == 7) {
					holder.transform.position = topRightPivot.transform.position;
					selectedObject.transform.gameObject.transform.parent = holder.transform;
					if (holder.transform.localScale.x > 0.04f) {
						if (Input.GetAxis ("Mouse X") > 0) {
							holder.transform.localScale = new Vector3 (holder.transform.localScale.x - 0.02f, holder.transform.localScale.y, holder.transform.localScale.z);
						}
					}
					if (Input.GetAxis ("Mouse X") < 0) {
						holder.transform.localScale = new Vector3 (holder.transform.localScale.x + 0.02f, holder.transform.localScale.y, holder.transform.localScale.z);
					}
					if (holder.transform.localScale.z > 0.04f) {
						if (Input.GetAxis ("Mouse Y") > 0) {
							holder.transform.localScale = new Vector3 (holder.transform.localScale.x, holder.transform.localScale.y, holder.transform.localScale.z - 0.02f);
						}
					}
					if (Input.GetAxis ("Mouse Y") < 0) {
						holder.transform.localScale = new Vector3 (holder.transform.localScale.x, holder.transform.localScale.y, holder.transform.localScale.z + 0.02f);
					}
				} else if (sizingdirection == 8) {
					holder.transform.position = rightPivot.transform.position;
					selectedObject.transform.gameObject.transform.parent = holder.transform;
					if (holder.transform.localScale.x > 0.04f) {
						if (Input.GetAxis ("Mouse X") > 0) {
							holder.transform.localScale = new Vector3 (holder.transform.localScale.x - 0.02f, holder.transform.localScale.y, holder.transform.localScale.z);
						}
					}
					if (Input.GetAxis ("Mouse X") < 0) {
						holder.transform.localScale = new Vector3 (holder.transform.localScale.x + 0.02f, holder.transform.localScale.y, holder.transform.localScale.z);
					}
				}
			}
		} else {
			if (Input.GetMouseButtonDown (0)) {
				spawningMode = false;
			} else {
				if (Input.GetKeyDown (KeyCode.R)) {
					spawnedObject.transform.gameObject.transform.rotation = Quaternion.Euler (0, spawnedObject.transform.gameObject.transform.rotation.eulerAngles.y + 90, 0);
				}
				Ray tempRay = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit tempHit;
				Physics.Raycast (tempRay, out tempHit);
				spawnedObject.transform.position = new Vector3 (tempHit.point.x, spawnedObject.transform.position.y, tempHit.point.z);
			}
		}
	}
	public void returnToMainMenu(){
		Destroy(GameObject.Find("A*"));
		SceneManager.LoadScene (0);
	}
}