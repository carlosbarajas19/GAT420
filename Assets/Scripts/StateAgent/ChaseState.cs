using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    public ChaseState(StateAgent owner, string name) : base(owner, name) { }

    public override void onEnter()
    {
        Debug.Log(name + " enter");
        //owner.movement.Resume();
    }

    public override void onExit()
    {
        Debug.Log(name + " exit");
    }

    public override void onUpdate()
    {
        
        owner.movement.MoveTowards(owner.enemy.transform.position);
        Debug.Log(name + " update");
    }

}
