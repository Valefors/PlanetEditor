using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Cell : PersistantObject
{
    Mesh mesh;
    Renderer renderer;
    Vector3[] normals;

    Vector3 _previousMousePosition = Vector3.zero;
    GameObject _prop = null;
    [SerializeField] List<GameObject> _propsList = new List<GameObject>();
    int _propListCount = 0;

    public bool isClicked = false;   //Public bool to update the custom Inspector of the script

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
        //_prop = pProp;
        _prop = PrefabUtility.InstantiatePrefab(pProp) as GameObject;
        
    }

    #region Mouse Events Functions
    public void OnHooverMode()
    {
        if (isClicked) return;
        renderer.material = AssetsManager.hooverMaterial;
    }

    public void OnClickMode()
    {
        print("click");
        renderer.material = AssetsManager.selectionMaterial;
        isClicked = true;

        if(_prop != null && InputManager.mouseFocusState == Enums.MOUSE_FOCUS.INGAME)
        {
            _propsList.Add(_prop);
            print(_propsList.Count);
            _propListCount = _propsList.Count;
            _prop.transform.parent = transform;
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
    /// <summary>
    /// Add datas to the binary Save File
    /// </summary>
    public override void Save(GameDataWriter writer)
    {
        writer.Write(type);
        writer.Write(_propListCount);

        for(int i = 0; i < _propListCount; i++)
        {
            writer.Write(_propsList[i].name);
            writer.Write(_propsList[i].transform.position);
            writer.Write(_propsList[i].transform.rotation);
        }
    }

    public override void Load(GameDataReader reader)
    {
        type = reader.ReadCellType();
        int lCount = reader.ReadInt();

        string path = "Assets/Editor Default Resources/PropsPalette";
        string[] prefabFiles = System.IO.Directory.GetFiles(path, "*.prefab");

        for (int i = 0; i < lCount; i++)
        {
            string lSavePrefabName = reader.ReadString();
            Vector3 lSavePrefabPosition = reader.ReadVector3();
            Quaternion lSavePrefabRotation = reader.ReadQuaternion();

            foreach(string name in prefabFiles)
            {
                print(name.Substring(0, 45));
                if(name == name.Substring(0, 45) + lSavePrefabName + ".prefab")
                {
                    GameObject lProp = AssetDatabase.LoadAssetAtPath(name, typeof(GameObject)) as GameObject;
                    GameObject lProp2 = PrefabUtility.InstantiatePrefab(lProp) as GameObject;
                    lProp2.transform.position = lSavePrefabPosition;
                    lProp2.transform.rotation = lSavePrefabRotation;
                    lProp2.transform.parent = transform;

                    _propsList.Add(lProp2);
                }
            }
        }

    }
    #endregion

    void Update()
    {
        //Debug.DrawRay(transform.position, GetNormal(), Color.red);

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
        print(mouse);
        if (mouse == _previousMousePosition) return;

        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hit;
        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
        {
            _prop.transform.position = hit.point;
            _prop.transform.up = GetNormal();
        }

        _previousMousePosition = mouse;
    }
}
