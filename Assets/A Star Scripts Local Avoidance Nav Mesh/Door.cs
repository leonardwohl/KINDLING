using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Door : MonoBehaviour {

	public GameObject DoorToDie;
	public GameObject DoorItself;
	public bool open = false;

	void OnTriggerEnter(Collider col){
		if(col.gameObject.tag == "agent"){
			/*Debug.Log("kindle found.");

			col.GetComponent<Unit>().enabled = false;
			Debug.Log("stop?");
			//yield return new WaitForSeconds(5);
			//col.Start();
			col.GetComponent<Unit>().enabled = true;*/
			if (!open) {
				List<Node> gridLocation = DoorToDie.GetComponent<DoorGridLocation> ().gridLocation;
				open = true;
				foreach (Node check in gridLocation) {
					check.door = false;
				}
				//DoorToDie.layer = 0;
				if (gameObject.transform.parent != null && gameObject.transform.parent.name == "Door(Clone)") {
					Destroy (gameObject.transform.parent.gameObject);
				}
				Destroy (DoorToDie);
				Destroy (DoorItself);
			}
		}
	}
    
}
