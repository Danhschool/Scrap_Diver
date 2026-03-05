using UnityEngine;
using DG.Tweening;

public class PlayerVFX : MonoBehaviour
{
    [SerializeField] private GameObject auraObject;
    [SerializeField] private ParticleSystem auraParticles;

    public void ShowAura(float duration, Color auraColor)
    {
        auraObject.SetActive(true);

        if (auraParticles != null)
        {
            var main = auraParticles.main;
            main.startColor = auraColor;
            auraParticles.Play();
        }

        DOVirtual.DelayedCall(duration, HideAura);
    }

    public void HideAura()
    {
        if (auraParticles != null)
        {
            auraParticles.Stop();
        }
        auraObject.SetActive(false);
    }
}