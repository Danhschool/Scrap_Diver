using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public Transform[] backgroundLayers;
    public float[] parallaxMultipliers;
    public Transform targetToTrack;

    private void Update()
    {
        MoveParallaxBackground();
    }

    public void MoveParallaxBackground()
    {
        if (targetToTrack == null) return;

        for (int i = 0; i < backgroundLayers.Length; i++)
        {
            if (backgroundLayers[i] != null)
            {
                float parallaxX = targetToTrack.position.x * 5 * parallaxMultipliers[i];
                Vector3 bgPos = backgroundLayers[i].position;
                bgPos.x = parallaxX;
                backgroundLayers[i].position = bgPos;
            }
        }
    }
}