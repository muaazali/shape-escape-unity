using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class Collectible : MonoBehaviour
{
	[SerializeField] private GameObject objectToRotate;

	void Update()
	{
		objectToRotate.transform.Rotate(Vector3.up * 100f * Time.deltaTime);
	}
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.GetComponentInParent<PhotonView>().IsMine)
		{
			AudioManager.Instance.PlayStarPickSound();
			GameManager.Instance.starsCollected++;
			GameManager.Instance.starCountUI.text = GameManager.Instance.starsCollected.ToString();
			other.gameObject.GetComponentInParent<PlayerController>().starPickupParticles.Play();
		}
		Destroy(this.gameObject);
	}
}
