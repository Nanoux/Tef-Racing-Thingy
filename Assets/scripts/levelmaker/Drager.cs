using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
	#region IBeginDragHandler implementation

	Vector3 last;

	public void OnBeginDrag (PointerEventData eventData)
	{
		last = Input.mousePosition - transform.position;
		Debug.Log (last);
	}

	#endregion

	#region IDragHandler implementation

	public void OnDrag (PointerEventData eventData)
	{
		transform.parent.transform.position = Input.mousePosition - transform.localPosition - last;
		RectTransform rect = transform.parent.transform.GetComponent<RectTransform> ();
		rect.localPosition = new Vector3 (Mathf.Clamp(rect.localPosition.x,-Screen.width/2 + rect.sizeDelta.x/2,Screen.width/2 - rect.sizeDelta.x/2),Mathf.Clamp(rect.localPosition.y,-Screen.height/2+ rect.sizeDelta.y/2,Screen.height/2 - rect.sizeDelta.y/2 - GetComponent<RectTransform> ().sizeDelta.y),0f);
	}

	#endregion

	#region IEndDragHandler implementation

	public void OnEndDrag (PointerEventData eventData)
	{
		Rect screen = new Rect (0, 0, Screen.width, Screen.height);
		//RectTransform rect = transform.parent.transform.GetComponent<RectTransform> ();
		//Debug.Log (rect.sizeDelta.y);
		//Rect window = new Rect (rect.localPosition.x - rect.sizeDelta.x / 2, rect.localPosition.y - rect.sizeDelta.y / 2, rect.sizeDelta.x, rect.sizeDelta.y);
		//if (screen.
	}

	#endregion



}
