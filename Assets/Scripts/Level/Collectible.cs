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
			Debug.Log("PICKED UP STAR!");
		}
		Destroy(this.gameObject);
	}
}
