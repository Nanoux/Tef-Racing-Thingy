using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System.Collections;
using UnityEngine.SceneManagement;

public class playamanaga : MonoBehaviour {
	
	NetworkManager netman;
	List<MatchInfoSnapshot> matchList = new List<MatchInfoSnapshot>();
	List<GameObject> roomList = new List<GameObject>();
	NetworkMatch matchy;
	bool matchCreated;
	public Text status;
	public Transform roomlist;
	public GameObject roomListItemPrefab;
	// Use this for initialization
	void Start () {
		netman = NetworkManager.singleton;
		if (netman.matchMaker == null) {
			netman.StartMatchMaker ();
		}
		RefreshRoomList ();
	}

	public void RefreshRoomList ()
	{
		ClearRoomList();

		if (netman.matchMaker == null)
		{
			netman.StartMatchMaker();
		}

		netman.matchMaker.ListMatches(0, 20, "", true, 0, 0, OnMatchList);
		status.text = "Loading...";
	}

	public void OnMatchList (bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
	{
		status.text = "";

		if (!success || matchList == null)
		{
			status.text = "Couldn't get room list.";
			return;
		}

		ClearRoomList();

		foreach (MatchInfoSnapshot match in matchList)
		{
			GameObject _roomListItemGO = Instantiate(roomListItemPrefab);
			_roomListItemGO.transform.SetParent(roomlist);

			RoomListItem _roomListItem = _roomListItemGO.GetComponent<RoomListItem>();
			if (_roomListItem != null)
			{
				_roomListItem.Setup(match, JoinRoom);
			}


			// as well as setting up a callback function that will join the game.

			roomList.Add(_roomListItemGO);
		}

		if (roomList.Count == 0)
		{
			status.text = "No rooms at the moment.";
		}
	}

	void ClearRoomList()
	{
		for (int i = 0; i < roomList.Count; i++)
		{
			Destroy(roomList[i]);
		}

		roomList.Clear();
	}

	public void JoinRoom (MatchInfoSnapshot _match)
	{
		netman.matchMaker.JoinMatch(_match.networkId, "", "", "", 0, 0, netman.OnMatchJoined);
		ClearRoomList ();
		status.text = "Joining...";
		StartCoroutine(WaitForJoin());
	}

	IEnumerator WaitForJoin ()
	{
		ClearRoomList();

		int countdown = 10;
		while (countdown > 0)
		{
			//status.text = "JOINING... (" + countdown + ")";

			yield return new WaitForSeconds(1);

			countdown--;
		}

		// Failed to connect
		//status.text = "Failed to connect.";
		yield return new WaitForSeconds(1);

		MatchInfo matchInfo = netman.matchInfo;
		if (matchInfo != null)
		{
			netman.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, netman.OnDropConnection);
			netman.StopHost();
		}

		RefreshRoomList();

	}
}
