using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
	#region Serialized Attributes
	[SerializeField] private GameObject joiningPanel, waitingPanel;
	[SerializeField] private TMP_InputField nameInput;
	#endregion

	void Awake()
	{
		PhotonNetwork.AutomaticallySyncScene = true;
	}

	void Start()
	{
		string savedPlayerName = PlayerPrefs.GetString("PlayerName", "");
		nameInput.text = savedPlayerName;
	}
	public void JoinRoom()
	{
		if (nameInput.text.Length <= 0) return;
		PhotonNetwork.NickName = nameInput.text;
		PlayerPrefs.SetString("PlayerName", nameInput.text);
		Connect();
	}

	public void Connect()
	{
		joiningPanel.SetActive(true);
		if (PhotonNetwork.IsConnected)
		{
			PhotonNetwork.JoinRandomRoom();
		}
		else
		{
			PhotonNetwork.ConnectUsingSettings();
		}
	}

	public override void OnConnectedToMaster()
	{
		Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
		PhotonNetwork.JoinRandomRoom();
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
	}

	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

		// #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
		PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
	}

	public override void OnJoinedRoom()
	{
		joiningPanel.SetActive(false);
		waitingPanel.SetActive(true);
		// SceneManager.LoadScene("LobbyScene");
		Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
		if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
		{
			waitingPanel.SetActive(false);
			Debug.Log("All players connected. Starting game.");
		}
	}

	public override void OnPlayerEnteredRoom(Player other)
	{
		if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
		{
			waitingPanel.SetActive(false);
			Debug.Log("All players connected. Starting game.");
		}
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		if (PhotonNetwork.CurrentRoom.PlayerCount < 2)
		{
			waitingPanel.SetActive(true);
			Debug.Log("Waiting for players...");
		}
	}
}
