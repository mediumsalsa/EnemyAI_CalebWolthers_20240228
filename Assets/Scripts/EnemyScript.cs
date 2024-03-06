using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewBehaviourScript : MonoBehaviour
{

    public NavMeshAgent enemy;

    public Transform[] points;

    public Transform player;

    private int destPoint = 0;

    public float visionAngle = 45f;


    public Material patrolMat;
    public Material chaseMat;

    private Renderer enemyRenderer;


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
        enemyRenderer = GetComponent<Renderer>();
        enemyRenderer.material = patrolMat;
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
        if (CanSeePlayer())
        { 
            currentState = EnemyState.Chasing; 
        }

        enemyRenderer.material = patrolMat;
        if (!enemy.pathPending && enemy.remainingDistance < 0.5f)
        {
            GotoNextPoint();
        }
    }
    void ChasingUpdate()
    {
        if (!CanSeePlayer())
        {
            currentState = EnemyState.Patrolling;
            return;
        }

        enemyRenderer.material = chaseMat;
        enemy.destination = player.position;
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

    bool CanSeePlayer()
    {
        Vector3 direction = player.position - transform.position;
        float angle = Vector3.Angle(transform.forward, direction);

        if (angle < visionAngle * 0.5)
        {

            RaycastHit hit;
            if (Physics.Raycast(transform.position, player.position - transform.position, out hit))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    return true;
                }
            }
        }
        return false;
    }


}
