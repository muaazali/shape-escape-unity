using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class WinPoint : MonoBehaviour
{
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.GetComponentInParent<PhotonView>().IsMine)
		{
			other.gameObject.GetComponentInParent<PlayerController>().isStopped = true;
			if (PhotonNetwork.IsMasterClient)
			{
				GameManager.Instance.hasFinished = true;
			}
			else
			{
				other.gameObject.GetComponentInParent<PhotonView>().RPC("OtherFinishedGame", RpcTarget.MasterClient, GameManager.Instance.starsCollected);
			}
		}

	}
}
