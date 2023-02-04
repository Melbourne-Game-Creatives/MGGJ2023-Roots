using UnityEngine;

public interface ISelectable
{
    public void ShowSelection();
    public void HideSelection();
    public void SetTargetPosition(Vector3 pos);
    public GameObject GetGameObject();
}
