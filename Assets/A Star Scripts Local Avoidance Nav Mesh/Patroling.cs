// Patrol.cs
using UnityEngine;
using System.Collections;



public class Patroling : MonoBehaviour
{

    public GameObject ALL;
	public bool fire = false;

    private Transform[] points = new Transform[4];
    public Transform Target;
    public Transform Target2;
    public Transform Target3;
    public Transform Target4;


    private Transform[] otherAltertingBeans = new Transform[4];
    public Transform AltertingBean1;
    public Transform AltertingBean2;

    public Material change; 

    private int destPoint;
    private NavMeshAgent agent;




    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        points[0] = Target;
        points[1] = Target2;
        points[2] = Target3;
        points[3] = Target4; 

        otherAltertingBeans[0] = AltertingBean1;
        otherAltertingBeans[1] = AltertingBean2;


     
        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        destPoint = 1;
        GotoNextPoint();
    }


    void GotoNextPoint()
    {
        

        // Set the agent to go to the currently selected destination.
        agent.SetDestination(points[destPoint].position); 
    

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        //destPoint = (destPoint + 1) % points.Length;

        if (destPoint == 0)
            destPoint = 1;
        else if (destPoint == 1) 
            destPoint = 2;
        else if(destPoint ==2 )
            destPoint = 3;
        else
            destPoint = 0;


    }


    void Update()
    {
        // Choose the next destination point when the agent gets
        // close to the current one.

  

        if (fire) { 

            gameObject.GetComponent<Unit>().enabled = true;

			int children = ALL.transform.childCount;
            for(int i = 0; i<children; i++){
               if(ALL.transform.GetChild(i).gameObject.activeInHierarchy){
                    ALL.transform.GetChild(i).GetComponent<FieldOfView>().timeToGo = true;

                    //ALL.transform.GetChild(i).GetComponent<Unit>().enabled = true;
                }

            }
    
			if(AltertingBean1 != null ){
				AltertingBean1.GetComponent<Unit>().enabled = true;
				AltertingBean1.GetComponent<Unit>().repath = true;
				AltertingBean1.GetComponent<FieldOfView>().timeToGo = true;
				AltertingBean1.GetComponent<Patroling>().enabled = false;

			}

			if(AltertingBean2 != null){
				AltertingBean2.GetComponent<Unit>().enabled = true;
				AltertingBean2.GetComponent<Unit>().repath = true;            
				AltertingBean2.GetComponent<FieldOfView>().timeToGo = true;
				AltertingBean2.GetComponent<Patroling>().enabled = false;
			}

            GetComponent<Renderer>().material = change;
			            
            gameObject.GetComponent<Unit> ().repath = true;

            gameObject.GetComponent<FieldOfView>().timeToGo = true;
            gameObject.GetComponent<Patroling>().enabled = false;

        }


        if (agent.remainingDistance < 1.0f)
            GotoNextPoint();
    }
}