using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{
    public PatrolState(StateAgent owner, string name) : base(owner, name) {}

    public override void onEnter()
    {
        owner.timer.value = Random.Range(5, 10);
        owner.pathFollower.targetNode = owner.pathFollower.pathNodes.GetNearestNode(owner.transform.position);
        owner.movement.Resume();
        Debug.Log(name + " enter");
    }

    public override void onExit()
    {
        Debug.Log(name + " exit");
        owner.movement.Stop();
    }

    public override void onUpdate()
    {
        Debug.Log(name + " update");

        owner.pathFollower.Move(owner.movement);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            owner.stateMachine.SetState(owner.stateMachine.StateFromName(typeof(IdleState).Name));
        }
    }
}
