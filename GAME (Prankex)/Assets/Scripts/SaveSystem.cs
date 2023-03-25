using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer (SaveLoadGamestate saveloadgamestate)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/gamestate.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        Gamestate data = new Gamestate();
        data.create(saveloadgamestate);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static Gamestate LoadGamestate()
    {
        string path = Application.persistentDataPath + "/gamestate.save";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            Gamestate data = formatter.Deserialize(stream) as Gamestate;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
