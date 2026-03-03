
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
        //if (!File.Exists(savePath))
        //{
        //    return new GameData();
        //}

        //string encryptedJson = File.ReadAllText(savePath);
        //string decryptedJson = CaesarCipherUtility.Decrypt(encryptedJson);

        //GameData loadedData = JsonUtility.FromJson<GameData>(decryptedJson);

        //if (loadedData == null)
        //{
        //    return new GameData();
        //}

        //return loadedData;
    }

    public void Save(GameData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);

        //
        //string encryptedJson = CaesarCipherUtility.Encrypt(json);

        //File.WriteAllText(savePath, encryptedJson);
    }
}
