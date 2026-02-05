using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : BodyPartSensor
{
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }

    protected override void OnHit()
    {
        base.OnHit();
        //GamePlayManager.instance.GameOver();
    }

    protected override void Start()
    {
        base.Start();
    }
}
