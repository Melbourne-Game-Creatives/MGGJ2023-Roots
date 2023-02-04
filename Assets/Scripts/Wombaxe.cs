using UnityEngine;

public class Wombaxe : MonoBehaviour
{
    private void Start()
    {
        UnitSelections.Instance.unitList.Add(this.gameObject);
    }

    private void OnDestroy()
    {
        UnitSelections.Instance.unitList.Remove(this.gameObject);
    }


    public void MoveTowards(Vector3 targetPoint)
    {

    }
}