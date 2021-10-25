using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public string BestScoreName;
    public int BestScore;
}

class SaveDataUtils
{
    static public SaveData Load()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (!File.Exists(path))
        {
            return null;
        }

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<SaveData>(json);
    }

    static public void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(GetSaveDataPath(), json);
    }

    static string GetSaveDataPath()
    {
        return Application.persistentDataPath + "/savefile.json";
    }

}