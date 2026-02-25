using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_Portal : Obstacles
{
    private bool isUp = true;
    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void Update()
    {
        if (isUp) {
            base.Update();

            if (gameObject.transform.position.y >= -2)
            {
                isUp = false;
            }

        }
    }
}
