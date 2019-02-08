using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BezierBit : MonoBehaviour {

	public int ID;
	public int child;
	public Mesh mesh;
	public GameObject empty;
	public Shader zero, two;

	BezierBit childGO;
	ExtrudeShape exShape;
	OrientedPoint[] roadPath;
	float[] vSamples;

	Vector3 lastpos;

	int isBridge = 0;
	int lastBridge = 0;

	public void Start(){
		Setup ();
		if (SceneManager.GetActiveScene ().buildIndex != 3) {
			GetComponentInChildren<SphereCollider> ().gameObject.SetActive (false);
			GetComponentInChildren<CapsuleCollider> ().gameObject.SetActive (false);
		}
	}

	public void Update(){
		if (SceneManager.GetActiveScene ().buildIndex == 3) {
			Setup ();
		}

	}
	public void Setup(){
		if (child != -1) {
			exShape = new ExtrudeShape (mesh);
			BezierBit[] bits = GameObject.FindObjectsOfType<BezierBit> ();
			for (int i = 0; i < bits.Length; i++) {
				if (bits [i].ID == child) {
					childGO = bits [i];
				}
			}
			if (childGO == null) {
				child = -1;
				GetComponent<MeshFilter> ().mesh = null;
				return;
			}
			Vector3[] pts = new Vector3[] {
				transform.position,
				transform.position + transform.forward * transform.localScale.z,
				childGO.transform.position - childGO.transform.forward * childGO.transform.localScale.z,
				childGO.transform.position
			};
			vSamples = CurveMath.CalcLengthTable (new float[6], pts);
			roadPath = new OrientedPoint[40];
			for (int i = 0; i < roadPath.Length; i++) {
				float t = i * (1f / (float)(roadPath.Length - 1));
				roadPath [i] = new OrientedPoint (CurveMath.GetPoint(pts,t),CurveMath.GetOrientation(pts,t, Vector3.LerpUnclamped(transform.up,childGO.transform.up,t)));
			}
			Extrude (vSamples, exShape, roadPath);
		}


		if (SceneManager.GetActiveScene ().buildIndex == 3) {
			GetComponentInChildren<SphereCollider> ().transform.localScale = new Vector3 (5 / transform.localScale.x, 5 / transform.localScale.y, 5 / transform.localScale.z);
			GetComponentInChildren<CapsuleCollider> ().transform.localScale = new Vector3 (0.5f / transform.localScale.x, 1f, 0.5f / -transform.localScale.y);
		}

		if (GetComponent<MeshCollider>() == null && child > -1)
			gameObject.AddComponent<MeshCollider> ();
		if (GetComponent<MeshCollider>() != null)
			GetComponent<MeshCollider> ().sharedMesh = GetComponent<MeshFilter> ().mesh;
	}
	public void Extrude( float[] vs, ExtrudeShape shape, OrientedPoint[] path){
		int vertsInShape = shape.verts.Length;
		int segments = path.Length - 1;
		int edgeLoops = path.Length;
		int vertCount = vertsInShape * edgeLoops;
		int triCount = shape.lines.Length * segments;
		int triIndexCount = triCount * 3;

		int[] triangleIndices = new int[triIndexCount];
		Vector3[] vertices = new Vector3[vertCount];
		Vector3[] normals = new Vector3[vertCount];
		Vector2[] uvs = new Vector2[vertCount];

		for (int i = 0; i < path.Length; i++) {
			int offset = i * vertsInShape;
			for (int j = 0; j < vertsInShape; j++) {
				int id = offset + j;
				GameObject frame = Instantiate (empty, path [i].position, path [i].rotation) as GameObject;
				uvs [id] = new Vector2 (shape.us[j].x, vs.Sample((float)i / (float)path.Length));
				vertices [id] = ( frame.transform.TransformPoint(shape.verts[j]));
				if (shape.colors [j] != Color.white) {
					uvs [id] = new Vector2 (vertices[id].y + 10, vs.Sample((float)i / (float)path.Length));
					vertices [id] = new Vector3 (vertices [id].x, -10f, vertices [id].z);
				}
				vertices [id] = transform.InverseTransformPoint (vertices [id]);
				normals [id] = transform.InverseTransformPoint(frame.transform.TransformVector(shape.normals[j]));
				DestroyImmediate(frame);
			}
		}
		int ti = 0;
		for (int i = 0; i < segments; i++) {
			int offset = i * vertsInShape;
			for (int j = 0; j < shape.lines.Length; j += 2) {
				int a = offset + shape.lines [j] + vertsInShape;
				int b = offset + shape.lines [j];
				int c = offset + shape.lines [j + 1];
				int d = offset + shape.lines [j + 1] + vertsInShape;
				triangleIndices [ti] = a; ti++;
				triangleIndices [ti] = b; ti++;
				triangleIndices [ti] = c; ti++;
				triangleIndices [ti] = c; ti++;
				triangleIndices [ti] = d; ti++;
				triangleIndices [ti] = a; ti++;
			}
		}

		Mesh meshRoad = new Mesh();
		meshRoad.vertices = vertices;
		meshRoad.triangles = triangleIndices;
		meshRoad.normals = normals;
		meshRoad.uv = uvs;
		GetComponent<MeshFilter> ().mesh = meshRoad;
	}
	void OnTriggerEnter(Collider col){
		if (col.GetComponent<Bridge> () != null) {
			if (col.Equals (col.GetComponent<Bridge> ().upperCol)) {
				if (isBridge < 1) {
					isBridge = 5;
				} else {
					if (isBridge < 4)
						isBridge+=2;
				}
			}
		}
	}
	void FixedUpdate(){
		if (isBridge > 0) {
			isBridge--;
		}
		if (isBridge > 0 && lastBridge == 0) {
			GetComponent<MeshRenderer> ().material.shader = two;
		} else if (isBridge == 0 && lastBridge > 0) {
			GetComponent<MeshRenderer> ().material.shader = zero;
		}
		lastBridge = isBridge;
	}
}
