using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public enum Shape
{
	CUBE = 0,
	SPHERE
}

public class Obstacle : MonoBehaviour
{
	public Shape requiredShape = Shape.SPHERE;

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.GetComponentInParent<PhotonView>().IsMine)
		{
			if (other.gameObject.GetComponentInParent<PlayerController>().currentShape != requiredShape)
			{
				GameManager.Instance.starsCollected -= 3;
			}
		}

	}
}
