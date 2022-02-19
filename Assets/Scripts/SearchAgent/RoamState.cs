using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoamState : State
{
    public RoamState(StateAgent owner, string name) : base(owner, name) { }

    public override void onEnter()
    {
        Quaternion rotation = Quaternion.Euler(0,Random.Range(-90, 90),0); //< create a quaternion with a random angle between - 90 and 90 and rotate about the y axis>;
        Vector3 forward = rotation * owner.transform.forward;//< set the forward vector by rotating the owner transform forward with the quaternion rotation >;
        Vector3 destination = owner.transform.position + forward * Random.Range(10, 15); //< position of the owner + forward + random float between 10 and 15 >;

        owner.movement.MoveTowards(destination);
        owner.movement.Resume();
        owner.atDestination.value = false;
    }

    public override void onExit()
    {
        //
    }

    public override void onUpdate()
    {
        var posMagnitude = owner.transform.position.magnitude;
        var desMagnitude = owner.movement.destination.magnitude;
        if (posMagnitude - desMagnitude <= 1.5) //< get distance between owner position and movement destination >
        {
            owner.atDestination.value = true;

        }
    }
    
}
