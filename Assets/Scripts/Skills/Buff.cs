using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public abstract class Buff : MonoBehaviour
{

    protected RTSUnit unit;

    public abstract void Run(RTSUnit unit);

    public void Remove()
    {
        unit.RemoveBuff(this);
        Destroy(this);
    }
}
