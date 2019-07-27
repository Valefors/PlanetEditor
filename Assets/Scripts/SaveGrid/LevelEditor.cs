using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class LevelEditor : MonoBehaviour
{
    string _levelSave = "";

    // Start is called before the first frame update
    void Start()
    {
        EventsManager.Instance.Raise(new OnCreateGrid());
    }

    //Open the level save
    public void OpenSave()
    {
        string path = EditorUtility.OpenFilePanel("Choose a save", "", "txt");
        StreamReader save = new StreamReader(path);
        _levelSave = save.ReadToEnd();

        if (_levelSave != "") EventsManager.Instance.Raise(new OnOpenSave(_levelSave));
        else Debug.LogError("Level Save empty");
    }
}
