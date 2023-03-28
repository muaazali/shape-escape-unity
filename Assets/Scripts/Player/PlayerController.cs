using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerController : MonoBehaviour
{
	[Header("Object References")]
	[SerializeField] private PhotonView photonView;
	[SerializeField] private Camera playerCamera;
	[SerializeField] private GameObject cubeShape, sphereShape;
	[SerializeField] private TextMeshProUGUI playerName;
	public ParticleSystem starPickupParticles, hitParticles;

	[Header("Positioning")]
	[SerializeField] private float laneSeparation = 3f;
	[SerializeField] private Transform redCameraPosition, blueCameraPosition;

	[Header("Speed Config")]
	[SerializeField] private float speed = 10f;
	[SerializeField] private float speedIncrement = 1f;
	[SerializeField] private float laneSwitchSpeed = 10f;
	[HideInInspector] public bool isStopped = false;

	[HideInInspector] public Shape currentShape = Shape.SPHERE;
	private int currentLane = 0;
	public float targetXPosition;
	public float baseXPosition;

	void Awake()
	{
		baseXPosition = targetXPosition = transform.position.x;
		if (!photonView.IsMine)
		{
			playerCamera.gameObject.SetActive(false);
			if (PhotonNetwork.IsMasterClient)
				playerName.text = PhotonNetwork.CurrentRoom.GetPlayer(2).NickName;
			else playerName.text = PhotonNetwork.CurrentRoom.GetPlayer(0, true).NickName;
		}
		else
		{
			playerName.text = PhotonNetwork.LocalPlayer.NickName;
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
		if (isStopped) return;
		if (photonView.IsMine)
		{
			this.transform.position += Vector3.forward * speed * Time.deltaTime;
			speed += speedIncrement * Time.deltaTime;

			HandleInputs();
			transform.position = Vector3.Lerp(transform.position, new Vector3(targetXPosition, transform.position.y, transform.position.z), Time.deltaTime * laneSwitchSpeed);
		}
	}

	void HandleInputs()
	{
		if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
		{
			if (currentLane <= -1) return;
			currentLane--;
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
		{
			if (currentLane >= 1) return;
			currentLane++;
		}
		if (currentLane == -1)
		{
			targetXPosition = baseXPosition - laneSeparation;
			currentShape = Shape.CUBE;
			cubeShape.SetActive(true);
			sphereShape.SetActive(false);
		}
		else if (currentLane == 0)
		{
			targetXPosition = baseXPosition;
			currentShape = Shape.SPHERE;
			cubeShape.SetActive(false);
			sphereShape.SetActive(true);
		}
		else if (currentLane == 1)
		{
			targetXPosition = baseXPosition + laneSeparation;
			currentShape = Shape.CUBE;
			cubeShape.SetActive(true);
			sphereShape.SetActive(false);
		}
	}

	[PunRPC]
	void OtherFinishedGame(int otherStars)
	{
		GameManager.Instance.otherPlayerStarsCollected = otherStars;
		GameManager.Instance.hasOtherPlayedFinished = true;
	}

	[PunRPC]
	void FinishGame(int masterStars, int otherStars)
	{
		isStopped = true;
		if ((PhotonNetwork.IsMasterClient && masterStars > otherStars) || (!PhotonNetwork.IsMasterClient && masterStars < otherStars))
		{
			GameManager.Instance.yourStatus.text = "You Won!";
		}
		else if (masterStars == otherStars)
		{
			GameManager.Instance.yourStatus.text = "Draw!";
		}
		else
		{
			GameManager.Instance.yourStatus.text = "You Lost!";
		}
		GameManager.Instance.masterName.text = PhotonNetwork.CurrentRoom.GetPlayer(0, true).NickName;
		GameManager.Instance.otherName.text = PhotonNetwork.CurrentRoom.GetPlayer(2).NickName;
		GameManager.Instance.masterScore.text = masterStars.ToString();
		GameManager.Instance.otherScore.text = otherStars.ToString();
		GameManager.Instance.gameFinishedUI.SetActive(true);
	}
}
