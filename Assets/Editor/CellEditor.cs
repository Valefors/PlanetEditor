using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Cell))]
public class CellEditor : Editor
{
    [SerializeField] List<GameObject> paletteProps = new List<GameObject>();
    string path = "Assets/Editor Default Resources/PropsPalette";
    int _paletteIndex = 0;
    int _previousIndex = 0;
    Cell script = null;
    /// <summary>
    /// Show new variables in the custom inspectors when we click on the cell
    /// Hide them when we unclick
    /// </summary>
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        RefreshPalette();
        script = (Cell)target;

        if (script.isClicked)
        {
            script.type = (Enums.CELL_TYPE)EditorGUILayout.EnumPopup("Tile Type", script.type);
            Palette();
        }
    }

    void RefreshPalette()
    {
        paletteProps.Clear();
        string[] prefabFiles = System.IO.Directory.GetFiles(path, "*.prefab");

        foreach (string prefabFile in prefabFiles)
            paletteProps.Add(AssetDatabase.LoadAssetAtPath(prefabFile, typeof(GameObject)) as GameObject);
    }

    void Palette()
    {
        List<GUIContent> paletteIcons = new List<GUIContent>();

        foreach(GameObject prefab in paletteProps)
        {
            Texture2D icon = AssetPreview.GetAssetPreview(prefab);
            paletteIcons.Add(new GUIContent(icon));
        }

        _paletteIndex = GUILayout.SelectionGrid(_paletteIndex, paletteIcons.ToArray(), 3);

        if (_paletteIndex == _previousIndex) return;

        InstantiatePalettePrefab();
        Debug.Log(_paletteIndex);
    }

    void InstantiatePalettePrefab()
    {
        _previousIndex = _paletteIndex;
        GameObject lPrefab = paletteProps[_previousIndex];

        script.AddProp(lPrefab);

        //Undo.RegisterCompleteObjectUndo(lGameObject, "");

        InputManager.mouseFocusState = Enums.MOUSE_FOCUS.IN_EDITOR;
    }
}
