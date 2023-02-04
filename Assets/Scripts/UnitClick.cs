using UnityEngine;

public class UnitClick : MonoBehaviour
{
    private Camera myCam;

    public LayerMask clickable;
    public LayerMask ground;
    public AudioExclamation exclaimer;

    private void Start()
    {
        myCam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = myCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickable))
            {
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    UnitSelections.Instance.ShiftClickSelect(hit.collider.gameObject.GetComponentInChildren<ISelectable>());
                    exclaimer.PlayExclamation();
                }
                else
                {
                    UnitSelections.Instance.ClickSelect(hit.collider.gameObject.GetComponentInChildren<ISelectable>());
                    exclaimer.PlayExclamation();
                }
            }
            else
            {
                if (!Input.GetKey(KeyCode.LeftControl))
                {
                    UnitSelections.Instance.DeselectAll();
                }
            }
        } 
        else if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = myCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
            {
                foreach (ISelectable unit in UnitSelections.Instance.unitsSelected)
                {
                    unit.SetTargetPosition(hit.point);
                } 
            }
        }
    }
}
