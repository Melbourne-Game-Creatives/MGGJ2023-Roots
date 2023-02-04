using UnityEngine;

public class Wombaxe : MonoBehaviour, ISelectable
{
    [SerializeField] private GameObject selectionQuad;

    [SerializeField] private float speed;

    public Vector3 TargetPos;

    private void Start()
    {
        UnitSelections.Instance.unitList.Add(this);

        TargetPos = transform.position;
    }

    private void Update()
    {
        Debug.Log(TargetPos.ToString());
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, TargetPos, step);
    }

    private void OnDestroy()
    {
        UnitSelections.Instance.unitList.Remove(this);
    }

    public void ShowSelection()
    {
        selectionQuad.SetActive(true);
    }

    public void HideSelection()
    {
        selectionQuad.SetActive(false);
    }

    public void SetTargetPosition(Vector3 pos)
    {
        TargetPos = pos;
    }
}