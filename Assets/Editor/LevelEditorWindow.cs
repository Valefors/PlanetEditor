using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelEditorWindow : EditorWindow
{
    string _titleFile = "LEVEL_01";
    Planet _planet;
    PersistantStorage _storage;
    string _feedBack = "";
    //Color _color;

    //int _selGridInt = 0;
    string[] _buttonsNamesArray = { "Save Level", "Load Level", "Save new Level" };

    [InitializeOnLoadMethod]
    static void Init()
    {
        Planet.OnOpenWindow += ShowWindow;
    }

    [MenuItem("Level Editor/Save Level")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<LevelEditorWindow>("Edit level");
    }

    private void OnGUI()
    {
        GUILayout.Space(10);

        GUILayout.Label("Save your level here.", EditorStyles.boldLabel);

        GUILayout.Space(10);

        _titleFile = EditorGUILayout.TextField("Name", _titleFile);

        GUILayout.Space(10);
        //_selGridInt = GUILayout.SelectionGrid(_selGridInt, _buttonsNamesArray, 3);
        //_color = EditorGUILayout.ColorField("Color", _color);

        GUILayout.BeginHorizontal("Box");
        if (GUILayout.Button("Save Level"))
        {
            SaveFile();
            _feedBack = "Level Saved!";
        }

        GUILayout.Space(15);

        if (GUILayout.Button("Load Level"))
        {
            LoadFile();
            _feedBack = "Level Loaded!";
        }

        GUILayout.Space(15);

        if (GUILayout.Button("New Level"))
        {
            if (Planet.instance != null) Planet.instance.SaveDatas();
        }

        GUILayout.EndHorizontal();

        GUILayout.Space(15);

        GUILayout.Label(_feedBack, EditorStyles.boldLabel);
    }

    void SaveFile()
    {
        if (Planet.instance != null)
        {
            PersistantStorage.UpdateSaveName(_titleFile);
            Planet.instance.SaveDatas();
        }
    }

    void LoadFile()
    {
        string path = EditorUtility.OpenFilePanel("Choose a save", "", "");
        Planet.instance.storage.Load(path, Planet.instance);
    }
}
