using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeState : State
{
    float angle;
    float distance;
    public EvadeState(StateAgent owner, string name) : base(owner, name) { }

    public override void onEnter()
    {
        Debug.Log(name + " enter");
        angle = owner.perception.angle;
        distance = owner.perception.distance;

        owner.perception.angle = 180;
        owner.perception.distance = 10;
        if(owner.TryGetComponent<SphereCastPerception>(out SphereCastPerception perception))
        {
            perception.numRaycast = 40;
        }
        owner.movement.Resume();
    }

    public override void onExit()
    {
        if (owner.TryGetComponent<SphereCastPerception>(out SphereCastPerception perception))
        {
            perception.numRaycast = 20;
        }
        owner.perception.angle = angle;
        owner.perception.distance = distance;
    }

    public override void onUpdate()
    {
        Vector3 direction = (owner.transform.position - owner.enemy.transform.position).normalized;
        owner.movement.MoveTowards(owner.transform.position + direction);
    }
}
