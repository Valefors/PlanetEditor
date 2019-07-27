using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelEditorWindow : EditorWindow
{
    string _title = "yo";
    Color _color;

    [MenuItem("Level Editor/Modify")]
    static void ShowWindow()
    {
        EditorWindow.GetWindow<LevelEditorWindow>("Edit level");
    }

    private void OnGUI()
    {
        GUILayout.Label("You can update your level", EditorStyles.boldLabel);

        _title = EditorGUILayout.TextField("Name", _title);
        _color = EditorGUILayout.ColorField("Color", _color);

        if(GUILayout.Button("SAVE ME"))
        {
            Colorize();
        }
            
    }

    void Colorize()
    {
        Debug.Log("saved!");
        foreach (GameObject go in Selection.gameObjects)
        {
            Renderer renderer = go.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.sharedMaterial.color = _color;
            }
        }
    }
}
