using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static string filePath = Application.persistentDataPath + "/saves";
    public static string fileName = "/_01.flp";

    public static void SaveData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        SaveDataUnit data = new SaveDataUnit();

        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
        FileStream stream = new FileStream(filePath + fileName, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void LoadData()
    {
        if (File.Exists(filePath + fileName))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(filePath + fileName, FileMode.Open);

            SaveDataUnit data = formatter.Deserialize(stream) as SaveDataUnit;
            PlayerData.highestLevel = data.highestLevel;
            stream.Close();
        } else
        {
            Debug.LogError("Save file not found in " + filePath);
        }
    }
}
