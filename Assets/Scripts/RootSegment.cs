using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RootSegment : MonoBehaviour
{
    [SerializeField] private Transform growthPointTr;
    [SerializeField] private Transform[] branchPointTrs;
    [SerializeField] private PrefabCatalog rootPrefabs;
    [SerializeField] private Transform modelTr;

    [Space]

    [SerializeField] private float initialHealth;
    [SerializeField][Tooltip("Health points per second")] private float healthRate;

    [Space]

    [SerializeField] private float growthDelay;
    [SerializeField] private float maxAngle;
    [SerializeField][Tooltip("When to start targeting")] private float targetingThreshold;
    [SerializeField][Tooltip("When to aim directly")] private float directThreshold;

    private bool hasGrown = false;
    private Vector3 target;
    private float distanceToTarget;
    private float timeToGrow;
    private int generation = 1; // to be changed by parent
    private Vector3 modelInitialScale;
    private float health;

    public Transform ModelTr { get => modelTr; }


    public void init(int _generation)
    {
        generation = _generation;
        AnimateGrowth();
    }


    private void Awake()
    {
        target = Vector3.zero;
        modelInitialScale = modelTr.localScale;
        health = initialHealth;

        Reactivate();
        CheckHovelReached();
    }

    private void Start()
    {
        AffectMap(transform);
    }


    private void Update()
    {
        if (health <= 0) return;

        if (hasGrown)
        {
            UpdateHealth();
        }
        else if (timeToGrow <= 0) {
            hasGrown = true;
            CalculateDistance();
            Grow();
            Branch();
        }
        else {
            timeToGrow -= Time.deltaTime;
        }
    }


    public void SetHealth(float _health)
    {
        health = _health;
    }

    private void AffectMap(Transform tr/*, RootSegment segment*/)
    {
        var map = GameObject.Find("HexGrid").GetComponent<HexGrid>();

        var cells = new HashSet<HexCell>();
        cells.Add(map.GetCell(tr.position));
        
        // Tried to add branch point and growth point,
        // but because it isn't scaled up yet they are all the same point. Not enough time to resolve this.
        
        // foreach (var branchPoint in segment.branchPointTrs)
        // {
        //     var point = segment.transform.position;
        //     cells.Add(map.GetCell(point));
        // }
        // cells.Add(map.GetCell(segment.growthPointTr.position));
        
        foreach (var cell in cells)
        {
            AffectCell(cell);
        }
    }

    private void AffectCell(HexCell cell)
    {
        if (!cell) return;
        
        cell.TerrainTypeIndex = 3;
        if (cell.Color.r > 0.2f)
        {
            var newColor = cell.Color - new Color(0.3f, 0.3f, 0.3f);
            if (newColor.r < 0.2f)
            {
                newColor = new Color(0.2f, 0.2f, 0.2f);
            }
            cell.Color = newColor;
        }
    }
    
    public bool IsInMap(Transform tr)
    {
        return Physics.Raycast(tr.position + Vector3.up, Vector3.down, 100f, 1 << LayerMask.NameToLayer("Ground"));
    }


    public void TakeDamage(float damage)
    {
        if (health <= 0) return;

        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }


    public void Reactivate()
    {
        timeToGrow = growthDelay + Random.Range(-0.5f, 0.5f);
        hasGrown = false;
    }


    private void CalculateDistance()
    {
        if (growthPointTr == null) return;

        distanceToTarget = Vector3.Distance(target, growthPointTr.position);
    }


    private void Grow()
    {
        if (growthPointTr == null || !IsInMap(growthPointTr)) return;

        Quaternion effectiveRotation = growthPointTr.rotation;

        if (distanceToTarget > targetingThreshold)
        {
            // No targeting, instead random offset
            effectiveRotation = Quaternion.Euler(0, Random.Range(-maxAngle, maxAngle), 0) * effectiveRotation;
        }
        else if (distanceToTarget < directThreshold)
        {
           effectiveRotation = Quaternion.LookRotation(target - growthPointTr.position);
        }
        else
        {
            // Target the destination, with random movement
            Quaternion desiredRotation = Quaternion.LookRotation(target - growthPointTr.position);
            float deviationFactor = (distanceToTarget - directThreshold) / (targetingThreshold - directThreshold); // gradually from 1 at targetingThreshold to 0 at directThreshold
            effectiveRotation = Quaternion.Euler(0, Random.Range(-maxAngle, maxAngle) * deviationFactor, 0) * desiredRotation;
        }

        GameObject newSegmentGO = Instantiate(rootPrefabs.getRandom(), growthPointTr.position, effectiveRotation, growthPointTr);
        var rootSegment = newSegmentGO.GetComponent<RootSegment>();
        rootSegment.init(generation);
        
        // AffectMap(growthPointTr, rootSegment);
    }


    private void Branch()
    {
        foreach (Transform branchPointTr in branchPointTrs)
        {
            if (ShouldBranch() && IsInMap(branchPointTr))
            {
                GameObject newSegmentGO = Instantiate(rootPrefabs.getRandom(), branchPointTr.position, branchPointTr.rotation, branchPointTr);
                var rootSegment = newSegmentGO.GetComponent<RootSegment>();
                rootSegment.init(generation + 1);
                
                // AffectMap(branchPointTr, rootSegment);
            }
        }
    }


    private bool ShouldBranch()
    {
        float maxProbability = 0.3f;
        float probability;

        if (generation > 3) return false; // no branching from generation 4

        // Adjust based on distance
        if (distanceToTarget > targetingThreshold)
        {
            probability = maxProbability;
        }
        else if (distanceToTarget < directThreshold)
        {
            probability = 0;
        }
        else
        {
            probability = (distanceToTarget - directThreshold) / (targetingThreshold - directThreshold) * maxProbability; // gradually from maxProbability at targetingThreshold to 0 at directThreshold
        }

        // Adjust based on generation
        probability /= generation;

        return Random.Range(0, 1f) < probability;
    }


    private void UpdateHealth()
    {        
        health += healthRate * Time.deltaTime;
     
        if (health < initialHealth) return; // we only show them bigger, never smaller than initially
        float factor = health / initialHealth + (health - initialHealth);
        modelTr.localScale = new Vector3(modelInitialScale.x * factor, modelInitialScale.y, modelInitialScale.z * factor);
    }


    private void Die()
    {
        float fadeOutTime = 3f;
        RootSegment[] segments = GetComponentsInChildren<RootSegment>();
        foreach (RootSegment segment in segments)
        {
            segment.ModelTr.GetComponent<CapsuleCollider>().enabled = false;
            segment.enabled = false;
            segment.ModelTr.DOScale(new Vector3(0, segment.modelTr.localScale.y, 0), fadeOutTime);
        }
        ReactivateParent();
        Destroy(gameObject, fadeOutTime + 3f);
    }


    private void ReactivateParent()
    {
        if (transform.parent == null) return;
        RootSegment parentSegment = transform.parent.GetComponentInParent<RootSegment>();
        if (parentSegment != null)
        {
            parentSegment.Reactivate();
        }
    }


    private void CheckHovelReached()
    {
        CalculateDistance();
        if (distanceToTarget < 1f)
        {
            print("YOU'RE HOMELESS!");
            FindObjectOfType<Burrow>().ShowEndScreen();
        }
    }


    private void AnimateGrowth()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 3f);
    }
}
