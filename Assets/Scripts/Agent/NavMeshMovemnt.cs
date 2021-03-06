using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshMovemnt : Movement
{
    

    public override Vector3 velocity { get => navMeshAgent.velocity; set => navMeshAgent.velocity = value; }
    /*public override Vector3()
    {

    }*/

    public NavMeshAgent navMeshAgent;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public override Vector3 destination
    {
        get => navMeshAgent.destination;
        set => navMeshAgent.destination = value;
    }


    private void Update()
    {
        navMeshAgent.speed = movementData.maxSpeed;
        navMeshAgent.acceleration = movementData.maxForce;
        navMeshAgent.angularSpeed = movementData.turnRate;
    }

    public override void ApplyForce(Vector3 force)
    {
       //
    }

    public override void MoveTowards(Vector3 target)
    {
        navMeshAgent.SetDestination(target);
    }

    public override void Resume()
    {
        navMeshAgent.isStopped = false;
    }

    public override void Stop()
    {
        navMeshAgent.isStopped = true;
    }
}
