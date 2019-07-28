using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PersistantStorage : MonoBehaviour
{
    static string saveName = "saveFile";
    static string savePath;
    static string path = "C:/Users/meume/Documents/VideoGames/Unity/Projets/Experimental/PlanetEditor/Assets/Save/";

    void Awake()
    {
        savePath = Path.Combine(path, saveName);
    }

    public void Save(PersistantObject pObject)
    {
        using(BinaryWriter writer = new BinaryWriter(File.Open(savePath, FileMode.Create)))
        {
            pObject.Save(new GameDataWriter(writer));
        }
    }

    public void Load(PersistantObject pObject)
    {
        using (BinaryReader reader = new BinaryReader(File.Open(savePath, FileMode.Open)))
        {
            pObject.Load(new GameDataReader(reader));
        }
    }

    public void Load(string pPath, PersistantObject pObject)
    {
        using (BinaryReader reader = new BinaryReader(File.Open(pPath, FileMode.Open)))
        {
            pObject.Load(new GameDataReader(reader));
        }
    }

    public static void UpdateSaveName(string pName)
    {
        if (pName.Length <= 1)
        {
            Debug.LogError("NO NAME ENTERED. SAVE FAILED.");
            return;
        }

        saveName = pName;
        savePath = Path.Combine(path, saveName);
    }
}
