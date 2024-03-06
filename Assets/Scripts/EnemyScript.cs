using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewBehaviourScript : MonoBehaviour
{

    public NavMeshAgent enemy;

    public Transform[] points;
    private int destPoint = 0;



    public enum EnemyState
    {
        Patrolling,
        Chasing,
        Searching,
        Attacking,
        Retreating
    }
    private EnemyState currentState;


    void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
        currentState = EnemyState.Patrolling;
        GotoNextPoint();
    }


    void Update()
    {

        switch (currentState)
        {
            case EnemyState.Patrolling:
                PatrollingUpdate();
                break;

            case EnemyState.Chasing:
                ChasingUpdate();
                break;

            case EnemyState.Searching:
                SearchingUpdate();
                break;

            case EnemyState.Attacking:
                AttackingUpdate();
                break;

            case EnemyState.Retreating:
                RetreatingUpdate();
                break;
        }

    }

    void PatrollingUpdate()
    {
        if (!enemy.pathPending && enemy.remainingDistance < 0.5f)
        {
            GotoNextPoint();
        }
    }
    void ChasingUpdate()
    {

    }
    void SearchingUpdate()
    {

    }
    void AttackingUpdate()
    {

    }
    void RetreatingUpdate()
    {

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




}
