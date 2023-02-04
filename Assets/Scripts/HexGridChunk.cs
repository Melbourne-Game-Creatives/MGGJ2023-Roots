using UnityEngine;

public class HexGridChunk : MonoBehaviour
{
    public HexCell[] cells;

    public HexMesh terrain;
    
    public void Awake()
    {
        cells = new HexCell[HexMetrics.ChunkSizeX * HexMetrics.ChunkSizeZ];
    }

    public void AddCell(int index, HexCell cell)
    {
        cells[index] = cell;
        cell.chunk = this;
        cell.transform.SetParent(transform, false);
    }

    public void Refresh()
    {
        enabled = true;
    }

    public void LateUpdate()
    {
        enabled = false;
        Triangulate();
    }

    private void Triangulate ()
	{
		terrain.Clear();
		foreach (var cell in cells)
		{
			Triangulate(cell);
		}
		terrain.Apply();
	}

	private void Triangulate (HexCell cell) {
		for (var d = HexDirection.NE; d <= HexDirection.NW; d++) {
			if (cell)
			{
				Triangulate(d, cell, cell.TerrainTypeIndex);
			}
		}
	}

	private void Triangulate (HexDirection direction, HexCell cell, float type) {
		var center = cell.transform.localPosition;
		var v1 = center + HexMetrics.GetFirstSolidCorner(direction);
		var v2 = center + HexMetrics.GetSecondSolidCorner(direction);
		var v3 = center + HexMetrics.GetFirstCorner(direction) + new Vector3(0,-0.04f,0);
		var v4 = center + HexMetrics.GetSecondCorner(direction) + new Vector3(0,-0.04f,0);
		
		terrain.AddTriangle(center, v1, v2);
		terrain.AddTriangleColor(cell.Color);
		Vector3 types;
		types.x = types.y = types.z = type;
		terrain.AddTriangleTerrainTypes(types);
		
		terrain.AddTriangle(v2, v1, v3);
		terrain.AddTriangleColor(cell.Color);
		types.x = types.y = types.z = type;
		terrain.AddTriangleTerrainTypes(types);

		terrain.AddTriangle(v2, v3, v4);
		terrain.AddTriangleColor(cell.Color);
		types.x = types.y = types.z = type;
		terrain.AddTriangleTerrainTypes(types);
	}
}
