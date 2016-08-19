using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;

public class SpawnCopy : MonoBehaviour {

	public string fileName = "saveFile.txt";

	public bool onePress;

	public string heatFileName = "saveFileHeat.txt";
	public GameObject levelCopy;
	public GameObject doorPrefab;
	public GameObject goalPrefab;
	public GameObject wallPrefab;
	public GameObject planePrefab;
	public GameObject shown;
	public GameObject heatHolder;

	// Use this for initialization
	void Awake () {
		
		if (GameObject.Find("A*").GetComponent<GameOver>().previousScene != 2 && GameObject.Find("A*").GetComponent<GameOver>().previousScene != 4) {
			Instantiate (levelCopy, Vector3.zero, Quaternion.identity);
			heatHolder = (GameObject)Instantiate (shown, Vector3.zero, Quaternion.identity);
			DontDestroyOnLoad (heatHolder);
		} else {
			heatHolder = (GameObject)Instantiate (shown, Vector3.zero, Quaternion.identity);
			DontDestroyOnLoad (heatHolder);
			heatHolder.GetComponent<HeatMapData> ().levelEdited = GameObject.Find ("A*").GetComponent<Pathfinding> ().levelEdited;
			Instantiate (planePrefab, Vector3.zero, Quaternion.identity);
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
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator returnToLevelRoutine() {
		StreamReader nameReader = new StreamReader("localLevelName.txt", Encoding.Default);
		string levelFileName = nameReader.ReadToEnd();
		int previousScene = GameObject.Find ("A*").GetComponent<GameOver> ().previousScene;
		WWW myWWW = new WWW ("lenwohl.noip.me/" + levelFileName +"SCORE.txt");
		yield return myWWW;
		print (myWWW.text);
		if (myWWW.error == null && !heatHolder.GetComponent<HeatMapData>().levelEdited) {
			string line;
			File.WriteAllText (heatFileName, myWWW.text);
			StreamReader theReader = new StreamReader (heatFileName, Encoding.Default);
			using (theReader) {
				// While there's lines left in the text file, do this:
				line = theReader.ReadLine ();
				print (line);
				if (line != null) {
					if (int.Parse(line) < GameObject.Find("A*").GetComponent<GameOver>().count) {
						theReader.Close ();
						StreamWriter sw = File.CreateText (heatFileName);
						sw.WriteLine(GameObject.Find("A*").GetComponent<GameOver>().count);
						WWW survey_info = new WWW ("lenwohl.noip.me/weblog.php?name=" + levelFileName + "SCORE" + "&text=" + GameObject.Find("A*").GetComponent<GameOver>().count);
						yield return survey_info;
						sw.Close();
						Application.CaptureScreenshot("Assetsscreenshot.png");
						StreamReader heatReader = new StreamReader("LocalHeatMap.txt", Encoding.Default);
						string heatStorage = heatReader.ReadToEnd ();
						heatReader.Close ();
						WWW heat_info = new WWW ("lenwohl.noip.me/weblog.php?name=" + levelFileName + "HEAT" + "&text=" + heatStorage);
						yield return heat_info;
						print (heat_info.error);
						yield return 0;
					}
				}
				else {
					theReader.Close ();
				}
			}
		} else {
			if (myWWW.text[0] == '<') {
				StreamWriter sw = File.CreateText (heatFileName);
				sw.WriteLine (GameObject.Find ("A*").GetComponent<GameOver> ().count);
				WWW survey_info = new WWW ("lenwohl.noip.me/weblog.php?name=" + levelFileName + "SCORE" + "&text=" + GameObject.Find ("A*").GetComponent<GameOver> ().count);
				yield return survey_info;
				sw.Close ();
				Application.CaptureScreenshot ("Assetsscreenshot.png");
				StreamReader heatReader = new StreamReader ("LocalHeatMap.txt", Encoding.Default);
				string heatStorage = heatReader.ReadToEnd ();
				heatReader.Close ();
				WWW heat_info = new WWW ("lenwohl.noip.me/weblog.php?name=" + levelFileName + "HEAT" + "&text=" + heatStorage);
				yield return heat_info;
				yield return 0;
			} else {
				if (int.Parse(myWWW.text) < GameObject.Find("A*").GetComponent<GameOver>().count) {
					StreamWriter sw = File.CreateText (heatFileName);
					sw.WriteLine(GameObject.Find("A*").GetComponent<GameOver>().count);
					WWW survey_info = new WWW ("lenwohl.noip.me/weblog.php?name=" + levelFileName + "SCORE" + "&text=" + GameObject.Find("A*").GetComponent<GameOver>().count);
					yield return survey_info;
					sw.Close();
					Application.CaptureScreenshot("Assetsscreenshot.png");
					print (levelFileName);
					StreamReader heatReader = new StreamReader("LocalHeatMap.txt", Encoding.Default);
					string heatStorage = heatReader.ReadToEnd ();
					heatReader.Close ();
					WWW heat_info = new WWW ("lenwohl.noip.me/weblog.php?name=" + levelFileName + "HEAT" + "&text=" + heatStorage);
					yield return heat_info;
					print (heat_info.error);
					yield return 0;
				}
			}
		}
		Destroy(GameObject.Find("A*"));
		SceneManager.LoadScene (previousScene);
	}

	public void returnToLevel() {
		if (!onePress) {
			onePress = true;
			StartCoroutine ("returnToLevelRoutine");
		}
	}

	IEnumerator returnToMainMenuRoutine() {
		StreamReader nameReader = new StreamReader("localLevelName.txt", Encoding.Default);
		string levelFileName = nameReader.ReadToEnd();
		print (levelFileName);
		WWW myWWW = new WWW ("lenwohl.noip.me/" + levelFileName +"SCORE.txt");
		yield return myWWW;
		print (myWWW.text);
		if (myWWW.error == null && !heatHolder.GetComponent<HeatMapData>().levelEdited) {
			string line;
			File.WriteAllText (heatFileName, myWWW.text);
			StreamReader theReader = new StreamReader (heatFileName, Encoding.Default);
			using (theReader) {
				// While there's lines left in the text file, do this:
				line = theReader.ReadLine ();
				print (line);
				if (line != null) {
					if (int.Parse(line) < GameObject.Find("A*").GetComponent<GameOver>().count) {
						theReader.Close ();
						StreamWriter sw = File.CreateText (heatFileName);
						sw.WriteLine(GameObject.Find("A*").GetComponent<GameOver>().count);
						WWW survey_info = new WWW ("lenwohl.noip.me/weblog.php?name=" + levelFileName + "SCORE" + "&text=" + GameObject.Find("A*").GetComponent<GameOver>().count);
						yield return survey_info;
						sw.Close();
						Application.CaptureScreenshot("Assetsscreenshot.png");
						print (levelFileName);
						StreamReader heatReader = new StreamReader("LocalHeatMap.txt", Encoding.Default);
						string heatStorage = heatReader.ReadToEnd ();
						heatReader.Close ();
						WWW heat_info = new WWW ("lenwohl.noip.me/weblog.php?name=" + levelFileName + "HEAT" + "&text=" + heatStorage);
						yield return heat_info;
						print (heat_info.error);
						yield return 0;
					}
				}
				else {
					theReader.Close ();
				}
			}
		} else {
			if (myWWW.text[0] == '<') {
				StreamWriter sw = File.CreateText (heatFileName);
				sw.WriteLine (GameObject.Find ("A*").GetComponent<GameOver> ().count);
				WWW survey_info = new WWW ("lenwohl.noip.me/weblog.php?name=" + levelFileName + "SCORE" + "&text=" + GameObject.Find ("A*").GetComponent<GameOver> ().count);
				yield return survey_info;
				sw.Close ();
				Application.CaptureScreenshot ("Assetsscreenshot.png");
				StreamReader heatReader = new StreamReader ("LocalHeatMap.txt", Encoding.Default);
				string heatStorage = heatReader.ReadToEnd ();
				heatReader.Close ();
				WWW heat_info = new WWW ("lenwohl.noip.me/weblog.php?name=" + levelFileName + "HEAT" + "&text=" + heatStorage);
				yield return heat_info;
				yield return 0;
			} else {
				if (int.Parse(myWWW.text) < GameObject.Find("A*").GetComponent<GameOver>().count) {
					StreamWriter sw = File.CreateText (heatFileName);
					sw.WriteLine(GameObject.Find("A*").GetComponent<GameOver>().count);
					WWW survey_info = new WWW ("lenwohl.noip.me/weblog.php?name=" + levelFileName + "SCORE" + "&text=" + GameObject.Find("A*").GetComponent<GameOver>().count);
					yield return survey_info;
					sw.Close();
					Application.CaptureScreenshot("Assetsscreenshot.png");
					print (levelFileName);
					StreamReader heatReader = new StreamReader("LocalHeatMap.txt", Encoding.Default);
					string heatStorage = heatReader.ReadToEnd ();
					heatReader.Close ();
					WWW heat_info = new WWW ("lenwohl.noip.me/weblog.php?name=" + levelFileName + "HEAT" + "&text=" + heatStorage);
					yield return heat_info;
					print (heat_info.error);
					yield return 0;
				}
			}
		}
		Destroy(GameObject.Find("A*"));
		SceneManager.LoadScene (0);
	}

	public void returnToMainMenu(){
		if (!onePress) {
			onePress = true;
			StartCoroutine ("returnToMainMenuRoutine");
		}
	}
}
