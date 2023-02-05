using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
	public int chunkCountX = 4;
	public int chunkCountZ = 3;

	private int _cellCountX;
	private int _cellCountZ;

	public HexCell cellPrefab;
	public HexGridChunk chunkPrefab;

	public HexCell[] cells;
	public HexGridChunk[] chunks;

	public BoxCollider boxCollider;
	
	[Range(0f, 20f)]
	public float strength = 10f;
	public float frequency = 2f;
	[Range(1, 8)]
	public int octaves = 3;
	[Range(1f, 4f)]
	public float lacunarity = 2f;
	[Range(0f, 1f)]
	public float persistence = 0.15f;
	[Range(1, 3)]
	public int dimensions = 3;
	public NoiseMethodType type = NoiseMethodType.Simplex;

	private List<(int, int)> hiddenCells = new();

	private void AddHiddenBlock(int x, int z)
	{
		hiddenCells.AddRange(new List<(int, int)>
		{
			(x - 1 + z % 2, z + 1), (x + z % 2, z + 1),
			(x - 1, z), (x, z), (x + 1, z),
			(x - 1 + z % 2, z - 1), (x + z % 2, z - 1)
		});
	}

	private void AddBigHiddenBlock(int x, int z)
	{
		hiddenCells.AddRange(new List<(int, int)>
		{
			(x - 1, z + 2), (x, z + 2), (x + 1, z + 2),
			(x - 2 + z % 2, z + 1), (x - 1 + z % 2, z + 1), (x + z  % 2, z + 1), (x + 1 + z % 2, z + 1),
			(x - 2, z), (x - 1, z), (x, z), (x + 1, z), (x + 2, z),
			(x - 2 + z % 2, z - 1), (x - 1 + z % 2, z - 1), (x + z % 2, z - 1), (x + 1 + z % 2, z - 1),
			(x - 1, z - 2), (x, z - 2), (x + 1, z - 2),
		});
	}

	public void Awake()
	{
		Vector3 position;
		var x = -chunkCountX * HexMetrics.ChunkSizeX / 2;
		var z = -chunkCountZ * HexMetrics.ChunkSizeZ / 2;
		position.x = (x + z * 0.5f - z / 2) * (HexMetrics.InnerRadius * 2f);
		position.y = 0f;
		position.z = z * (HexMetrics.OuterRadius * 1.5f);
		
		transform.position = position;
		_cellCountX = chunkCountX * HexMetrics.ChunkSizeX;
		_cellCountZ = chunkCountZ * HexMetrics.ChunkSizeZ;
		
		AddBigHiddenBlock(50, 50);

		// AddBigHiddenBlock(50, 59);

		// AddBigHiddenBlock(50, 40);

		// AddHiddenBlock(40, 40);
		// AddHiddenBlock(40, 59);
		
		boxCollider = gameObject.AddComponent<BoxCollider>();
		boxCollider.size = new Vector3(chunkCountX * HexMetrics.ChunkSizeX * 1.35f,10f,chunkCountZ * HexMetrics.ChunkSizeZ * 1.14f);
		boxCollider.center = -position - new Vector3(0, 5, 0);

		CreateChunks();
		CreateCells();
	}

	public HexCell GetCell(Vector3 position)
	{
		position = transform.InverseTransformPoint(position);
		var coordinates = HexCoordinates.FromPosition(position);
		if (coordinates.X > chunkCountX * HexMetrics.ChunkSizeX ||
		    coordinates.X > chunkCountX * HexMetrics.ChunkSizeX ||
		    coordinates.X < 0 ||
		    coordinates.Z < 0) return null;
		var index = coordinates.X + coordinates.Z * _cellCountX + coordinates.Z / 2;
		return cells[index];
	}

	private void CreateChunks()
	{
		chunks = new HexGridChunk[chunkCountX * chunkCountZ];

		for (var z = 0; z < chunkCountZ; z++)
		{
			for (var x = 0; x < chunkCountX; x++)
			{
				var chunk = chunks[x + z * chunkCountX] = Instantiate(chunkPrefab);
				chunk.transform.SetParent(transform);
				chunk.transform.localPosition = Vector3.zero;
			}
		}
	}

	private void CreateCells()
	{
		cells = new HexCell[_cellCountZ * _cellCountX];

		var noiseMethod = Noise.methods[(int)type][dimensions - 1];

		for (var z = 0; z < _cellCountZ; z++)
		{
			for (var x = 0; x < _cellCountX; x++)
			{
				if (hiddenCells.Contains((x, z))) continue;
				
				var patch = Noise.Sum(noiseMethod, new Vector3(x + 30, z - 5, 0), frequency, octaves, lacunarity, persistence);
				var patch2 = Noise.Sum(noiseMethod, new Vector3(x, z, 0), 2f, 8, 2f,0.1f).value / 4 + 1.5f;
				CreateCell(x, z, patch.value * strength, patch2);
			}
		}
	}

	private void CreateCell(int x, int z, float patch, float patch2)
	{
		Vector3 position;
		position.x = (x + z * 0.5f - z / 2) * (HexMetrics.InnerRadius * 2f);
		position.y = 0f;
		position.z = z * (HexMetrics.OuterRadius * 1.5f);

		var cellIndex = x + z * _cellCountX;
		var cell = cells[cellIndex] = Instantiate(cellPrefab);
		cell.transform.localPosition = position;
		cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);

		cell.TerrainTypeIndex = (int)math.min(math.max((patch + 4) * 0.3f, 1), 3);
		cell.Color = Color.HSVToRGB(0, 0, patch2);

		AddCellToChunk(x, z, cell);
	}

	private void AddCellToChunk(int x, int z, HexCell cell)
	{
		var chunkX = x / HexMetrics.ChunkSizeX;
		var chunkZ = z / HexMetrics.ChunkSizeZ;
		var chunk = chunks[chunkX + chunkZ * chunkCountX];

		var localX = x - chunkX * HexMetrics.ChunkSizeX;
		var localZ = z - chunkZ * HexMetrics.ChunkSizeZ;
		chunk.AddCell(localX + localZ * HexMetrics.ChunkSizeX, cell);
	}
}