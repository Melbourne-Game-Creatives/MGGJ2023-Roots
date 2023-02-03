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

    private bool hasGrown;
    private Vector3 target;
    private float distanceToTarget;
    private float timeToGrow;


    private void Start()
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
        timeToGrow -= Time.deltaTime;
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

        GameObject newSegment = Instantiate(rootSegmentPrefab, growthPointTr.position, effectiveRotation);
    }


    private void Branch()
    {
        foreach (Transform branchPointTr in branchPointTrs)
        {
            if (ShouldBranch())
            {
                Instantiate(rootSegmentPrefab, branchPointTr.position, branchPointTr.rotation);
            }
        }
    }


    private static bool ShouldBranch()
    {
        return Random.Range(0, 1f) < 0.1f;
    }
}
