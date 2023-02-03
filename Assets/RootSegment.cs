using UnityEngine;

public class RootSegment : MonoBehaviour
{
    [SerializeField] private Transform growthPointTr;
    [SerializeField] private Transform[] branchPointTrs;
    [SerializeField] private GameObject rootSegmentPrefab; // TODO: allow different prefabs

    private float timeToGrow;
    private bool hasGrown;
    private Vector3 target;


    private void Start()
    {
        timeToGrow = 2;
        hasGrown = false;
        target = Vector3.zero;
    }


    private void Update()
    {
        if (hasGrown) return;

        if (timeToGrow <= 0) {
            hasGrown = true;
            Grow();
            Branch();
        }
        timeToGrow -= Time.deltaTime;
    }


    private void Grow()
    {
        if (growthPointTr == null) return;
        
        float distanceToTarget = Vector3.Distance(target, growthPointTr.position);
        Quaternion effectiveRotation = growthPointTr.rotation;

        if (distanceToTarget > 100)
        {
            // No targeting, instead random offset
            effectiveRotation = Quaternion.Euler(0, Random.Range(-20f, 20f), 0) * effectiveRotation;
        }
        else
        {
            // Target the destination
            Quaternion desiredRotation = Quaternion.LookRotation(target - growthPointTr.position);
            effectiveRotation = Quaternion.RotateTowards(growthPointTr.rotation, desiredRotation, 10f);
            // FUTURE: increase angle as we approach it?
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
