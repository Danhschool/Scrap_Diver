using UnityEngine;

public class TunnelSegment : MonoBehaviour
{
    [Header("Visual Settings")]
    [SerializeField] private GameObject[] wallVariants;
    [SerializeField] private bool randomizeRotation = true;
    [SerializeField] private Transform visualRoot;

    public void RandomizeVisuals()
    {
        if (wallVariants == null || wallVariants.Length == 0) return;

        foreach (var variant in wallVariants) variant.SetActive(false);

        int randomIndex = Random.Range(0, wallVariants.Length);
        wallVariants[randomIndex].SetActive(true);

        if (randomizeRotation && visualRoot != null)
        {
            float randomY = Random.Range(0, 4) * 90f;
            visualRoot.localRotation = Quaternion.Euler(0, randomY, 0);
        }
    }
}