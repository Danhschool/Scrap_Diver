using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : BodyPartSensor
{
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }

    protected override void Start()
    {
        base.Start();
    }
}
