using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlanetInteraction : MonoBehaviour
{
    List<Cell> clickedCells = new List<Cell>();
    Cell _clickedCell = null;
    Cell _hooverCell = null;

    // Start is called before the first frame update
    void Start()
    {
        EventsManager.Instance.AddListener<OnMouseClick>(OnMouseClick);
        EventsManager.Instance.AddListener<OnMouseHoover>(OnCellHoover);
        EventsManager.Instance.AddListener<OnMouseExit>(OnCellExit);
        EventsManager.Instance.AddListener<OnCellUpdated>(OnCellUpdated);
    }

    void OnCellHoover(OnMouseHoover e)
    {
        if (!e.hooverObject.GetComponent<Cell>()) return;

        e.hooverObject.GetComponent<Cell>().OnHooverMode();
    }

    void OnCellExit(OnMouseExit e)
    {
        if (!e.exitObject.GetComponent<Cell>()) return;

        e.exitObject.GetComponent<Cell>().OnExitMode();
    }

    void OnMouseClick(OnMouseClick e)
    {
        Cell lCell = null;
        if (e.clickObject) lCell = e.clickObject.GetComponent<Cell>();

        //Did we click on an other cell or empty?
        if (lCell != null) OnCellClicked(lCell);
        else Deselection();
    }

    public void OnCellClicked(Cell pCell)
    {
        GameObject[] selectedObjects = new GameObject[1];

        //Multi-editing
        if (Input.GetKey(KeyCode.LeftShift))
        {
            //If we reclick on a selected cell that means we want to remove it
            if (CheckIfIsInClickArray(pCell))
            {
                clickedCells.Remove(pCell);
                pCell.OnDeselectionMode();
                return;
            }

            clickedCells.Add(pCell);
            pCell.OnClickMode();
            print(clickedCells.Count);
            selectedObjects[0] = clickedCells[0].gameObject;
            Selection.objects = selectedObjects;

            return;
        }

        else
        {
            Deselection();
            if (pCell != _clickedCell)
            {
                if (_clickedCell != null) _clickedCell.OnDeselectionMode();
                _clickedCell = pCell;
                _clickedCell.OnClickMode();

                selectedObjects[0] = _clickedCell.gameObject;
            }

            else
            {
                _clickedCell.OnDeselectionMode();
                _clickedCell = null;
            }
        }

        Selection.objects = selectedObjects;
    }

    void Deselection()
    {
        for (int i = 0; i < clickedCells.Count; i++)
        {
            clickedCells[i].OnDeselectionMode();
        }

        clickedCells.Clear();

        if (_clickedCell != null)
        {
            _clickedCell.OnDeselectionMode();
            _clickedCell = null;
        }
    }

    /// <summary>
    /// Update all the selected cells
    /// As Unity doesn't allow multi-editing scripts, we must first click on a cell, update it and then apply all the modifications on the others celle
    /// </summary>
    public void OnCellUpdated(OnCellUpdated e)
    {
        //Single cell updates itself
        if (!CheckIfIsInClickArray(e.cell)) return;

        for (int i = 0; i < clickedCells.Count; i++)
        {
            clickedCells[i].type = e.cell.type;
        }
    }

    bool CheckIfIsInClickArray(Cell pCell)
    {
        for (int i = 0; i < clickedCells.Count; i++)
        {
            if (pCell == clickedCells[i]) return true;
        }

        return false;
    }

    private void OnDestroy()
    {
        EventsManager.Instance.RemoveListener<OnMouseClick>(OnMouseClick);
        EventsManager.Instance.RemoveListener<OnMouseHoover>(OnCellHoover);
        EventsManager.Instance.RemoveListener<OnMouseExit>(OnCellExit);
        EventsManager.Instance.RemoveListener<OnCellUpdated>(OnCellUpdated);

        clickedCells.Clear();
        _clickedCell = null;
        _hooverCell = null;
    }
}
