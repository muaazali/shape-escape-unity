using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviour
{
	[SerializeField] private GameObject playerPrefab;
	[SerializeField] private Transform redSpawn, blueSpawn;

	public static GameManager Instance;

	void Awake()
	{
		Vector3 spawnPosition = PhotonNetwork.IsMasterClient ? blueSpawn.position : redSpawn.position;
		var spawnedPlayer = PhotonNetwork.Instantiate(this.playerPrefab.name, spawnPosition, Quaternion.identity, 0);
	}


}
