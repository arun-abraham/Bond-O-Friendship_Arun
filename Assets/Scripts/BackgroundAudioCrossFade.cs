﻿using UnityEngine;
using System.Collections;

public class BackgroundAudioCrossFade : MonoBehaviour {

	private BackgroundAudioCrossFade instance;
	/*private BackgroundAudioCrossFade Instance
	{
		// fill this in
	}*/
    public float fadeTime = 3.0f;
    public AudioSource oldAudio;
    public AudioSource newAudio;
    public bool fading = false;

    //private
    //private float oldFadeRate;
    //private float newFadeRate;
    //private bool fadeActive = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space) && !fading)
        {
            Debug.Log("Space Pressed");
            StartCoroutine(CrossFade(1.0f));
        }
	}

    IEnumerator CrossFade(float newVolume = 1.0f)
    {
        fading = true;
        //Calculate rates of change
        float oldVolumeRate = oldAudio.volume / fadeTime;
        float newVolumeRate = newVolume / fadeTime;
        newAudio.Play();
        Debug.Log("new audio played");
        newAudio.volume = 0.0f;

        float startOldVolume = oldAudio.volume;
        float oldAudioProgress = 0;
        float newAudioProgress = 0;
      
        while (oldAudio.volume > 0)
        {
            //lerp old audio volume to 0
            oldAudioProgress += oldVolumeRate * Time.deltaTime;
            oldAudio.volume = Mathf.Lerp(startOldVolume, 0.0f, oldAudioProgress);
            Debug.Log(oldAudio.volume);
            //lerp new audio volume to 1
            newAudioProgress += newVolumeRate * Time.deltaTime;
            newAudio.volume = Mathf.Lerp(0.0f, newVolume, newAudioProgress);
            yield return null;
        }

        oldAudio.Stop();

        if (newAudio.volume != newVolume)
        {
            ///newAudio.volume = newVolume;
        }
        
    }
}
