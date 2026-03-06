using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : BodyPartSensor
{
    [SerializeField] private bool isSpine = false;
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }

    protected override void OnHit()
    {
        base.OnHit();

        if (isSpine) return;
        player.health.OnDie();
    }

    protected override void Start()
    {
        base.Start();
    }
}
