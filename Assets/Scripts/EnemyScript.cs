using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewBehaviourScript : MonoBehaviour
{

    public NavMeshAgent enemy;

    public Transform[] points;
    private int destPoint = 0;


    void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
        GotoNextPoint();
    }

    void GotoNextPoint()
    {
        if (points.Length == 0)
        {
            return;
        }

        enemy.destination = points[destPoint].position;

        destPoint = (destPoint + 1) % points.Length;
    }


    void Update()
    {
        if (!enemy.pathPending && enemy.remainingDistance < 0.5f)
        {
            GotoNextPoint();
        }
    }

}
