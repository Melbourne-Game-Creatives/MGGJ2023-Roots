using UnityEngine;

public class MapRootSpawner : MonoBehaviour
{
    public HexGrid Map;
    public GameObject MainRootPrefab;

    [SerializeField] float timeBetweenSpawns;
    float timeToNextSpawn;


    private void Awake()
    {
        timeToNextSpawn = timeBetweenSpawns;
    }


    // Start is called before the first frame update
    void Start()
    {
        SpawnAt(0,0);
        SpawnAt(Map.chunkCountX * HexMetrics.ChunkSizeX - 1,0);
        SpawnAt(0,Map.chunkCountZ * HexMetrics.ChunkSizeZ - 1);
        SpawnAt(Map.chunkCountX * HexMetrics.ChunkSizeX - 1,Map.chunkCountZ * HexMetrics.ChunkSizeZ - 1);
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


    void SpawnAt(int x, int z)
    {
        var hexCenter = Map.transform.position + HexMetrics.GetCenter(x, z);

        GameObject newGO = Instantiate(MainRootPrefab, hexCenter, Quaternion.identity);
        newGO.transform.LookAt(Vector3.zero);
        newGO.GetComponent<RootSegment>().SetHealth(100f);
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
}
