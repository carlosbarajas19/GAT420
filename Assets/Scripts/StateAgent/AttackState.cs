using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    public AttackState(StateAgent owner, string name) : base(owner, name) { }

    public override void onEnter()
    {
        Debug.Log(name + " enter");

        if(owner.enemy.TryGetComponent<StateAgent>(out StateAgent stateAgent))
        {
            stateAgent.health.value -= owner.damage.value;
        }

        owner.movement.Stop();
        owner.animator.SetTrigger("Attack");
        owner.timer.value = 1;
    }

    public override void onExit()
    {
        
    }

    public override void onUpdate()
    {
        
    }
}
