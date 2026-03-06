using System.Collections.Generic;
using UnityEngine;

public class ShopRobotSpawner : MonoBehaviour
{
    [SerializeField] private Material silhouetteMaterial;

    public Dictionary<int, GameObject> claws = new Dictionary<int, GameObject>();
    private Dictionary<GameObject, Dictionary<Renderer, Material>> originalMaterials = new Dictionary<GameObject, Dictionary<Renderer, Material>>();

    public void SpawnRobotsInShop(RobotData[] robotList, float distance)
    {
        for (int i = 0; i < robotList.Length; i++)
        {
            SpawnRobot(i, robotList[i], distance);
        }
    }

    public void SpawnRobot(int i, RobotData charData, float distance)
    {
        if (charData.robot != null)
        {
            GameObject newRobot = Instantiate(charData.robot, transform);
            int a = i - DataManager.SelectedPlayerIndex;
            newRobot.transform.localPosition = new Vector3(a * distance, 0, 0);

            if (!claws.ContainsKey(i)) claws.Add(i, newRobot);
            newRobot.name = charData.robotName;

            CacheOriginalMaterials(newRobot);
            SetRobotSilhouette(newRobot, !charData.isUnlocked);
        }
    }

    private void CacheOriginalMaterials(GameObject robot)
    {
        if (originalMaterials.ContainsKey(robot)) return;

        var matDict = new Dictionary<Renderer, Material>();
        Renderer[] renderers = robot.GetComponentsInChildren<Renderer>(true);
        foreach (var rend in renderers)
        {
            matDict[rend] = rend.sharedMaterial;
        }
        originalMaterials[robot] = matDict;
    }

    public void SetRobotSilhouette(GameObject robot, bool isLocked)
    {
        if (robot == null || silhouetteMaterial == null || !originalMaterials.ContainsKey(robot)) return;

        Renderer[] renderers = robot.GetComponentsInChildren<Renderer>(true);
        foreach (var rend in renderers)
        {
            if (isLocked)
            {
                rend.sharedMaterial = silhouetteMaterial;
            }
            else
            {
                rend.sharedMaterial = originalMaterials[robot][rend];
            }
        }
    }
}