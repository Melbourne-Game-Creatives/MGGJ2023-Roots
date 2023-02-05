using System.Collections;
using UnityEngine;

public class MapRootSpawner : MonoBehaviour
{
    public GameObject MainRootPrefab;

    [SerializeField] float timeBetweenSpawns;
    float timeToNextSpawn;


    private void Awake()
    {
        timeToNextSpawn = timeBetweenSpawns;
    }

    private void Start()
    {
        StartCoroutine(IncreaseGrowthRateOverTime());   
    }


    private void Update()
    {
        if (timeToNextSpawn <= 0)
        {
            SpawnRandom();
            timeToNextSpawn = timeBetweenSpawns;
        }
        else
        {
            timeToNextSpawn -= Time.deltaTime;
        } 
    }


    private void SpawnRandom()
    {
        float randomX = Random.Range(10f, 40f);
        float randomZ = Random.Range(10f, 40f);

        if (Random.Range(0, 1f) > 0.5f) randomX *= -1;
        if (Random.Range(0, 1f) > 0.5f) randomZ *= -1;

        GameObject newGO = Instantiate(MainRootPrefab, new Vector3(randomX, 0, randomZ), Quaternion.identity);
        newGO.transform.LookAt(Vector3.zero);
        newGO.GetComponent<RootSegment>().SetHealth(100f);
    }

    private IEnumerator IncreaseGrowthRateOverTime()
    {
        while (true)
        {
            timeBetweenSpawns *= 0.9f;
            yield return new WaitForSeconds(15f);
        }
    }
}
