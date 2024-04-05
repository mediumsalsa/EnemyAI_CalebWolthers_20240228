using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

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

    public Renderer enemyRenderer;

    public float searchDuration = 5f;
    public float retreatDuration = 5f;
    private float searchTimer;
    public float retreatTimer;

    private Transform target;

    public ThirdPersonCharacter character;
    


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
        enemy.updateRotation = false;

        currentState = EnemyState.Patrolling;
        target = points[destPoint];
        enemy = GetComponent<NavMeshAgent>();
        //enemyRenderer = GetComponent<Renderer>();
        enemyRenderer.material = patrolMat;
        GoToNextPoint();
    }


    //Constantly checking the states for updates
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

        if (enemy.remainingDistance > enemy.stoppingDistance)
        {
            character.Move(enemy.desiredVelocity, false, false);
        }
        else
        {
            character.Move(Vector3.zero, false, false);
        }


    }

    //Patrols between 4 locations, utill the player comes into sight
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
            target = points[destPoint];
            GoToNextPoint();
        }
    }

    //Sets the enemies target to the player's location
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

        target = player;
        enemyRenderer.material = chaseMat;
        enemy.destination = target.position;
    }

    //Goes to the last known position of the player, for a set time, and then retreats.
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
            target = player;
            enemy.destination = target.position;
        }
        if (searchTimer >= searchDuration)
        {
            currentState = EnemyState.Retreating;
            retreatTimer = 0f;
            target = points[destPoint];
        }
    }

    //Just turns the enemy red to show that he is attacking the player, if the conditions are met
    void AttackingUpdate()
    {

        if (!CanAttackPlayer()) 
        {
            currentState = EnemyState.Chasing;
            return;
        }

        enemy.destination = transform.position;
        enemyRenderer.material = attackMat;
    }

    //Enemy retreats, goes to the closest patrol point, for a set time, and then switches back to patrol
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
            target = points[destPoint];
            GoToNextPoint();
        }
        if (retreatTimer >= retreatDuration)
        {
            currentState = EnemyState.Patrolling;
        }
    }


    //Sends enemy to the next patrol point
    //I have it in it's own method because retreating sends the enemy to a patrol point too
    void GoToNextPoint()
    {
        if (points.Length == 0)
        {
            return;
        }

        enemy.destination = points[destPoint].position;

        destPoint = (destPoint + 1) % points.Length;
    }


    //Returns a bool, depending on if the player is in vision or not
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


    //Returns a bool depending on if the player is in attak range or not, regardless of if the player is in sight.
    bool CanAttackPlayer()
    {
        return (Vector3.Distance(transform.position, player.position) < attackRange);
    }


}
