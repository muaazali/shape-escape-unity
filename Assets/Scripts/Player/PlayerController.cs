using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private PhotonView photonView;
	[SerializeField] private Camera playerCamera;

	[SerializeField] private Transform redCameraPosition, blueCameraPosition;

	[SerializeField] private float speed = 10f;
	[SerializeField] private float speedIncrement = 1f;

	public Shape currentShape;

	void Awake()
	{
		currentShape = Shape.SPHERE;
		if (!photonView.IsMine)
		{
			playerCamera.gameObject.SetActive(false);
		}
		else
		{
			if (PhotonNetwork.IsMasterClient)
			{
				playerCamera.transform.position = blueCameraPosition.position;
				playerCamera.transform.rotation = blueCameraPosition.rotation;
			}
			else
			{
				playerCamera.transform.position = redCameraPosition.position;
				playerCamera.transform.rotation = redCameraPosition.rotation;
			}
		}
	}

	void Update()
	{
		this.transform.position += Vector3.forward * speed * Time.deltaTime;
		speed += speedIncrement * Time.deltaTime;
	}

	[PunRPC]
	void GenerateRandom()
	{
		Debug.Log(Random.Range(0, 1000));
	}
}
