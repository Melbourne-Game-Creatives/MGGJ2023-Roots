using UnityEngine;

public class HexCell : MonoBehaviour {
	public HexCoordinates coordinates;

	private Color _colour;

	public HexGridChunk chunk;

	private int _terrainTypeIndex;

	public Color Color
	{
		get => _colour;
		set
		{
			_colour = value;
		}
	}

	public int TerrainTypeIndex
	{
		get => _terrainTypeIndex;
		set
		{
			if (_terrainTypeIndex != value)
			{
				_terrainTypeIndex = value;
				Refresh();
			}
		}
	}

	private void Refresh()
	{
		if (!chunk) return;

		chunk.Refresh();
	}
}