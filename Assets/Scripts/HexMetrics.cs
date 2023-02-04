using UnityEngine;

public static class HexMetrics {
	public const float OuterRadius = 0.75f;
	public const float InnerRadius = OuterRadius * 0.866025404f;
	private const float SolidFactor = 0.85f;
	private const float BlendFactor = 1f - SolidFactor;
	public const float ElevationStep = 0.1f;
	public const float EdgeDropHeight = 0.25f;
	public const int ChunkSizeX = 5;
	public const int ChunkSizeZ = 5;

	private static readonly Vector3[] Corners = {
		new (0f, 0f, OuterRadius),
		new (InnerRadius, 0f, 0.5f * OuterRadius),
		new (InnerRadius, 0f, -0.5f * OuterRadius),
		new (0f, 0f, -OuterRadius),
		new (-InnerRadius, 0f, -0.5f * OuterRadius),
		new (-InnerRadius, 0f, 0.5f * OuterRadius),
		new (0f, 0f, OuterRadius)
	};

	public static Vector3 GetCenter(int x, int z)
	{
		Vector3 position;
		position.x = (x + z * 0.5f - z / 2) * (HexMetrics.InnerRadius * 2f);
		position.y = 0f;
		position.z = z * (HexMetrics.OuterRadius * 1.5f);

		return position;
	}
	
	public static Vector3 GetFirstCorner (HexDirection direction) {
		return Corners[(int)direction];
	}

	public static Vector3 GetSecondCorner (HexDirection direction) {
		return Corners[(int)direction + 1];
	}

	public static Vector3 GetFirstSolidCorner (HexDirection direction) {
		return Corners[(int)direction] * SolidFactor;
	}

	public static Vector3 GetSecondSolidCorner (HexDirection direction) {
		return Corners[(int)direction + 1] * SolidFactor;
	}
}