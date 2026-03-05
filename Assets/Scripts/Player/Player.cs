using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerControls controls { get; private set; }

    public PlayeMovement movement { get; private set; }

    //public PlayerCollision collision { get; private set; }
    public PlayerLimb limb { get; private set; }
    public PlayerShadow shadow { get; private set; }
    public PlayerHealth health { get; private set; }
    public PlayerArea area { get; private set; }
    public PlayerVFX vfx { get; private set; }

    private void Awake()
    {
        controls = new PlayerControls();

        movement = GetComponent<PlayeMovement>();
        //collision = GetComponent<PlayerCollision>();
        limb = GetComponent<PlayerLimb>();
        shadow = GetComponent<PlayerShadow>();
        health = GetComponent<PlayerHealth>();
        area = GetComponentInChildren<PlayerArea>();
        vfx = GetComponentInChildren<PlayerVFX>();

    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
