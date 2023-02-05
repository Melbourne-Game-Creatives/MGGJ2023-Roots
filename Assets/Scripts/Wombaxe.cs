using System;
using System.Collections;
using UnityEngine;

public class Wombaxe : MonoBehaviour, ISelectable
{
    [SerializeField] private GameObject selectionQuad;

    [SerializeField] private float speed;
    [SerializeField] private float attackDamage;
    [SerializeField] private float attackCooldown;

    public Animator animator;
    private Rigidbody rb;

    public Vector3 TargetPos;
    private float currentCooldown;

    private Vector3 lastPosCheck;

    [Header("Health")]
    [SerializeField] private float initialHealth;
    [SerializeField] private float damageFromBranchPerSecond;
    [SerializeField] private float healingFromBurrow;
    [SerializeField] private HealthBar healthBar;

    [SerializeField] private AudioClip chopSound;

    private float health;
    private bool takeDamageThisFrame;
    private bool healThisFrame;
    private bool isMovingTowardsTarget;

    private IEnumerator axeStopCoroutine;

    public ParticleSystem Particle;
    public ParticleSystem HealParticle;

    private void Start()
    {
        lastPosCheck = transform.position;
        rb = GetComponent<Rigidbody>();
        health = initialHealth;
        UnitSelections.Instance.unitList.Add(this);

        TargetPos = transform.position;

        rb.sleepThreshold = 0.0f;
    }

    private void Update()
    {
        if (animator)
        {
            var delta = transform.position - lastPosCheck;
            lastPosCheck = transform.position;
            if (animator.GetBool("Moving") != delta.magnitude > 0.0001f)
            {
                animator.SetBool("Moving", delta.magnitude > 0.0001f);
            }
        }
        
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
            if (!HealParticle.isPlaying)
            {
                HealParticle.Play();
            }
        }
        else
        {
            if (HealParticle.isPlaying)
            {
                HealParticle.Stop();
            }
        }
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
        var lookAtPos = pos;
        lookAtPos.y = transform.position.y;
        transform.LookAt(lookAtPos);
        isMovingTowardsTarget = true;
    }

    private void AttackRoot(RootSegment root)
    {
        StopCoroutine("StopAxe");
        animator.SetBool("Cutting", true);
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }
        else
        {
            AudioSource.PlayClipAtPoint(chopSound, this.transform.position);
            Particle.Play();
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

    private void OnCollisionExit(Collision other)
    {
        StartCoroutine("StopAxe");
    }

    private IEnumerator StopAxe()
    {
        yield return new WaitForSeconds(0.25f);
        animator.SetBool("Cutting", false);
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


    public void Die()
    {
        FindObjectOfType<AudioExclamation>().PlayDeathSound();

        UnitSelections.Instance.unitsSelected.Remove(this);
        print("Wombat dead!");
        Destroy(gameObject, 0.4f);
    }

    public void TriggerFrenzy()
    {
        speed = 30f;
        attackDamage = 500f;
        attackCooldown = 0.25f;
        this.transform.localScale = new Vector3(2f, 2f, 2f);
    }

    public void RemoveFrenzy()
    {
        speed = 7f;
        attackDamage = 150f;
        attackCooldown = 1f;
        this.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
    }

    public void Grow()
    {
        speed *= 1.25f;
        attackDamage *= 1.25f;
        attackCooldown *= 0.75f;
        this.transform.localScale *= 1.38f;
    }
}