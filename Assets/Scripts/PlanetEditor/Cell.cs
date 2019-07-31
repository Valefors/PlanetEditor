using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Cell : PersistantObject
{
    Vector3 position;
    Mesh mesh;
    Renderer renderer;
    Vector3[] normals;

    Vector3 _previousMousePosition = Vector3.zero;
    GameObject _prop = null;
    [SerializeField] List<GameObject> _propsList = new List<GameObject>();

    public bool isClicked = false;   //Public bool to update the custom Inspector of the script

    //2 types to detect modified values from the Editor in runtime.
    //As Unity doesn't have events to detect modified values we use this
    [HideInInspector] public Enums.CELL_TYPE type = Enums.CELL_TYPE.EMPTY;
    Enums.CELL_TYPE _originalType = Enums.CELL_TYPE.EMPTY;

    Color _color = Color.white;

    void Start()
    {
        position = transform.position;
        renderer = GetComponent<Renderer>();
        renderer.material = AssetsManager.baseMaterial;
    }

    Vector3 GetNormal()
    {
        Vector3 lNormal = Vector3.zero;

        if (mesh == null) mesh = GetComponent<MeshFilter>().mesh;
        normals = mesh.normals;

        /*for (int i = 0; i < normals.Length; i++)
        {
            print(normals[i]);
        }*/
        lNormal = Vector3.Cross(normals[0], Vector3.up);
        lNormal = normals[0];
        return lNormal;
    }

    public void AddProp(GameObject pProp)
    {
        if (_prop != null) Destroy(_prop);
        //_prop = PrefabUtility.InstantiatePrefab(pProp) as GameObject;
        _prop = pProp;
        
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

        if(_prop != null && InputManager.mouseFocusState == Enums.MOUSE_FOCUS.INGAME)
        {
            GameObject lInstantiateProp = PrefabUtility.InstantiatePrefab(_prop) as GameObject;
            lInstantiateProp.transform.parent = transform;
            lInstantiateProp.transform.up = GetNormal();
            _propsList.Add(lInstantiateProp);

            _prop = null;
        }
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

    #region Save Functions

    public override void Save(GameDataWriter writer)
    {
        writer.Write(type);
        writer.Write(_propsList.Count);

        for(int i = 0; i < _propsList.Count; i++)
        {
            writer.Write(_propsList[i]);
        }
    }

    public override void Load(GameDataReader reader)
    {
        type = reader.ReadCellType();
        int lCount = reader.ReadInt();

        LoadProps(lCount, reader);
    }

    void LoadProps(int pCount, GameDataReader pReader)
    {
        string[] prefabFiles = System.IO.Directory.GetFiles(Text.PROPS_PALETTE_PATH, Text.PREFABS_SUFIX);

        for (int i = 0; i < pCount; i++)
        {
            GameObject lSavedPrefab = pReader.ReadGameObject();

            foreach (string name in prefabFiles)
            {
                //print(name.Substring(0, 45));
                //Tricky part: as we have a name and not a file path, we add path and suffix text ("oak_tree" => "c:/.../oak_tree.prefab")
                if (name == name.Substring(0, 45) + lSavedPrefab.name + ".prefab")
                {
                    GameObject lProp = AssetDatabase.LoadAssetAtPath(name, typeof(GameObject)) as GameObject;
                    GameObject lInstantiateProp = PrefabUtility.InstantiatePrefab(lProp) as GameObject;

                    lInstantiateProp.transform.position = lSavedPrefab.transform.position;
                    lInstantiateProp.transform.rotation = lSavedPrefab.transform.rotation;
                    lInstantiateProp.transform.parent = transform;

                    _propsList.Add(lInstantiateProp);
                }
            }
        }
    }

    #endregion

    void Update()
    {
        Debug.DrawRay(position, GetNormal(), Color.red);

        if (_prop != null)
        {
            UpdateProp();
        }

        //Update values only when there is a modification from the inspector
        if ((int)type == (int)_originalType) return; 

        UpdateType();
        EventsManager.Instance.Raise(new OnCellUpdated(this));
        
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

    void UpdateProp()
    {
        Vector3 mouse = Input.mousePosition;
        if (mouse == _previousMousePosition) return;

        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hit;
        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
        {
            if (hit.transform.gameObject.GetComponent<Cell>())
            {
                _prop.transform.position = hit.point;
                position = hit.point;
                _prop.transform.up = GetNormal();
                print(GetNormal());
            } 

            else
            {
                _prop.transform.position = transform.position;
                _prop.transform.up = GetNormal();
            }
        }
        
        _previousMousePosition = mouse;
    }
}
