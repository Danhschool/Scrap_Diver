
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JsonDataService : IDataService
{
    private readonly string savePath = Path.Combine(Application.persistentDataPath, "gamedata.json");

    public GameData Load()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            return JsonUtility.FromJson<GameData>(json);
        }
        return new GameData();

    }

    public void Save(GameData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
    }
}
