using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DbgMovement : MonoBehaviour
{
    public Camera cam;
    public NavMeshAgent agent;
    [Range(0,2)]
    public int bt;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(bt))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray,out hit))
            {
                agent.SetDestination(hit.point);
            }
        }       
    }
}
