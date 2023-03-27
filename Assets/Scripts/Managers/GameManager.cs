using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviour
{
	[SerializeField] private GameObject playerPrefab;
	[SerializeField] private Transform redSpawn, blueSpawn;

	[SerializeField] private List<PhotonView> redStarSets, blueStarSets;
	[SerializeField] private List<Obstacle> redObstacles, blueObstacles;

	public static GameManager Instance;
	private GameObject localPlayer;

	void Awake()
	{
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
			}
		}
		else
		{
			for (int i = 0; i < redObstacles.Count; i++)
			{
				redObstacles[i].requiredShape = (Shape)Random.Range(0, 2);
			}
		}
	}
}
