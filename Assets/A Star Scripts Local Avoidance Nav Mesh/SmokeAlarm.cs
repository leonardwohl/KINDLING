// Patrol.cs
using UnityEngine;
using System.Collections;
using System.Collections.Generic; 


public class SmokeAlarm : MonoBehaviour
{
    public GameObject[] initalAgents;
    public List<GameObject> ALLAgents;
	public bool fire = false;

    private int agentCount; 
    private bool stop = false; 



    public Material change; 


    void Start()
    {

        initalAgents = GameObject.FindGameObjectsWithTag("agent");

        foreach(GameObject g in initalAgents){
 
            ALLAgents.Add(g);
            agentCount++;

        }

    }

    void Update()
    {
        initalAgents = GameObject.FindGameObjectsWithTag("agent");

        if(agentCount < initalAgents.Length){
            foreach(GameObject a in initalAgents){

                ALLAgents.Add(a);
                agentCount++;
            }

        }
     if(!stop){
     if (fire) { 
   

        gameObject.GetComponent<Renderer>().material = change;

        foreach(GameObject g in ALLAgents){
            if(g != null ){
                if(g.tag =="agent"){
                    if(g.GetComponent<Patroling>() != null){
                    g.GetComponent<Patroling>().enabled = false;
                    g.GetComponent<Unit>().enabled = true; 

                    }
                g.GetComponent<FieldOfView>().timeToGo = true;


                }
            }
        }
        stop = true; 
       }
    }


    }

}
