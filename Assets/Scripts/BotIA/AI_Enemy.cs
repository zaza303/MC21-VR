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
    private Animator anim;
    private Transform target;
    private float idleSpeed;

    void Start()
    {
        anim = this.GetComponent<Animator>();
        navMesh = GetComponent<NavMeshAgent>();
        rigidBody = GetComponent<Rigidbody>();

        rigidBody.freezeRotation = true;

        target = WayPoints[currentWayPoint];
        idleSpeed = anim.speed;
    }

    void Update()
    {
        navMesh.acceleration = Speed;
        navMesh.stoppingDistance = StopDistance;
        anim.speed = navMesh.velocity.magnitude / 3;

        float distance = Vector3.Distance(transform.position, target.position);

        if (anim.GetBool("isWalking") && distance <= StopDistance)
        {
            currentWayPoint++;
            if (currentWayPoint >= WayPoints.Length)
            {
                anim.SetBool("isWalking", false);
                anim.SetBool("isIdle", true);
                anim.speed = idleSpeed;
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
        anim.SetBool("isWalking", true);
        anim.SetBool("isIdle", false);
    }
}
