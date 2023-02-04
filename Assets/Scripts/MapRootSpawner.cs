using UnityEngine;

public class MapRootSpawner : MonoBehaviour
{
    public HexGrid Map;
    public GameObject MainRootPrefab;

    [SerializeField] private PrefabCatalog rootPrefabs;
    [SerializeField] float timeBetweenSpawns;
    float timeToNextSpawn;


    private void Awake()
    {
        timeToNextSpawn = timeBetweenSpawns;
    }


    // Start is called before the first frame update
    void Start()
    {
        SpawnAt(0,0, MainRootPrefab);
        SpawnAt(Map.chunkCountX * HexMetrics.ChunkSizeX - 1,0, MainRootPrefab);
        SpawnAt(0,Map.chunkCountZ * HexMetrics.ChunkSizeZ - 1, MainRootPrefab);
        SpawnAt(Map.chunkCountX * HexMetrics.ChunkSizeX - 1,Map.chunkCountZ * HexMetrics.ChunkSizeZ - 1, MainRootPrefab);
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


    void SpawnAt(int x, int z, GameObject prefab)
    {
        var hexCenter = Map.transform.position + HexMetrics.GetCenter(x, z);

        GameObject newGO = Instantiate(prefab, hexCenter, Quaternion.identity);
        newGO.transform.LookAt(Vector3.zero);
    }


    private void SpawnRandom()
    {
        float randomX = Random.Range(10f, 40f);
        float randomZ = Random.Range(10f, 40f);

        if (Random.Range(0, 1f) > 0.5f) randomX *= -1;
        if (Random.Range(0, 1f) > 0.5f) randomZ *= -1;

        print($"Spawning at {randomX}, {randomZ}");

        GameObject newGO = Instantiate(rootPrefabs.getRandom(), new Vector3(randomX, 0, randomZ), Quaternion.identity);
        newGO.transform.LookAt(Vector3.zero);
    }
}
