using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Cell))]
public class CellEditor : Editor
{
    [SerializeField] List<GameObject> paletteProps = new List<GameObject>();
    string path = Text.PROPS_PALETTE_PATH;

    int _paletteIndex = 0;
    int _previousIndex = 0;

    Cell script = null;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        RefreshPalette();
        script = (Cell)target;

        // Show new variables in the custom inspectors when we click on the cell
        if (script.isClicked)
        {
            script.type = (Enums.CELL_TYPE)EditorGUILayout.EnumPopup("Tile Type", script.type);
            DisplayPalette();
        }
    }

    void RefreshPalette()
    {
        paletteProps.Clear();
        string lPath = path;
        script = (Cell)target;

        switch (script.type)
        {
            case Enums.CELL_TYPE.EMPTY:
                break;

            case Enums.CELL_TYPE.GRASS:
                lPath = path + "/GRASS";
                break;

            case Enums.CELL_TYPE.ROCK:
                lPath = path + "/ROCK";
                break;

            case Enums.CELL_TYPE.SAND:
                lPath = path + "/SAND";
                break;
        }

        string[] prefabFiles = System.IO.Directory.GetFiles(lPath, "*.prefab");

        //We add all the prefabs in an array to display them on inspector
        foreach (string prefabFile in prefabFiles)
            paletteProps.Add(AssetDatabase.LoadAssetAtPath(prefabFile, typeof(GameObject)) as GameObject);
    }

    void DisplayPalette()
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
    }

    void InstantiatePalettePrefab()
    {
        _previousIndex = _paletteIndex;
        GameObject lPrefab = paletteProps[_previousIndex];

        script.AddProp(lPrefab);

        InputManager.mouseFocusState = Enums.MOUSE_FOCUS.IN_EDITOR;
    }
}
