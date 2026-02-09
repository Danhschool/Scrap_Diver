using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject explosion;
    [SerializeField] private List<GameObject> debrisPrefabs;

    [Header("Quantity")]
    public int minQuantity = 3;
    public int maxQuantity = 6;

    [Header("Force")]
    public float minForce = 5f;
    public float maxForce = 10f;

    public void OnDie()
    {
        model.SetActive(false);
        explosion.SetActive(true);

        int count = Random.Range(minQuantity, maxQuantity + 1);

        for (int i = 0; i < count; i++)
        {
            SpawnSingleItem();
        }
        StartCoroutine(GamePlayManager.instance.SpeedToZere());
        Invoke(nameof(RunGameContinueToEnd), 2f);

        StartCoroutine(DestroyRoutine(5));
    }
    public void SpawnSingleItem()
    {
        if (debrisPrefabs.Count == 0) return;

        int randomIndex = Random.Range(0, debrisPrefabs.Count);
        GameObject selectedPrefab = debrisPrefabs[randomIndex];

        Quaternion spawnRot = Quaternion.Euler(0, 0, Random.Range(0, 360f));

        GameObject obj = Instantiate(selectedPrefab, transform.position, spawnRot, transform);

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {

            float force = Random.Range(minForce, maxForce);
            Quaternion quaternion = Quaternion.Euler(Random.Range(0, 360), 0, Random.Range(0, 360));

            Vector3 vector3 = quaternion * Vector3.down;
            rb.AddForce(vector3 * force, ForceMode.Impulse);

        }
        else
        {
            Debug.LogWarning("Prefab " + obj.name + " Rigidbody2D null!");
        }
    }
    private void RunGameContinueToEnd()
    {
        GamePlayManager.instance.GameContinueToEnd();
    }
    private IEnumerator DestroyRoutine(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        Destroy(gameObject);
    }
}
