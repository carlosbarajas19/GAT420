using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public abstract void onEnter();
    public abstract void onExit();
    public abstract void onUpdate();

}
