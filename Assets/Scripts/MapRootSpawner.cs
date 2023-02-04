using UnityEngine;

public class MapRootSpawner : MonoBehaviour
{
    public HexGrid Map;
    public GameObject RootPrefab;

    // Start is called before the first frame update
    void Start()
    {
        SpawnAt(0,0);
        SpawnAt(Map.chunkCountX * HexMetrics.ChunkSizeX - 1,0);
        SpawnAt(0,Map.chunkCountZ * HexMetrics.ChunkSizeZ - 1);
        SpawnAt(Map.chunkCountX * HexMetrics.ChunkSizeX - 1,Map.chunkCountZ * HexMetrics.ChunkSizeZ - 1);
    }

    void SpawnAt(int x, int z)
    {
        var hexCenter = Map.transform.position + HexMetrics.GetCenter(x, z);

        GameObject newGO = Instantiate(RootPrefab, hexCenter, Quaternion.identity);
        newGO.transform.LookAt(Vector3.zero);
    }
}
