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

        Invoke(nameof(RunGameContinueToEnd), 2f);
    }

    protected override void Start()
    {
        base.Start();
    }

    private void RunGameContinueToEnd()
    {
        GamePlayManager.instance.GameContinueToEnd();
    }
}
