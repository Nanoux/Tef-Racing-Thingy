using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using UnityEngine.UI;

public class PickCarUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (!SteamManager.Initialized)
			return;
		GetComponentInChildren<Text> ().text = SteamFriends.GetPersonaName ();

		StartCoroutine (_FetchAvatar ());
		
	}

	int avatarInt;
	uint width, height;
	Texture2D downloadedAvater;
	Rect rect = new Rect(0,0,184,184);
	Vector2 pivot = new Vector2(0.5f,0.5f);
	IEnumerator _FetchAvatar(){
		avatarInt = SteamFriends.GetLargeFriendAvatar (SteamUser.GetSteamID());
		while (avatarInt == -1) {
			yield return null;
		}
		if (avatarInt > 0) {
			SteamUtils.GetImageSize (avatarInt, out width, out height);
			if (width > 0 && height > 0) {
				byte[] avatarStream = new byte[4*(int)width*(int)height];
				SteamUtils.GetImageRGBA (avatarInt, avatarStream, 4 * (int)width * (int)height);
				downloadedAvater = new Texture2D ((int)width, (int)height, TextureFormat.RGBA32, false);
				downloadedAvater.LoadRawTextureData (avatarStream);
				downloadedAvater.Apply();
				GetComponentInChildren<Image> ().sprite = Sprite.Create (downloadedAvater, rect, pivot);
			}
		}
	}
}
