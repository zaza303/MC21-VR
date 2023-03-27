using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]

public class AI_Enemy : MonoBehaviour
{
    public Transform[] WayPoints;
    public int Speed = 5;
    public float StopDistance = 0.1f;

    private int currentWayPoint = 0;
    private NavMeshAgent navMesh;
    private Rigidbody rigidBody;
    private Animator animator;
    private Transform target;
    private float idleSpeed;

    void Start()
    {
        animator = this.GetComponent<Animator>();
        navMesh = GetComponent<NavMeshAgent>();
        rigidBody = GetComponent<Rigidbody>();

        rigidBody.freezeRotation = true;

        target = WayPoints[currentWayPoint];
        idleSpeed = animator.speed;
    }

    void Update()
    {
        //navMesh.acceleration = Speed;
        navMesh.stoppingDistance = StopDistance;
        //animator.speed = navMesh.velocity.magnitude / 3;

        float distance = Vector3.Distance(transform.position, target.position);

        if (animator.GetBool("isWalking") && distance <= StopDistance)
        {
            currentWayPoint++;
            if (currentWayPoint >= WayPoints.Length)
            {
                animator.SetBool("isWalking", false);
                animator.SetBool("isIdle", true);
                animator.speed = idleSpeed;
            }
            else
            {
                target = WayPoints[currentWayPoint];
            }
        }
        navMesh.SetDestination(target.position);
    }

    public void StartWalking()
    {
        currentWayPoint = 0;
        target = WayPoints[currentWayPoint];
        animator.SetBool("isWalking", true);
        animator.SetBool("isIdle", false);
    }
}
