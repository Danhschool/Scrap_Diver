using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerLimb : MonoBehaviour
{
    private Player player;

    public bool isShielded = false;
    private bool isInvincible = false;
    private Coroutine shieldCoroutine;

    private struct BrokenLimb
    {
        public GameObject mesh;
        public Collider skel;
    }
    private List<BrokenLimb> brokenLimbs = new List<BrokenLimb>();
    private int totalLimbs = 8;

    //[SerializeField] private Arm_01_L arm_01_L;
    //[SerializeField] private Arm_02_L arm_02_L;
    //[SerializeField] private Leg_01_L leg_01_L;
    //[SerializeField] private Leg_02_L leg_02_L;
    //[SerializeField] private Arm_01_R arm_01_R;
    //[SerializeField] private Arm_02_R arm_02_R;
    //[SerializeField] private Leg_01_R leg_01_R;
    //[SerializeField] private Leg_02_R leg_02_R;


    private void Start()
    {
        player = GetComponent<Player>();
    }

    //private void Update()
    //{

    //    //arm_01_L.RotateLimb_2();
    //    //arm_02_L.RotateLimb_2();
    //    //leg_01_L.RotateLimb_2();
    //    //leg_02_L.RotateLimb_2();
    //    //arm_01_R.RotateLimb_2();
    //    //arm_02_R.RotateLimb_2();
    //    //leg_01_R.RotateLimb_2();
    //    //leg_02_R.RotateLimb_2();
    //}

    public void ActivateShield(float duration)
    {
        isShielded = true;
        if (shieldCoroutine != null) StopCoroutine(shieldCoroutine);
        shieldCoroutine = StartCoroutine(ShieldCountdown(duration));
    }

    private IEnumerator ShieldCountdown(float duration)
    {
        yield return new WaitForSeconds(duration);
        isShielded = false;
    }

    public void GetHit(bool _isLimb, GameObject _Mesh, Collider _skel)
    {
        if (isInvincible) return;

        if (isShielded)
        {
            BreakShield();
            return;
        }
        if (_isLimb)
            RemoveLimb(_Mesh, _skel);

    }

    public void RemoveLimb(GameObject _mesh, Collider _skel)
    {
        _mesh.SetActive(false);
        _skel.enabled = false;
        brokenLimbs.Add(new BrokenLimb { mesh = _mesh, skel = _skel });
        UpdateMovementPenalty();
    }
    public void FullRepair()
    {
        foreach (var limb in brokenLimbs)
        {
            limb.mesh.SetActive(true);
            limb.skel.enabled = true;
        }
        brokenLimbs.Clear();
        UpdateMovementPenalty();
    }

    private void UpdateMovementPenalty()
    {
        int activeLimbs = totalLimbs - brokenLimbs.Count;
        float healthRatio = (float)activeLimbs / totalLimbs;
        player.movement.ApplyMovementPenalty(healthRatio);
    }
    private void BreakShield()
    {
        isShielded = false;
        if (shieldCoroutine != null) StopCoroutine(shieldCoroutine);

        isInvincible = true;
        DOVirtual.DelayedCall(0.1f, () => isInvincible = false).SetLink(gameObject);
    }
}
