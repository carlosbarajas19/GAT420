using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : State
{
    public DeathState(StateAgent owner, string name) : base(owner, name) { }

    public override void onEnter()
    {
        owner.movement.Stop();
        owner.animator.SetTrigger("Death");

        GameObject.Destroy(owner.gameObject, 3);
    }

    public override void onExit()
    {
        
    }

    public override void onUpdate()
    {
        
    }

}
