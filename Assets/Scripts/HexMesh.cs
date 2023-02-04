using System;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexMesh : MonoBehaviour {

	private Mesh _hexMesh;
	[NonSerialized] private List<Vector3> _vertices;
	[NonSerialized] private List<Vector3> _terrainTypes;
	[NonSerialized] private List<Color> _colors;
	[NonSerialized] private List<int> _triangles;

	// public MeshCollider meshCollider;

	public void Awake () {
		GetComponent<MeshFilter>().mesh = _hexMesh = new Mesh();
		// meshCollider = gameObject.AddComponent<MeshCollider>();
		_hexMesh.name = "Hex Mesh";
	}

	public void Clear()
	{
		_hexMesh.Clear();
		_vertices = ListPool<Vector3>.Get();
		_terrainTypes = ListPool<Vector3>.Get();
		_colors = ListPool<Color>.Get();
		_triangles = ListPool<int>.Get();
	}

	public void Apply()
	{
		_hexMesh.SetVertices(_vertices);
		ListPool<Vector3>.Add(_vertices);
		_hexMesh.SetUVs(2, _terrainTypes);
		ListPool<Vector3>.Add(_terrainTypes);
		_hexMesh.SetColors(_colors);
		ListPool<Color>.Add(_colors);
		_hexMesh.SetTriangles(_triangles, 0);
		ListPool<int>.Add(_triangles);
		_hexMesh.RecalculateNormals();
		// meshCollider.sharedMesh = _hexMesh;
	}
	
	public void AddTriangle (Vector3 v1, Vector3 v2, Vector3 v3) {
		var vertexIndex = _vertices.Count;
		_vertices.Add(v1);
		_vertices.Add(v2);
		_vertices.Add(v3);
		_triangles.Add(vertexIndex);
		_triangles.Add(vertexIndex + 1);
		_triangles.Add(vertexIndex + 2);
	}

	public void AddTriangleColor (Color color) {
		_colors.Add(color);
		_colors.Add(color);
		_colors.Add(color);
	}

	public void AddTriangleColor (Color c1, Color c2, Color c3) {
		_colors.Add(c1);
		_colors.Add(c2);
		_colors.Add(c3);
	}

	public void AddTriangleTerrainTypes(Vector3 types)
	{
		_terrainTypes.Add(types);
		_terrainTypes.Add(types);
		_terrainTypes.Add(types);
	}

	public void AddQuad (Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4) {
		var vertexIndex = _vertices.Count;
		_vertices.Add(v1);
		_vertices.Add(v2);
		_vertices.Add(v3);
		_vertices.Add(v4);
		_triangles.Add(vertexIndex);
		_triangles.Add(vertexIndex + 2);
		_triangles.Add(vertexIndex + 1);
		_triangles.Add(vertexIndex + 1);
		_triangles.Add(vertexIndex + 2);
		_triangles.Add(vertexIndex + 3);
	}

	public void AddQuadColor (Color c1, Color c2) {
		_colors.Add(c1);
		_colors.Add(c1);
		_colors.Add(c2);
		_colors.Add(c2);
	}

	public void AddQuadColor (Color c1, Color c2, Color c3, Color c4) {
		_colors.Add(c1);
		_colors.Add(c2);
		_colors.Add(c3);
		_colors.Add(c4);
	}
	
	public void AddQuadTerrainTypes (Vector3 types) {
		_terrainTypes.Add(types);
		_terrainTypes.Add(types);
		_terrainTypes.Add(types);
		_terrainTypes.Add(types);
	}
}