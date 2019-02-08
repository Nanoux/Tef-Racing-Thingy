using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EditorUI : MonoBehaviour {

	public GameObject[] categories;
	public GameObject[] Objects;
	public GameObject[] Primitives;
	public GameObject[] Props;
	public GameObject[] Course;
	public GameObject[] Roads;
	public GameObject[] cursors;

	public GameObject modes;

	public GameObject selected;

	public GameObject colorPicker;

	public LayerMask notgizmo;
	public LayerMask gizmos;
	public LayerMask gizmoarms;

	bool gizmoing = false;
	GameObject Gizmoarm;
	Vector3 gizmooffset;
	Vector3 lastscale;

	float angle;
	Vector3 cross;

	bool isLocal = false;

	GameObject cursor;

	// Use this for initialization
	void Start () {
		lastscale = new Vector3 (224422f, 224422f, 224422f);
		cursor = (GameObject)Instantiate (cursors [0], Vector3.zero, Quaternion.identity, null);
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<RectTransform> ().sizeDelta = new Vector2 (Screen.width / 10 * 2, GetComponent<RectTransform> ().sizeDelta.y);
		GetComponentInChildren<VerticalLayoutGroup> ().GetComponent<RectTransform> ().sizeDelta = GetComponent<RectTransform> ().sizeDelta;

		if (Input.GetKeyDown(KeyCode.Alpha1)&& !EventSystem.current.IsPointerOverGameObject()){
			modes.GetComponent<Dropdown> ().value = 0;
		}
		if (Input.GetKeyDown(KeyCode.Alpha2)&& !EventSystem.current.IsPointerOverGameObject()){
			modes.GetComponent<Dropdown> ().value = 1;
		}
		if (Input.GetKeyDown(KeyCode.Alpha3)&& !EventSystem.current.IsPointerOverGameObject()){
			modes.GetComponent<Dropdown> ().value = 2;
		}
		if (Input.GetKeyDown(KeyCode.Alpha4)&& !EventSystem.current.IsPointerOverGameObject()){
			modes.GetComponent<Dropdown> ().value = 3;
		}
		if (Input.GetKeyDown (KeyCode.F)) {
			isLocal = !isLocal;
		}

		if (Input.GetMouseButtonDown (0) && !EventSystem.current.IsPointerOverGameObject()) {
			if (selected == null) {
				select ();
			} else {
				Ray ray = GameObject.Find ("Cam").GetComponentInChildren<Camera> ().ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit, Mathf.Infinity, gizmos,QueryTriggerInteraction.Ignore)) {
					hit.collider.gameObject.GetComponentInChildren<BoxCollider> ().enabled = true;
					Gizmoarm = hit.collider.gameObject.GetComponentInChildren<BoxCollider> ().gameObject;
					gizmoing = true;
					gizmooffset = hit.point - cursor.transform.position;
				} else {
					select ();
				}
			}
		}
		if (selected != null) {
			if (modes.GetComponent<Dropdown> ().value == 2)
				cursor.transform.rotation = selected.transform.rotation;
		}
			
		if (gizmoing) {
			if (Gizmoarm.GetComponent<gizmoarm> ().id == 1) {
				Ray ray = GameObject.Find ("Cam").GetComponentInChildren<Camera> ().ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit, Mathf.Infinity, gizmoarms,QueryTriggerInteraction.Ignore)) {
					if (lastscale.x != 224422f) {
						selected.transform.localPosition += cursor.transform.TransformVector( new Vector3 (0f, (cursor.transform.InverseTransformPoint (hit.point).y - cursor.transform.InverseTransformPoint (lastscale).y), 0f));
					}
					lastscale = hit.point;
				}
			} else if (Gizmoarm.GetComponent<gizmoarm> ().id == 2) {
				Ray ray = GameObject.Find ("Cam").GetComponentInChildren<Camera> ().ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit, Mathf.Infinity, gizmoarms,QueryTriggerInteraction.Ignore)) {
					if (lastscale.x != 224422f) {
						selected.transform.localPosition += cursor.transform.TransformVector( new Vector3 (0f, 0f, (cursor.transform.InverseTransformPoint (hit.point).z - cursor.transform.InverseTransformPoint (lastscale).z)));
					}
					lastscale = hit.point;
				}
			} else if (Gizmoarm.GetComponent<gizmoarm> ().id == 3) {
				Ray ray = GameObject.Find ("Cam").GetComponentInChildren<Camera> ().ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit, Mathf.Infinity, gizmoarms,QueryTriggerInteraction.Ignore)) {
					if (lastscale.x != 224422f) {
						selected.transform.localPosition += cursor.transform.TransformVector( new Vector3 ((cursor.transform.InverseTransformPoint (hit.point).x - cursor.transform.InverseTransformPoint (lastscale).x), 0f, 0f));
					}
					lastscale = hit.point;
				}
			} else if (Gizmoarm.GetComponent<gizmoarm> ().id == 4) {
				Ray ray = GameObject.Find ("Cam").GetComponentInChildren<Camera> ().ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit, Mathf.Infinity, gizmoarms,QueryTriggerInteraction.Ignore)) {
					angle = Vector3.Angle (cursor.transform.InverseTransformVector(gizmooffset), cursor.transform.InverseTransformVector(hit.point - cursor.transform.position));
					cross = Vector3.Cross (cursor.transform.InverseTransformVector(gizmooffset), cursor.transform.InverseTransformVector(hit.point - cursor.transform.position));
					if (cross.y < 0)
						angle = -angle;
					if (isLocal) {
						selected.transform.Rotate (new Vector3 (0f, (angle), 0f));
					} else {
						selected.transform.Rotate (new Vector3 (0f, (angle), 0f), Space.World);
					}
					gizmooffset = hit.point - cursor.transform.position;
				}
			} else if (Gizmoarm.GetComponent<gizmoarm> ().id == 5) {
				Ray ray = GameObject.Find ("Cam").GetComponentInChildren<Camera> ().ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit, Mathf.Infinity, gizmoarms,QueryTriggerInteraction.Ignore)) {
					angle = Vector3.Angle (cursor.transform.InverseTransformVector(gizmooffset), cursor.transform.InverseTransformVector(hit.point - cursor.transform.position));
					cross = Vector3.Cross (cursor.transform.InverseTransformVector(gizmooffset), cursor.transform.InverseTransformVector(hit.point - cursor.transform.position));
					if (cross.z < 0)
						angle = -angle;
					if (isLocal) {
						selected.transform.Rotate (new Vector3 (0f, 0f, angle));
					} else {
						selected.transform.Rotate (new Vector3 (0f, 0f, angle), Space.World);
					}
					gizmooffset = hit.point - cursor.transform.position;
				}
			} else if (Gizmoarm.GetComponent<gizmoarm> ().id == 6) {
				Ray ray = GameObject.Find ("Cam").GetComponentInChildren<Camera> ().ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit, Mathf.Infinity, gizmoarms,QueryTriggerInteraction.Ignore)) {
					angle = Vector3.Angle (cursor.transform.InverseTransformVector(gizmooffset), cursor.transform.InverseTransformVector(hit.point - cursor.transform.position));
					cross = Vector3.Cross (cursor.transform.InverseTransformVector(gizmooffset), cursor.transform.InverseTransformVector(hit.point - cursor.transform.position));
					if (cross.x < 0)
						angle = -angle;
					if (isLocal) {
						selected.transform.Rotate (new Vector3 (angle, 0f, 0f));
					} else {
						selected.transform.Rotate (new Vector3 (angle, 0f, 0f), Space.World);
					}
					gizmooffset = hit.point - cursor.transform.position;
				}
			} else if (Gizmoarm.GetComponent<gizmoarm> ().id == 7) {
				Ray ray = GameObject.Find ("Cam").GetComponentInChildren<Camera> ().ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit, Mathf.Infinity, gizmoarms,QueryTriggerInteraction.Ignore)) {
					if (lastscale.x != 224422f) {
						if (Input.GetKey (KeyCode.LeftControl)) {
							selected.transform.localScale = new Vector3 (selected.transform.localScale.x + (cursor.transform.InverseTransformPoint (hit.point).y - lastscale.y) * 15f, selected.transform.localScale.y + (cursor.transform.InverseTransformPoint (hit.point).y - lastscale.y) * 15f, selected.transform.localScale.z + (cursor.transform.InverseTransformPoint (hit.point).y - lastscale.y) * 15f);
						} else {
							selected.transform.localScale = new Vector3 (selected.transform.localScale.x, selected.transform.localScale.y + (cursor.transform.InverseTransformPoint (hit.point).y - lastscale.y) * 15f, selected.transform.localScale.z);
						}
					}
					lastscale = cursor.transform.InverseTransformPoint(hit.point);
				}
			}else if (Gizmoarm.GetComponent<gizmoarm> ().id == 8) {
				Ray ray = GameObject.Find ("Cam").GetComponentInChildren<Camera> ().ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit, Mathf.Infinity, gizmoarms,QueryTriggerInteraction.Ignore)) {
					if (lastscale.x != 224422f) {
						if (Input.GetKey (KeyCode.LeftControl)) {
							selected.transform.localScale = new Vector3 (selected.transform.localScale.x + (cursor.transform.InverseTransformPoint (hit.point).z - lastscale.z) * 15f, selected.transform.localScale.y + (cursor.transform.InverseTransformPoint (hit.point).z - lastscale.z) * 15f, selected.transform.localScale.z + (cursor.transform.InverseTransformPoint (hit.point).z - lastscale.z) * 15f);
						} else {
							selected.transform.localScale = new Vector3 (selected.transform.localScale.x, selected.transform.localScale.y, selected.transform.localScale.z+ (cursor.transform.InverseTransformPoint (hit.point).z - lastscale.z) * 15f);
						}
					}
					lastscale = cursor.transform.InverseTransformPoint(hit.point);
				}
			}else if (Gizmoarm.GetComponent<gizmoarm> ().id == 9) {
				Ray ray = GameObject.Find ("Cam").GetComponentInChildren<Camera> ().ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit, Mathf.Infinity, gizmoarms,QueryTriggerInteraction.Ignore)) {
					if (lastscale.x != 224422f) {
						if (Input.GetKey (KeyCode.LeftControl)) {
							selected.transform.localScale = new Vector3 (selected.transform.localScale.x + (cursor.transform.InverseTransformPoint (hit.point).x - lastscale.x) * 15f, selected.transform.localScale.y + (cursor.transform.InverseTransformPoint (hit.point).x - lastscale.x) * 15f, selected.transform.localScale.z + (cursor.transform.InverseTransformPoint (hit.point).x - lastscale.x) * 15f);
						} else {
							selected.transform.localScale = new Vector3 (selected.transform.localScale.x+ (cursor.transform.InverseTransformPoint (hit.point).x - lastscale.x) * 15f, selected.transform.localScale.y, selected.transform.localScale.z);
						}
					}
					lastscale = cursor.transform.InverseTransformPoint(hit.point);
				}
			}
		}if (selected != null) {
			if (Input.GetKeyDown (KeyCode.Delete)) {
				Destroy (selected);
				selected = null;
				if (gizmoing) {
					Gizmoarm.GetComponent<BoxCollider> ().enabled = false;
					Gizmoarm = null;
					gizmoing = false;
					lastscale = new Vector3 (224422f, 224422f, 224422f);
				}
			}
		}
		if (cursor != null && selected == null) {
			cursor.transform.position = new Vector3 (224422f, 224422f, 224422f);
			if (gizmoing) {
				Gizmoarm.GetComponent<BoxCollider> ().enabled = false;
				Gizmoarm = null;
				gizmoing = false;
				lastscale = new Vector3 (224422f, 224422f, 224422f);
			}
		} else if (cursor != null) {
			cursor.transform.position = selected.transform.position;
			cursor.transform.localScale = new Vector3 (1f,1f,1f) * Vector3.Distance (cursor.transform.position, GameObject.Find ("Cam").GetComponentInChildren<Camera> ().transform.position);
			if (isLocal) {
				cursor.transform.rotation = selected.transform.rotation;
			} else {
				if (modes.GetComponent<Dropdown> ().value < 2)
					cursor.transform.eulerAngles = Vector3.zero;
			}
		}
		if (Input.GetMouseButtonUp(0)){
			if (gizmoing) {
				Gizmoarm.GetComponent<BoxCollider> ().enabled = false;
				Gizmoarm = null;
				gizmoing = false;
				lastscale = new Vector3 (224422f, 224422f, 224422f);
			}
		}
		if (Input.GetKey (KeyCode.LeftControl) && Input.GetKeyDown (KeyCode.D)) {
			if (selected.GetComponent<FinishLine> () == null) {
				if (selected.GetComponent<BezierBit> () != null) {
					if (selected.GetComponent<BezierBit> ().child == -1) {
						int index = -1;
						BezierBit[] bits = GameObject.FindObjectsOfType<BezierBit> ();
						for (int i = 0; i < bits.Length; i++) {
							if (bits [i].ID > index)
								index = bits [i].ID;
						}
						index++;
						selected.GetComponent<BezierBit> ().child = index;
						selected = Instantiate (selected) as GameObject;
						selected.GetComponent<BezierBit> ().ID = index;
						selected.GetComponent<BezierBit> ().child = -1;
					}
				} else {
					Instantiate (selected);
				}
			}
		}
		if (modes.GetComponent<Dropdown> ().value == 3 && selected != null) {
			colorPicker.SetActive (true);
			if (selected != null)
				selected.GetComponent<MeshRenderer> ().material.color = colorPicker.GetComponentInChildren<CUIColorPicker> ().resultCol;
		} else {
			colorPicker.SetActive (false);
		}
	}

	public void select(){
		Ray ray = GameObject.Find ("Cam").GetComponentInChildren<Camera> ().ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, Mathf.Infinity, notgizmo,QueryTriggerInteraction.Ignore)) {
			if (hit.collider.gameObject.GetComponentInParent<BezierBit> () != null)
				selected = hit.collider.transform.parent.gameObject;
			else
				selected = hit.collider.gameObject;
			if(modes.GetComponent<Dropdown> ().value == 3)
				colorPicker.GetComponentInChildren<CUIColorPicker> ().Color = selected.GetComponent<MeshRenderer> ().material.color;
		} else {
			selected = null;
		}
	}

	public void Switch(int hi){
		for (int i = 0; i < categories.Length; i++) {
			categories [i].SetActive (false);
		}
		categories [hi].SetActive (true);
	}
	public void SwitchObj(int hi){
		for (int i = 0; i < Objects.Length; i++) {
			Objects [i].SetActive (false);
		}
		Objects [hi].SetActive (true);
	}
	public void spawnprimitive(int hey){
		selected = Instantiate (Primitives [hey], GameObject.Find ("Cam").transform.position, Quaternion.identity, null) as GameObject;
	}
	public void spawncourse(int hey){
		if (hey == 1) {
			if (GameObject.FindObjectOfType<FinishLine>() == null)
				selected = Instantiate (Course [hey], GameObject.Find ("Cam").transform.position, Quaternion.identity, null) as GameObject;
		} else {
			selected = Instantiate (Course [hey], GameObject.Find ("Cam").transform.position, Quaternion.identity, null) as GameObject;
		}
	}
	public void spawnroad(int hey){
		int index = -1;
		BezierBit[] bits = GameObject.FindObjectsOfType<BezierBit> ();
		for (int i = 0; i < bits.Length; i++) {
			if (bits [i].ID > index)
				index = bits [i].ID;
		}
		index++;
		selected = Instantiate (Roads [hey], GameObject.Find ("Cam").transform.position, Quaternion.identity, null) as GameObject;
		selected.GetComponent<BezierBit> ().ID = index;
	}
	public void changecursor(int hi){
		if (cursor != null)
			Destroy (cursor);
		if (hi < 3)
			cursor = (GameObject)Instantiate (cursors [hi], Vector3.zero, Quaternion.identity, null);
		else if (selected != null)
			colorPicker.GetComponentInChildren<CUIColorPicker> ().Color = selected.GetComponent<MeshRenderer> ().material.color;
	}
}
