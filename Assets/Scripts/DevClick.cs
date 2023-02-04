using UnityEngine;

public class DevClick : MonoBehaviour
{
    private Camera cam;


    private void Awake()
    {
        cam = Camera.main;
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.TryGetComponent(out RootSegment segment))
                {
                    segment.TakeDamage(10f);
                }
            }
        }
    }
}
