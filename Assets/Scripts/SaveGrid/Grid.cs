using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] GameObject tile;

    [SerializeField] static int ROWS = 5;
    [SerializeField] static int COLUMNS = 5;

    [SerializeField] GameObject[] grid = new GameObject[ROWS * COLUMNS];

    static char EMPTY_SYMBOL = 'e';
    static char GRASS_SYMBOL = 'g';
    static char ROCK_SYMBOL = 'r';

    // Start is called before the first frame update
    void Awake()
    {
        EventsManager.Instance.AddListener<OnCreateGrid>(CreateGrid);
        EventsManager.Instance.AddListener<OnOpenSave>(LoadSave);
    }

    void CreateGrid(Event e)
    {
        Vector3 lTransformPosition = transform.position;

        for(int i = 0; i < ROWS; i++)
        {
            for(int j = 0; j < COLUMNS; j++)
            {
                lTransformPosition = new Vector3(i, j, 0);
                GameObject lTile = Instantiate(tile, lTransformPosition, Quaternion.identity, transform);
                grid[j * COLUMNS + i] = lTile;
            }
        }
        //TO-DO: fix this bug
        gameObject.transform.Rotate(new Vector3(0, 0, 180f), Space.Self);
        
    }

    void LoadSave(OnOpenSave e)
    {
        string lSave = e.saveLevel;

        //Lenght - 1 because of the '\n'
        if (lSave.Length - 1 != grid.Length)
        {
            Debug.LogError("THE SAVE AND THE LEVEL HAVEN'T THE SAME DIMENSIONS : LOADING FAILED");
            return;
        }

        int index = 0;

        foreach(char letter in lSave)
        {
            if(letter == '\n') continue;
            if (letter == EMPTY_SYMBOL) grid[index].GetComponent<SpriteRenderer>().color = Color.white;
            if (letter == GRASS_SYMBOL) grid[index].GetComponent<SpriteRenderer>().color = Color.green;
            if (letter == ROCK_SYMBOL) grid[index].GetComponent<SpriteRenderer>().color = Color.gray;
            index++;
        }
    }

    private void OnDestroy()
    {
        EventsManager.Instance.RemoveListener<OnCreateGrid>(CreateGrid);
    }
}
