using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CurveMath {

	public static Vector3 GetPoint (Vector3[] pts, float t){
		float omt = 1f - t;
		float omt2 = omt * omt;
		float t2 = t * t;
		return  pts [0] * (omt2 * omt) +
				pts [1] * (3f * omt2 * t) +
				pts [2] * (3f * omt * t2) +
				pts [3] * (t2 * t);
	}
	static Vector3 GetTangent (Vector3[] pts, float t){
		float omt = 1f - t;
		float omt2 = omt * omt;
		float t2 = t * t;
		Vector3 tangent =   pts [0] * (-omt2) +
			pts [1] * (3f * omt2 -2f * omt) +
			pts [2] * (-3f * t2 + 2 * t) +
			pts [3] * (t2);
		return tangent.normalized;
	}
	static Vector3 GetNormal (Vector3[] pts, float t, Vector3 up){
		Vector3 tng = GetTangent (pts, t);
		Vector3 binormal = Vector3.Cross (up, tng).normalized;
		return Vector3.Cross (tng, binormal);
	}
	public static Quaternion GetOrientation (Vector3[] pts, float t, Vector3 up){
		Vector3 tng = GetTangent (pts, t);
		Vector3 nrm = GetNormal (pts, t, up);
		return Quaternion.LookRotation (tng, nrm);
	}
	public static float[] CalcLengthTable (float[] arr, Vector3[] pts){
		arr [0] = 0f;
		float totalLength = 0f;
		float offset;
		Vector3 prev = pts [0];
		for (int i = 1; i < arr.Length; i++) {
			float t = ((float)i) / (arr.Length - 1);
			Vector3 pt = GetPoint (pts, t);
			float diff = (prev - pt).magnitude;
			totalLength += diff / 2;
			arr [i] = totalLength;
			prev = pt;
		}
		offset = (int)(totalLength + 1) - totalLength;
		offset /= arr.Length-1;
		for (int i = 1; i < arr.Length; i++) {
			arr [i] += (offset * i);
			arr [i] = (int)(arr [i]);
		}
		//Debug.Log (arr [arr.Length - 1]);
		return arr;
	}
}
public struct OrientedPoint {
	public Vector3 position;
	public Quaternion rotation;

	public OrientedPoint (Vector3 position, Quaternion rotation){
		this.position = position;
		this.rotation = rotation;
	}

	public Vector3 LocalToWorld (Vector3 point){
		return position + rotation * point;
	}

	public Vector3 WorldToLocal (Vector3 point){
		return Quaternion.Inverse (rotation) * (point - position);
	}
	public Vector3 LocalToWorldDirection (Vector3 dir){
		return rotation * dir;
	}
}
public struct ExtrudeShape{
	public Vector3[] verts;
	public Vector3[] normals;
	public Vector2[] us;
	public Color32[] colors;
	public int[] lines;

	public ExtrudeShape (Mesh mesh){
		verts = mesh.vertices;
		normals = mesh.normals;
		us = mesh.uv;
		colors = mesh.colors32;
		lines = new int[verts.Length * 2];
		lines [0] = 0;
		int index = 1;
		for (int i = 1; i < lines.Length - 1; i+=2) {
			lines [i] = index;
			lines [i + 1] = index;
			index++;
		}
		lines [lines.Length - 1] = 0;
	}
}
public static class FloatArrayExtensions{
	public static float Sample(this float[] arr, float t){
		int count = arr.Length;
		if (count == 0) {
			Debug.Log ("Can't Sample an Empty Array Dumbass");
			return 0f;
		}
		if (count == 1) {
			return arr [0];
		}
		float iFloat = t * (count - 1);
		int idLower = Mathf.FloorToInt (iFloat);
		int idUpper = Mathf.FloorToInt (iFloat + 1);
		if (idUpper >= count)
			return arr [count - 1];
		if (idUpper < 0)
			return arr [0];
		return Mathf.Lerp (arr [idLower], arr [idUpper], iFloat - idLower);
	}
}
