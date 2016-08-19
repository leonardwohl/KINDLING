using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System.Text;

public class OnlinePLay : MonoBehaviour {
	public GameObject wallPrefab;
	public GameObject agentPrefab;
	public GameObject doorPrefab;
	public GameObject goalPrefab;
	public GameObject extinguisherPrefab;
	public GameObject smokeAlarm;
	public InputField inputField;
	public GameObject secondCamera;
	public GameObject firstCamera;

	public string levelFileName;

	public Text Score;
	public Text Done;

	public bool onePress = false;

	public int agentCounter = 0;

	// Use this for initialization
	public void Start() {
		Done.text = "";
		Score.text = "";
		Done.enabled = false;
		Score.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Play() {
		secondCamera.SetActive (true);
		GameObject.Find ("FakePlane").SetActive (false);
		firstCamera.SetActive (false);
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
	}

	IEnumerator LoadLevelRoutine() {
		gameObject.GetComponent<Pathfinding> ().levelEdited = true;
		levelFileName = inputField.text;
		File.WriteAllText ("localLevelName.txt", levelFileName);
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
			Play ();
		}
	}

	public void LoadLevel() {
		if (!onePress) {
			onePress = true;
			StartCoroutine ("LoadLevelRoutine");
		}
	}

}
