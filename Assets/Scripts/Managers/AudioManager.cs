using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	[Header("Sources")]
	[SerializeField] private AudioSource uiSource;
	[SerializeField] private AudioSource gameSource;

	[Header("Clips")]
	[SerializeField] private AudioClip buttonClick;
	[SerializeField] private AudioClip starPick;
	[SerializeField] private AudioClip wallHit;
	[SerializeField] private AudioClip gameFinished;

	public static AudioManager Instance = null;

	void Awake()
	{
		if (Instance != null)
		{
			DestroyImmediate(this.gameObject);
			return;
		}
		DontDestroyOnLoad(this.gameObject);
		Instance = this;
	}

	public void PlayButtonClickSound()
	{
		uiSource.clip = buttonClick;
		uiSource.Play();
	}

	public void PlayStarPickSound()
	{
		gameSource.clip = starPick;
		gameSource.Play();
	}

	public void PlayWallHitSound()
	{
		gameSource.clip = wallHit;
		gameSource.Play();
	}

	public void PlayGameFinishedSound()
	{
		uiSource.clip = gameFinished;
		uiSource.Play();
	}
}
