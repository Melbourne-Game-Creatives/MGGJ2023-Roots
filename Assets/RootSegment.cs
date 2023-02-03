using UnityEngine;

public class RootSegment : MonoBehaviour
{
    [SerializeField] private Transform growthPointTr;
    [SerializeField] private Transform[] branchPointTrs;
    [SerializeField] private GameObject rootSegmentPrefab; // TODO: allow different prefabs

    [Space]

    [SerializeField] private float growthDelay;
    [SerializeField] private float maxAngle;
    [SerializeField][Tooltip("When to start targeting")] private float targetingThreshold;
    [SerializeField][Tooltip("When to aim directly")] private float directThreshold;

    private bool hasGrown = false;
    private Vector3 target;
    private float distanceToTarget = Mathf.Infinity;
    private float timeToGrow = Mathf.Infinity;
    private int generation = 1; // to be changed by parent

    private GameObject myparent;


    public void init(int _generation, GameObject theparent)
    {
        generation = _generation;
        myparent = theparent;
    }


    private void Awake()
    {
        hasGrown = false;
        target = Vector3.zero;
        timeToGrow = growthDelay;
    }


    private void Update()
    {
        if (hasGrown) return;

        if (timeToGrow <= 0) {
            hasGrown = true;
            CalculateDistance();
            Grow();
            Branch();
        }
        else {
            timeToGrow -= Time.deltaTime;
        }
    }


    private void CalculateDistance()
    {
        if (growthPointTr == null) return;

        distanceToTarget = Vector3.Distance(target, growthPointTr.position);
    }


    private void Grow()
    {
        if (growthPointTr == null) return;

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

        Debug.Log("grow: " + generation.ToString());
        GameObject newSegmentGO = Instantiate(rootSegmentPrefab, growthPointTr.position, effectiveRotation, growthPointTr);
        newSegmentGO.GetComponent<RootSegment>().init(generation, this.gameObject);
    }


    private void Branch()
    {
        foreach (Transform branchPointTr in branchPointTrs)
        {
            if (ShouldBranch())
            {
                Debug.Log("branch: " + generation.ToString());
                GameObject newSegmentGO = Instantiate(rootSegmentPrefab, branchPointTr.position, branchPointTr.rotation, branchPointTr);
                newSegmentGO.GetComponent<RootSegment>().init(generation + 1, this.gameObject);
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
}
