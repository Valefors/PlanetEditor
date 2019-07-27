using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Cell : PersistantObject
{
    Mesh mesh;
    Renderer renderer;
    Vector3[] normals;

    //Public bool to update the custom Inspector of the script
    public bool isClicked = false;

    //2 types to detect modified values from the Editor in runtime.
    //As Unity doesn't have events to detect modified values we use this
    [HideInInspector] public Enums.CELL_TYPE type = Enums.CELL_TYPE.EMPTY;
    Enums.CELL_TYPE _originalType = Enums.CELL_TYPE.EMPTY;

    Color _color = Color.white;

    void Start()
    {
        //GetNormal();
        renderer = GetComponent<Renderer>();
        renderer.material = AssetsManager.baseMaterial;
    }

    void GetNormal()
    {
        if(mesh == null) mesh = GetComponent<MeshFilter>().mesh;
        normals = mesh.normals;

        for (int i = 0; i < normals.Length; i++)
        {
            print(normals[i]);
        }
    }

    #region Mouse Events Functions
    public void OnHooverMode()
    {
        if (isClicked) return;
        renderer.material = AssetsManager.hooverMaterial;
    }

    public void OnClickMode()
    {
        renderer.material = AssetsManager.selectionMaterial;
        isClicked = true;
    }

    public void OnExitMode()
    {
        if (isClicked) return;
        renderer.material.color = _color;
    }

    public void OnDeselectionMode()
    {
        renderer.material.color = _color;
        isClicked = false;
    }
    #endregion

    /// <summary>
    /// Add datas to the binary Save File
    /// </summary>
    public override void Save(GameDataWriter writer)
    {
        writer.Write(type);
    }

    public override void Load(GameDataReader reader)
    {
        type = reader.ReadCellType();
    }

    void Update()
    {
        //Update values only when there is a modification from the inspector
        if ((int)type == (int)_originalType) return; 

        UpdateType();
        EventsManager.Instance.Raise(new OnCellUpdated(this));
        //Debug.DrawRay(transform.position, normals[0], Color.red);
    }

    void UpdateType()
    {
        switch (type)
        {
            case Enums.CELL_TYPE.EMPTY:
                renderer.material.color = Color.white;
                _color = renderer.material.color;

                _originalType = Enums.CELL_TYPE.EMPTY;
                break;

            case Enums.CELL_TYPE.GRASS:
                renderer.material.color = AssetsManager.grassColor;
                _color = renderer.material.color;

                _originalType = Enums.CELL_TYPE.GRASS;
                break;

            case Enums.CELL_TYPE.ROCK:
                renderer.material.color = AssetsManager.rockColor;
                _color = renderer.material.color;

                _originalType = Enums.CELL_TYPE.ROCK;
                break;

            case Enums.CELL_TYPE.SAND:
                renderer.material.color = AssetsManager.sandColor;
                _color = renderer.material.color;

                _originalType = Enums.CELL_TYPE.SAND;
                break;

            case Enums.CELL_TYPE.WATER:
                renderer.material.color = AssetsManager.waterColor;
                _color = renderer.material.color;

                _originalType = Enums.CELL_TYPE.WATER;
                break;
        }
    }
}
