using UnityEngine;
using System.Collections;


public class SoundManager : Singleton<SoundManager> {

	public AudioClip introClip,loopClip,endClip;

	private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();
		PlayStart ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlayStart()
	{
		if (introClip) 
		{
			audioSource.loop = false;
			audioSource.clip = introClip;
			audioSource.Play ();
			Invoke ("PlayLoop", introClip.length);
		}
	}

	public void PlayLoop()
	{
		if (loopClip) 
		{
			audioSource.Stop ();
			audioSource.loop = true;
			audioSource.clip = loopClip;
			audioSource.Play ();
		}
	}

	public void PlayEnd()
	{
		if (introClip) 
		{
			audioSource.loop = false;
			audioSource.clip = endClip;
			audioSource.Play ();
		}
	}
}
