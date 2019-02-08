using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GrassRenderer : MonoBehaviour
{

	//public Mesh grassMesh;
	//public Material material;

	private Mesh mesh;
	public MeshFilter filter;

	public int seed = 0;
	public Vector2 size = new Vector2 (122f, 122f);

	[Range (1, 60000)]
	public int grassNumber = 60000;

	public float startHeight = 1000;
	public float grassOffset = 0.0f;

	// Update is called once per frame
	public void Setup ()
	{
		filter = GetComponent<MeshFilter> ();

		Random.InitState (seed);
		List<Vector3> positions = new List<Vector3> (grassNumber);
		//int[] indicies = new int[grassNumber];
		int indices = grassNumber;
		List<Color> colors = new List<Color> (grassNumber);
		List<Vector3> normals = new List<Vector3> (grassNumber);/*
		for (int i = 0; i < grassNumber; ++i) {
			Vector3 origin = transform.position;
			origin.y = startHeight;
			origin.x += size.x * Random.Range (-0.5f, 0.5f);
			origin.z += size.y * Random.Range (-0.5f, 0.5f);
			Ray ray = new Ray (origin, Vector3.down);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				origin = hit.point;
				origin.y += grassOffset;
				origin.x -= this.transform.position.x;
				origin.z -= this.transform.position.z;

				positions.Add (origin);
				indicies [i] = i;
				colors.Add (new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), 1));
				normals.Add (hit.normal);
			}
		}*/
		/*GetComponentInParent<MeshFilter> ().mesh.GetVertices (positions);
		positions = GetComponentInParent<MeshFilter> ().mesh.vertices as List<Vector3>;
		print (positions.Count);
		int[] indicies = GetComponentInParent<MeshFilter> ().mesh.GetIndices (0);
		//int[] indicies = new int[indices];
		////for (int i = 0; i < indices; i++) {
		//	indicies [i] = i;
		//}
		GetComponentInParent<MeshFilter> ().mesh.GetColors (colors);
		GetComponentInParent<MeshFilter> ().mesh.GetNormals (normals);
		mesh = new Mesh ();
		mesh.SetVertices (positions);
		mesh.SetIndices (indicies, MeshTopology.Points, 0);
		mesh.SetColors (colors);
		mesh.SetNormals (normals);
		filter.mesh = mesh;*/
		mesh = GetComponentInParent<MeshFilter> ().mesh;
		filter.mesh = GetComponentInParent<MeshFilter> ().sharedMesh;

		//Graphics.DrawMeshInstanced(grassMesh, 0, material, materices);
	}
}