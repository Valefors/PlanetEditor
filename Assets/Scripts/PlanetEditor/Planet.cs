using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Planet : PersistantObject
{
    [SerializeField] PersistantStorage _storage;
    List<Cell> _cells;

    public KeyCode saveKey = KeyCode.S;
    public KeyCode loadKey = KeyCode.L;

    private void Awake()
    {
        _cells = new List<Cell>();
        FillArray();
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

    void Update()
    {
        if (Input.GetKeyDown(saveKey))
        {
            _storage.Save(this);
        }
        else if (Input.GetKeyDown(loadKey))
        {
            _storage.Load(this);
        }
    }

    /// <summary>
    /// Save the number of cells and theirs types
    /// As they are already instantiate, we don't need to save datas like positions or rotations
    /// </summary>
    /// <param name="writer">The Binary Datas</param>
    public override void Save(GameDataWriter writer)
    {
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

    private void OnDestroy()
    {
        _cells.Clear();
    }
}
