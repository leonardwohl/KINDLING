using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {

	public int Boundary = 25;
	public int speed = 10;
	private int theScreenWidth;
	private int theScreenHeight;

	// Use this for initialization
	void Start () {
		theScreenWidth = Screen.width;
		theScreenHeight = Screen.height;
	}
	
	// Update is called once per frame
	void Update () {

		if (gameObject.GetComponent<resize> () != null && gameObject.GetComponent<resize>().inGameMode == true) {
			while (Input.GetKeyDown (KeyCode.Q)) {

			}
		}
		if (Input.mousePosition.x > theScreenWidth - Boundary)
		{
			//transform.position.x += speed * Time.deltaTime; // move on +X axis
			transform.Translate(new Vector3(1, 0 ,-1) * speed * Time.deltaTime);
		}
		if (Input.mousePosition.x < 0 + Boundary)
		{
			//transform.position.x -= speed * Time.deltaTime; // move on -X axis
			transform.Translate(new Vector3(-1, 0, 1) * speed * Time.deltaTime);
		}
		if (Input.mousePosition.y > theScreenHeight - Boundary)
		{
			//transform.position.z += speed * Time.deltaTime; // move on +Z axis
			transform.Translate(new Vector3(1, 0 ,1) * speed * Time.deltaTime);
		}
		if (Input.mousePosition.y < 0 + Boundary)
		{
			//transform.position.z -= speed * Time.deltaTime; // move on -Z axis
			transform.Translate(new Vector3(-1, 0 ,-1) * speed * Time.deltaTime);
		}
	}
}
