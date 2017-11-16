using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarAI : MonoBehaviour {

    [SerializeField]
    Transform destination_R; //Spawnpoint
    Transform destination_L; //Destinationpoint
    NavMeshAgent navAgent;
    Boolean foundRWay=false;
    Boolean foundLWay = true;
 
    List<GameObject> waypointsInRange = new List<GameObject>();

    // Use this for initialization
    void Start () {
        navAgent = this.GetComponent<NavMeshAgent>(); //Get the NavMeshAgent
        if (navAgent == null)
        {
            Debug.LogError("NavAgent not found on " + gameObject.name);
        }
        else
        {
            destination_L = destination_R.GetChild(0).transform; //Get the child of the Spawnpoint/new Waypoint
            SetNextDestination(); //Get the next destination
        }
	}


    // Update is called once per frame
    void Update () {
        if (destination_L != null)
        {
            if (gameObject.transform.position.x == destination_L.position.x & gameObject.transform.position.z == destination_L.position.z)
            {
                foundLWay = true;
                if (foundLWay == true)
                {
                    Debug.Log("Bei L angekommen!");
                    WhereToDrive(gameObject.GetComponent<Collider>());
                }
            }
            else
            {
                foundLWay = false;
            }
            if (gameObject.transform.position.z == destination_R.position.z & gameObject.transform.position.x == destination_R.position.x & foundRWay==true)
            {
                Debug.Log("Bei R angekommen!");
                SetNextDestination();
                foundRWay = false;
                foundLWay = false;
            }
        }
	}

    private void WhereToDrive(Collider other)
    {
        
        if (waypointsInRange == null)
        {
            Debug.LogError("No way found!");
        }
        else if(waypointsInRange.Count==1)
        { 
            Debug.Log("Nur eins R gefunden!");
            destination_R = waypointsInRange[0].transform;
            destination_L = destination_R.GetChild(0);
            foundRWay = true;
            Vector3 tVector = destination_R.transform.position; //Get the position of the child (next Pos.)
            navAgent.SetDestination(tVector); //Move to  
        }
        else if (waypointsInRange.Count > 1)
        {
            Debug.Log("Viele R gefunden! "+waypointsInRange.Count);
            destination_R = waypointsInRange[UnityEngine.Random.Range(0, waypointsInRange.Count)].transform;
            destination_L = destination_R.GetChild(0);
            foundRWay = true;
            Vector3 tVector = destination_R.transform.position; //Get the position of the child (next Pos.)
            navAgent.SetDestination(tVector); //Move to
        }
    }

    private void SetNextDestination()
    {
        if (destination_L != null)
        {
            Vector3 tVector = destination_L.transform.position; //Get the position of the child (next Pos.)
            navAgent.SetDestination(tVector); //Move to
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "R" & other.gameObject.tag != "L")
        {
            waypointsInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "R" & other.gameObject.tag != "L")
        {
            waypointsInRange.Remove(other.gameObject);
        }
    }
}
