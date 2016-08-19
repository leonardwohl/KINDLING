using UnityEngine;
using System.Collections;

public class EditorCameraMove : MonoBehaviour {

	public int Boundary = 10;
	public int speed = 10;
	private int theScreenWidth;
	private int theScreenHeight;

	private float ZoomAmount = 0; //With Positive and negative values
	private float MaxToClamp = 10;
	private float ROTSpeed = 10;

	// Use this for initialization
	void Start () {
		theScreenWidth = Screen.width;
		theScreenHeight = Screen.height;
	}

	// Update is called once per frame
	void Update () {

		ZoomAmount = ZoomAmount + Input.GetAxis("Mouse ScrollWheel"); 

		ZoomAmount = Mathf.Clamp(ZoomAmount, -MaxToClamp, MaxToClamp);
		
		float translate = Mathf.Min(Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")), MaxToClamp-Mathf.Abs(ZoomAmount));
		
		gameObject.transform.Translate(0,translate * ROTSpeed * Mathf.Sign(Input.GetAxis("Mouse ScrollWheel")),0);

		if (Input.mousePosition.x > theScreenWidth - Boundary)
		{
			//transform.position.x += speed * Time.deltaTime; // move on +X axis
			transform.Translate(new Vector3(1, 0, 0) * speed * Time.deltaTime);
		}
		if (Input.mousePosition.x < 0 + Boundary)
		{
			//transform.position.x -= speed * Time.deltaTime; // move on -X axis
			transform.Translate(new Vector3(-1, 0, 0) * speed * Time.deltaTime);
		}
		if (Input.mousePosition.y > theScreenHeight - Boundary)
		{
			//transform.position.z += speed * Time.deltaTime; // move on +Z axis
			transform.Translate(new Vector3(0, 0 ,1) * speed * Time.deltaTime);
		}
		if (Input.mousePosition.y < 0 + Boundary)
		{
			//transform.position.z -= speed * Time.deltaTime; // move on -Z axis
			transform.Translate(new Vector3(0, 0 ,-1) * speed * Time.deltaTime);
		}
	}
}
