using UnityEngine;

public class Wombaxe : MonoBehaviour, ISelectable
{
    [SerializeField] private GameObject selectionQuad;

    [SerializeField] private float speed;
    [SerializeField] private float attackDamage;
    [SerializeField] private float attackCooldown;

    public Vector3 TargetPos;
    private float currentCooldown;

    private void Start()
    {
        UnitSelections.Instance.unitList.Add(this);

        TargetPos = transform.position;
    }

    private void Update()
    {
        if (Vector3.Distance(this.transform.position, TargetPos) <= 0.2f)
        {
            return;
        }
        else
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, TargetPos, step);
        }
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

    private void AttackRoot(RootSegment root)
    {
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }
        else
        {
            root.TakeDamage(attackDamage);
            currentCooldown = attackCooldown;
        }
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Root"))
        {
            AttackRoot(collision.gameObject.GetComponentInParent<RootSegment>());
        }
    }
}