using System.IO;
using UnityEngine;

public static class SaveSystem
{
    static string dataName = "data.txt";
    public static void SaveData(GameData gameData) 
    {
        var json = JsonUtility.ToJson(gameData);
        File.WriteAllText(Application.persistentDataPath + Path.DirectorySeparatorChar + dataName, json);
    }

    public static void LoadData(GameData gameData)
    {
        if (File.Exists(Application.persistentDataPath + Path.DirectorySeparatorChar + dataName))
        {
            var json = File.ReadAllText(Application.persistentDataPath + Path.DirectorySeparatorChar + dataName);
            JsonUtility.FromJsonOverwrite(json, gameData);
        }
        else
        {
            var json = JsonUtility.ToJson(gameData);
            File.WriteAllText(Application.persistentDataPath + Path.DirectorySeparatorChar + dataName, json);
        }
    }

    public static void DeleteData()
    {
        string filePath = Application.persistentDataPath + Path.DirectorySeparatorChar + dataName;
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("Data file deleted successfully.");
        }
        else
        {
            Debug.Log("No data file found to delete.");
        }
    }
}
