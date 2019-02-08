using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class NoiseCube : MonoBehaviour
{
	public float Scale;
	public float Interval;

	public float NoiseMagnitude;



	private void OnValidate()
	{
		List<Vector3> verts = new List<Vector3>();
		List<int> tris = new List<int>();
		List<Vector2> uvs = new List<Vector2>();

		CreatePlane(Vector3.right, Vector3.up, Vector3.back, ref verts, ref tris, ref uvs);
		CreatePlane(Vector3.up, Vector3.back, Vector3.right, ref verts, ref tris, ref uvs);
		CreatePlane(Vector3.left, Vector3.up, Vector3.forward, ref verts, ref tris, ref uvs);
		CreatePlane(Vector3.up, Vector3.forward, Vector3.left, ref verts, ref tris, ref uvs);

		CreatePlane(Vector3.back, Vector3.right, Vector3.up, ref verts, ref tris, ref uvs);
		CreatePlane(Vector3.forward, Vector3.right, Vector3.down, ref verts, ref tris, ref uvs);


		Mesh mesh = new Mesh();

		mesh.SetVertices(verts);
		mesh.SetUVs(0, uvs);
		mesh.SetTriangles(tris, 0);
		mesh.RecalculateBounds();

		GetComponent<MeshFilter>().mesh = mesh;
	}


	private void CreatePlane(Vector3 up, Vector3 right, Vector3 outward, ref List<Vector3> verts, ref List<int> tris, ref List<Vector2> uvs)
	{
		int triOffset = verts.Count;

		int length = Mathf.FloorToInt(Scale / Interval);

		for (int w = 0; w < length; w++)
		{
			for (int h = 0; h < length; h++)
			{
				Vector3 vert = 
					((w - length / 2f) * Interval * right) 
					+ ((h - length / 2f) * Interval * up) 
					+ (outward * Random.Range(0, NoiseMagnitude)) 
					+ (outward * length / 2f * Interval);

				verts.Add(vert);
				uvs.Add(new Vector2(w, h));

				if (w == 0 || h == 0)
					continue;

				int i = h * length + w + triOffset;

				tris.Add(i);
				tris.Add(i - length);
				tris.Add(i - length - 1);

				tris.Add(i);
				tris.Add(i - length - 1);
				tris.Add(i - 1);
			}
		}
	}
}
