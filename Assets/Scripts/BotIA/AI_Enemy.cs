using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]

public class AI_Enemy : MonoBehaviour
{
    NavMeshAgent nm;
    Rigidbody rb;
    Animator anim;
    public Transform Target;
    public Transform[] WayPoints;
    public int Cur_WayPoints;
    public int speed;
    public float stop_distance;
    public float PauseTimer;
    [SerializeField]
    private float cur_timer;

    void Start()
    {
        anim = GetComponent<Animator>();
        nm = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        rb.freezeRotation = true;

        Target = WayPoints[Cur_WayPoints];
        cur_timer = PauseTimer;
    }

    void Update()
    {
        //Settings Updated
        nm.acceleration = speed;
        nm.stoppingDistance = stop_distance;

        float distance = Vector3.Distance(transform.position, Target.position);


        //Move to Waypoint
        if (distance > stop_distance && WayPoints.Length > 0)
        {

            //Find Waypoint
            anim.SetBool("isWalking", true);
            anim.SetBool("Stop", false);
            Target = WayPoints[Cur_WayPoints];
            
        }
        else if (distance <= stop_distance && WayPoints.Length > 0)
        {
            if (cur_timer > 0)
            {
                cur_timer -= 0.01f;
                anim.SetBool("isWalking", false);
                anim.SetBool("Stop", true);
            }
            if (cur_timer <= 0)
            {
                Cur_WayPoints++;
                if (Cur_WayPoints >= WayPoints.Length)
                {
                    Cur_WayPoints = 0;
                }
                Target = WayPoints[Cur_WayPoints];
                cur_timer = PauseTimer;
            }
        }

        nm.SetDestination(Target.position);

    }
}
