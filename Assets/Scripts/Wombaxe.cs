using UnityEngine;

public class Wombaxe : MonoBehaviour, ISelectable
{
    [SerializeField] private GameObject selectionQuad;

    [SerializeField] private float speed;
    [SerializeField] private float attackDamage;
    [SerializeField] private float attackCooldown;

    public Vector3 TargetPos;
    private float currentCooldown;

    [Header("Health")]
    [SerializeField] private float initialHealth;
    [SerializeField] private float damageFromBranchPerSecond;
    [SerializeField] private float healingFromBurrow;
    [SerializeField] private HealthBar healthBar;
    private float health;
    private bool takeDamageThisFrame;
    private bool healThisFrame;
    private bool isMovingTowardsTarget;


    private void Start()
    {
        health = initialHealth;
        UnitSelections.Instance.unitList.Add(this);

        TargetPos = transform.position;
    }

    private void Update()
    {
        if (Vector3.Distance(this.transform.position, TargetPos) <= 0.2f)
        {
            isMovingTowardsTarget = false;
            return;
        }
        else
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, TargetPos, step);
        }
    }

    private void LateUpdate()
    {
        if (healThisFrame)
        {
            if (health < initialHealth)
            {
                SetHealth(Mathf.Min(initialHealth, health + healingFromBurrow * Time.deltaTime));
            }
            takeDamageThisFrame = false;
            
        }
        else if (takeDamageThisFrame)
        {
            TakeDamage(damageFromBranchPerSecond);
            takeDamageThisFrame = false;
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
        isMovingTowardsTarget = true;
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
            takeDamageThisFrame = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isMovingTowardsTarget && other.gameObject.CompareTag("Root"))
        {
            SetTargetPosition(other.transform.position);
        }
    }


    public void StartHealing()
    {
        healThisFrame = true;
    }


    public void StopHealing()
    {
        healThisFrame = false;
    }


    private void TakeDamage(float damage)
    {
        SetHealth(health - damage * Time.deltaTime);
        if (health == 0)
        {
            Die();
        }
    }


    private void SetHealth(float _health)
    {
        health = Mathf.Max(_health, 0);
        healthBar.UpdateBar(health / initialHealth);
    }


    private void Die()
    {
        print("Wombat dead!");
        Destroy(gameObject, 0.4f);
    }
}