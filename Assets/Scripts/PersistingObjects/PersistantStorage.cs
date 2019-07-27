using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PersistantStorage : MonoBehaviour
{
    string savePath;
    string path = "C:/Users/meume/Documents/VideoGames/Unity/Projets/Experimental/Level Editor/Assets/Save/";

    void Awake()
    {
        savePath = Path.Combine(path, "saveFile");
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
}
