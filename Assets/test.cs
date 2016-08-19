using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System.Text;

public class test : MonoBehaviour {

	public InputField inputField;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator LoadLevelRoutine() {
		//this saves the level
		StreamReader theReader = new StreamReader("saveFile.txt", Encoding.Default);
		string temp = theReader.ReadToEnd ();
		print (temp);

		WWW survey_info = new WWW ("lenwohl.noip.me/weblog.php?name=" + inputField.text + "&text=" + temp);

		// Wait for the request to send
		yield return survey_info;

		if (survey_info.error != null) {
			Debug.Log (survey_info.error); // You should probably check your page for errors
			// Optionally print survey_info.text to get a corresponding webpage with error messages
		} else {
			//this loads the level
			WWW myWWW = new WWW ("lenwohl.noip.me/" + inputField.text+".txt");
			yield return myWWW;
			temp = myWWW.text;
			temp = temp.Replace ('$', '\n');
			theReader.Close();
			print (temp);
			File.WriteAllText ("localOnlineFile.txt", temp);
		}
	}

	public void LoadLevel() {
		StartCoroutine ("LoadLevelRoutine");
	}
}
