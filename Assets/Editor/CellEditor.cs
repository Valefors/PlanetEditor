using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Cell))]
public class CellEditor : Editor
{
    /// <summary>
    /// Show new variables in the custom inspectors when we click on the cell
    /// Hide them when we unclick
    /// </summary>
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Cell script = (Cell)target;

        if (script.isClicked)
        {
            //script.color = EditorGUILayout.ColorField("Color", script.color);
            script.type = (Enums.CELL_TYPE)EditorGUILayout.EnumPopup("Tile Type", script.type);
        }
    }
}
