using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leg_01_R : BodyPartSensor
{
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }

    protected override void Start()
    {
        base.Start();

        //transform.localEulerAngles = new Vector3(initialRotation.x, startAngle, initialRotation.z);
    }

    public override void RotateLimb_2()
    {
        base.RotateLimb_2();
        RotateLeg(startAngle);
    }
}
