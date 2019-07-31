using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Planet : PersistantObject
{
    public PersistantStorage storage;
    List<Cell> _cells;

    Quaternion _originalRotation = Quaternion.identity;

    public static Planet instance {
        get { return _instance; }
    }

    static Planet _instance = null;

    public delegate void OpenWindowDelegate();
    public static OpenWindowDelegate OnOpenWindow;

    private void Awake()
    {
        if (_instance != null) Destroy(_instance);
        _instance = this;

        _cells = new List<Cell>();
        FillArray();
        _originalRotation = transform.rotation;
    }

    /// <summary>
    /// Add all the children cells to an array of cells, we don't need to reinstantiate them
    /// </summary>
    void FillArray()
    {
        int count = transform.childCount;

        for(int i = 0; i < count; i++)
        {
            _cells.Add(transform.GetChild(i).GetComponent<Cell>());
        }
    }

    void OpenEditorWindow()
    {
        if (OnOpenWindow != null) OnOpenWindow();
    }

    public void SaveDatas()
    {
        storage.Save(this);
    }

    /// <summary>
    /// Save the number of cells and theirs types
    /// As they are already instantiate, we don't need to save datas like positions or rotations
    /// </summary>
    /// <param name="writer">The Binary Datas</param>
    public override void Save(GameDataWriter writer)
    {
        transform.rotation = _originalRotation; //We reset the planet's rotation to avoid false rotations during the saving

        writer.Write(_cells.Count);
        for (int i = 0; i < _cells.Count; i++)
        {
            _cells[i].Save(writer);
        }
    }

    /// <summary>
    /// Load the save file and update the tiles datas
    /// </summary>
    /// <param name="reader"></param>
    public override void Load(GameDataReader reader)
    {
        int count = reader.ReadInt();
        print(count);
        for (int i = 0; i < count; i++)
        {
            _cells[i].Load(reader);
        }
    }

    private void OnGUI()
    {
        GUIStyle lCustomStyle = new GUIStyle("button");
        lCustomStyle.fontSize = 30;

        if (GUILayout.Button(Text.OPEN_EDITOR, lCustomStyle))
        {
            if (OnOpenWindow != null) OnOpenWindow();
        }
    }
    private void OnDestroy()
    {
        _cells.Clear();
    }
}
