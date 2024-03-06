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

    public float attackRange = 2f;

    public Material patrolMat;
    public Material chaseMat;
    public Material searchMat;
    public Material attackMat;
    public Material retreatMat;

    private Renderer enemyRenderer;

    public float searchDuration = 5f;
    public float retreatDuration = 5f;
    private float searchTimer;
    public float retreatTimer;

    


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
            return;
        }
        if (CanAttackPlayer())
        {
            currentState = EnemyState.Attacking;
            return;
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
            currentState = EnemyState.Searching;
            searchTimer = 0f;
            return;
        }
        if (CanAttackPlayer())
        {
            currentState = EnemyState.Attacking;
            return;
        }

        enemyRenderer.material = chaseMat;
        enemy.destination = player.position;
    }
    void SearchingUpdate()
    {
        if (CanSeePlayer())
        {
            currentState = EnemyState.Chasing;
            return;
        }
        if (CanAttackPlayer())
        {
            currentState = EnemyState.Attacking;
            return;
        }

        enemyRenderer.material = searchMat;
        searchTimer += Time.deltaTime;

        if (!enemy.pathPending && enemy.remainingDistance < 0.05f)
        {
            enemy.destination = player.position;
        }
        if (searchTimer >= searchDuration)
        {
            currentState = EnemyState.Retreating;
            retreatTimer = 0f;
        }
    }
    void AttackingUpdate()
    {
        if (!CanSeePlayer())
        {
            currentState = EnemyState.Searching;
            searchTimer = 0f;
            return;
        }
        if (!CanAttackPlayer()) 
        {
            currentState = EnemyState.Chasing;
            return;
        }

        enemyRenderer.material = attackMat;
    }
    void RetreatingUpdate()
    {
        if (CanAttackPlayer())
        {
            currentState = EnemyState.Attacking;
            return;
        }

        enemyRenderer.material = retreatMat;
        retreatTimer += Time.deltaTime;

        if (!enemy.pathPending && enemy.remainingDistance < 0.05f)
        {
            GotoNextPoint();
        }
        if (retreatTimer >= retreatDuration)
        {
            currentState = EnemyState.Patrolling;
        }
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

    bool CanAttackPlayer()
    {
        return (Vector3.Distance(transform.position, player.position) < attackRange);
    }


}
