
using UnityEngine;
using System.Xml;
using System.IO;
using System.Collections;
using System.Text;

// a very simplistic level upload and random name generator script

public class Upload : MonoBehaviour
{
	void StartUpload()
	{
		StartCoroutine("UploadLevel");
	}

	void StartRead() {
		StartCoroutine ("ReadLevel");
	}

	void DeleteFile() {
		File.Delete ("testfile.txt");
	}

	IEnumerator ReadLevel() {
		WWW myWWW = new WWW ("http://www.eden.rutgers.edu/~llw57/Kindling/log.txt");
		yield return myWWW;
		//StreamWriter sw = File.CreateText ("testfile.txt");
		System.IO.File.WriteAllText ("testfile.txt", myWWW.text);
		yield return null;
	}

	IEnumerator UploadLevel()
	{
		string levelName = "3";
		string values = "1,1,1,1,2";
		string score = "0";
		string map = "1,2,3,4";
		string substring = "";
		for (int i = 0; i < values.Length; i++) {
			if (values [i].Equals (',')) {
				print (substring);
				substring = "";
			} else {
				substring += values [i];
			}
		}
		print (substring);
		// Build the URL and feed it to a WWW object
		WWW survey_info = new WWW("http://www.eden.rutgers.edu/~llw57/Kindling/weblog.php?name=" + levelName +"&text=" + values + "&score=" + score + "&map=" + map);

		// Wait for the request to send
		yield return survey_info;

		if (survey_info.error != null)
		{
			Debug.Log(survey_info.error); // You should probably check your page for errors
			// Optionally print survey_info.text to get a corresponding webpage with error messages
		}
	}

	void OnGUI()
	{
		if (GUILayout.Button ("Save Level")) {
			StartUpload ();
		} else if (GUILayout.Button ("Load File")) {
			StartRead ();
		} else if (GUILayout.Button ("Delete")) {
			DeleteFile ();
		}
	}
}