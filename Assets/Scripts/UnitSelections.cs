using System.Collections.Generic;
using UnityEngine;

public class UnitSelections : MonoBehaviour
{
    public List<ISelectable> unitList = new List<ISelectable>();
    public List<ISelectable> unitsSelected = new List<ISelectable>();

    private static UnitSelections _instance;
    public static UnitSelections Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void ClickSelect(ISelectable unitToAdd)
    {
        DeselectAll();
        unitsSelected.Add(unitToAdd);
        unitToAdd.ShowSelection();
    }

    public void CtrlClickSelect(ISelectable unitToAdd)
    {
        if (!unitsSelected.Contains(unitToAdd))
        {
            unitsSelected.Add(unitToAdd);
            unitToAdd.ShowSelection();
        }
        else
        {
            unitsSelected.Remove(unitToAdd);
            unitToAdd.HideSelection();
        }
    }

    public void DragSelect(ISelectable unitToAdd)
    {
        if (!unitsSelected.Contains(unitToAdd))
        {
            unitsSelected.Add(unitToAdd);
            unitToAdd.ShowSelection();
        }
    }

    public void Deselect(ISelectable unitToRemove)
    {
        unitsSelected.Remove(unitToRemove);
        unitToRemove.HideSelection();
    }

    public void DeselectAll()
    {
        foreach (ISelectable unit in unitsSelected)
        {
            unit.HideSelection();
        }
        unitsSelected.Clear();
    }
}
