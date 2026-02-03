using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerLimb : MonoBehaviour
{
    private Player player;

    [SerializeField] private Arm_01_L arm_01_L;
    [SerializeField] private Arm_02_L arm_02_L;
    [SerializeField] private Leg_01_L leg_01_L;
    [SerializeField] private Leg_02_L leg_02_L;
    [SerializeField] private Arm_01_R arm_01_R;
    [SerializeField] private Arm_02_R arm_02_R;
    [SerializeField] private Leg_01_R leg_01_R;
    [SerializeField] private Leg_02_R leg_02_R;


    private void Start()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {

        arm_01_L.RotateLimb_2();
        arm_02_L.RotateLimb_2();
        //leg_01_L.RotateLimb_2();
        //leg_02_L.RotateLimb_2();
        arm_01_R.RotateLimb_2();
        arm_02_R.RotateLimb_2();
        //leg_01_R.RotateLimb_2();
        //leg_02_R.RotateLimb_2();
    }

    public void GetHit(bool _isLimb, GameObject _Mesh, Collider _skel)
    {
        if(!_isLimb)
        {
            // goi 2 lan
            GameOver();
        }
        else
        {
            RemoveLimb(_Mesh, _skel);
        }
    }

    public void GameOver()
    {
        Debug.Log("Game over");
    }
    public void RemoveLimb(GameObject _mesh, Collider _skel)
    {
        _mesh.SetActive(false);
        _skel.enabled = false;
    }

}
