using UnityEngine;

public class RootSegment : MonoBehaviour
{
    [SerializeField] private Transform growthPointTr;
    [SerializeField] private Transform[] branchPointTrs;
    [SerializeField] private PrefabCatalog prefabCatalog;
    [SerializeField] private Transform modelTr;

    [Space]

    [SerializeField] private float health;
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


    public void init(int _generation)
    {
        generation = _generation;
    }


    private void Awake()
    {
        hasGrown = false;
        target = Vector3.zero;
        timeToGrow = growthDelay;

        CheckHovelReached();
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


    public bool IsInMap(Transform tr)
    {
        return Physics.Raycast(growthPointTr.position + Vector3.up, Vector3.down, 100f, 1 << LayerMask.NameToLayer("Ground"));
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
        timeToGrow = growthDelay;
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

        GameObject newSegmentGO = Instantiate(prefabCatalog.getRandom(), growthPointTr.position, effectiveRotation, growthPointTr);
        newSegmentGO.GetComponent<RootSegment>().init(generation);
    }


    private void Branch()
    {
        foreach (Transform branchPointTr in branchPointTrs)
        {
            if (ShouldBranch() && IsInMap(branchPointTr))
            {
                GameObject newSegmentGO = Instantiate(prefabCatalog.getRandom(), branchPointTr.position, branchPointTr.rotation, branchPointTr);
                newSegmentGO.GetComponent<RootSegment>().init(generation + 1);
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
        // TODO: for now, make it a bit thicker (could be animated)
        modelTr.localScale = modelTr.localScale + new Vector3(0.1f, 0, 0.1f) * Time.deltaTime;
    }


    private void Die()
    {
        RootSegment[] segments = GetComponentsInChildren<RootSegment>();
        foreach (RootSegment segment in segments)
        {
            segment.enabled = false;
            segment.gameObject.transform.localScale = Vector3.one * 0.5f;
        }
        ReactivateParent();
    }


    private void ReactivateParent()
    {
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
        }
    }
}