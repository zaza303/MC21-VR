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
    public float StopDistance = 0.3f;

    private int currentWayPoint = 0;
    private NavMeshAgent navMesh;
    private Rigidbody rigidBody;
    private Animator anim;
    private Transform target;

    void Start()
    {
        anim = this.GetComponent<Animator>();
        navMesh = GetComponent<NavMeshAgent>();
        rigidBody = GetComponent<Rigidbody>();

        rigidBody.freezeRotation = true;

        target = WayPoints[currentWayPoint];
    }

    void Update()
    {
        navMesh.acceleration = Speed;
        navMesh.stoppingDistance = StopDistance;

        float distance = Vector3.Distance(transform.position, target.position);

        if (anim.GetBool("isWalking"))
        {
            target = WayPoints[currentWayPoint];
            if (distance <= StopDistance)
            {
                currentWayPoint = (currentWayPoint + 1) % WayPoints.Length;
                anim.SetBool("isWalking", false);
                anim.SetBool("isIdle", true);
            }
        }

        navMesh.SetDestination(target.position);
    }
}
