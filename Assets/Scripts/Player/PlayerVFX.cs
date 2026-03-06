using UnityEngine;
using DG.Tweening;

public class PlayerVFX : MonoBehaviour
{
    [SerializeField] private GameObject auraObject;
    [SerializeField] private ParticleSystem auraParticles;

    private Tween auraTimerTween;

    private void OnDestroy()
    {
        transform.DOKill();

        if (auraTimerTween != null)
        {
            auraTimerTween.Kill();
        }
    }
    public void ShowAura(float duration, Color auraColor)
    {
        auraObject.SetActive(true);

        if (auraParticles != null)
        {
            var main = auraParticles.main;
            main.startColor = auraColor;
            auraParticles.Play();
        }

        if (auraTimerTween != null) auraTimerTween.Kill();

        auraTimerTween = DOVirtual.DelayedCall(duration, HideAura)
            .SetLink(gameObject);
    }

    public void HideAura()
    {
        if (auraObject != null)
        {
            auraObject.SetActive(false);
        }
        if (auraParticles != null)
        {
            auraParticles.Stop();
        }
    }
}