using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
	[SerializeField] private GameObject playerPrefab;
	[SerializeField] private Transform redSpawn, blueSpawn;

	[SerializeField] private List<PhotonView> redStarSets, blueStarSets;
	[SerializeField] private List<Obstacle> redObstacles, blueObstacles;

	public TextMeshProUGUI starCountUI;

	[Header("Game Finished UI")]
	[SerializeField] public GameObject gameFinishedUI;
	[SerializeField] public TextMeshProUGUI yourStatus;
	[SerializeField] public TextMeshProUGUI masterName, otherName;
	[SerializeField] public TextMeshProUGUI masterScore, otherScore;

	public bool isGameOver = false;

	public bool hasFinished = false;
	public int starsCollected = 0;

	public bool hasOtherPlayedFinished = false;
	public int otherPlayerStarsCollected = 0;

	public static GameManager Instance;
	private GameObject localPlayer;

	void Awake()
	{
		Instance = this;
		Vector3 spawnPosition = PhotonNetwork.IsMasterClient ? blueSpawn.position : redSpawn.position;
		var spawnedPlayer = PhotonNetwork.Instantiate(this.playerPrefab.name, spawnPosition, Quaternion.identity, 0);
		localPlayer = spawnedPlayer;
	}

	void Start()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			for (int i = 0; i < redStarSets.Count; i++)
			{
				if (Random.Range(0, 2) == 1)
				{
					PhotonNetwork.Destroy(blueStarSets[i]);
					PhotonNetwork.Destroy(redStarSets[i]);
				}
			};
		}

		if (PhotonNetwork.IsMasterClient)
		{
			for (int i = 0; i < blueObstacles.Count; i++)
			{
				blueObstacles[i].requiredShape = (Shape)Random.Range(0, 2);
				if (blueObstacles[i].requiredShape == Shape.CUBE)
				{
					blueObstacles[i].sphereImage.SetActive(false);
					blueObstacles[i].cubeImage.SetActive(true);
				}
				else
				{
					blueObstacles[i].sphereImage.SetActive(true);
					blueObstacles[i].cubeImage.SetActive(false);
				}
			}
		}
		else
		{
			for (int i = 0; i < redObstacles.Count; i++)
			{
				redObstacles[i].requiredShape = (Shape)Random.Range(0, 2);
				if (redObstacles[i].requiredShape == Shape.CUBE)
				{
					redObstacles[i].sphereImage.SetActive(false);
					redObstacles[i].cubeImage.SetActive(true);
				}
				else
				{
					redObstacles[i].sphereImage.SetActive(true);
					redObstacles[i].cubeImage.SetActive(false);
				}
			}
		}
	}

	void Update()
	{
		if (isGameOver) return;
		if (hasFinished && hasOtherPlayedFinished)
		{
			isGameOver = true;
			localPlayer.GetComponent<PhotonView>().RPC("FinishGame", RpcTarget.All, starsCollected, otherPlayerStarsCollected);
			Debug.Log($"GAME COMPLETE! Master: {starsCollected}, Other: {otherPlayerStarsCollected}");
		}
	}

	public void ReturnToLobby()
	{
		PhotonNetwork.LeaveRoom();
		SceneManager.LoadScene(0);
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		Debug.Log("OTHER PLAYER LEFT THE ROOM!");
	}

}
