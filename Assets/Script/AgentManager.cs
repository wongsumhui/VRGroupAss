using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour
{
    GameObject[] agents;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agents = GameObject.FindGameObjectsWithTag("AI");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if(Physics.Raycast(Camera.main.
                ScreenPointToRay(Input.mousePosition), out 
                hit, 100))
            {
                foreach(GameObject a in agents)
                {
                    a.GetComponent<AIControl>().agent.
                        SetDestination(hit.point);
                }
            }
        }
    }
}
