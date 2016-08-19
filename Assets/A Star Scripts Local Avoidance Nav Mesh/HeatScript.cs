using UnityEngine;
using System.Collections;

public class HeatScript : MonoBehaviour {

	// The distance allowed to affect 'heat'
	float allowableDistance = 1;


	// How much to adjust the 'heat' by.
	float colorAdjuster = .2f;


	// Creates a material from shader&texture references
	Texture texture;
	Color color;


	// This stops the material from continuously altering color
	private bool thisCubeDone = false;

	void Start()
	{
		// Wait till map is finished loading to check nearby marks
		if(!thisCubeDone)
			setColor();
	}


	void setColor()
	{
		GameObject[] heatboxes = GameObject.FindGameObjectsWithTag("heatBox");
		for(int i = 0; i < heatboxes.Length; i++)
		{
			Vector2 distCheck = new Vector2(heatboxes[i].transform.position.x, heatboxes[i].transform.position.z) - 
				new Vector2(gameObject.transform.position.x, gameObject.transform.position.z);
			float dist = distCheck.sqrMagnitude;
			if( dist <= allowableDistance )
			{
				// Here's where the color is actually adjusted.
				// It starts blue (cool) by default and moves towards red (hot)
				gameObject.transform.GetComponent<Renderer>().material.color = new Color(gameObject.transform.GetComponent<Renderer>().material.color.r + colorAdjuster, gameObject.transform.GetComponent<Renderer>().material.color.g, gameObject.transform.GetComponent<Renderer>().material.color.b, 1);
				gameObject.transform.GetComponent<Renderer>().material.color = new Color(gameObject.transform.GetComponent<Renderer>().material.color.r, gameObject.transform.GetComponent<Renderer>().material.color.g, gameObject.transform.GetComponent<Renderer>().material.color.b - colorAdjuster, 1);
			}
		}
		thisCubeDone = true;
	}
}
