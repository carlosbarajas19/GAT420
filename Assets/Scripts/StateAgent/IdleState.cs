using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public IdleState(StateAgent owner, string name) : base(owner, name) {}

    public override void onEnter()
    {
        Debug.Log(name + " enter");
        //owner.movement.Stop();
        owner.timer.value = 2;
    }

    public override void onExit()
    {
        Debug.Log(name + " exit");
    }

    public override void onUpdate()
    {
        Debug.Log(name + " update");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
