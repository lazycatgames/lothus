using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{

    public NavMeshAgent agent;
    public Transform player;

    public LayerMask groundMask, playerMask;

    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    public float timeBetweenAttacks;
    bool alreadyAttacked;

    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;




    void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }


    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerMask);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerMask);

        if (!playerInSightRange && !playerInAttackRange) Patrol();

        if (playerInSightRange && !playerInAttackRange) Chase();

        if (playerInSightRange && playerInAttackRange) Attack();
    }

    void Patrol()
    {
        if (!walkPointSet) SearchWalkPoint();
        if (walkPointSet) agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f) walkPointSet = false;
    }

    void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if(Physics.Raycast(walkPoint, -transform.up, 2f, groundMask)) walkPointSet = true;


    }

    void Chase()
    {
        agent.SetDestination(player.position);
    }

    void Attack()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
