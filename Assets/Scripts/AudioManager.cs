using UnityEngine.Audio;
using System;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

	public static AudioManager instance;

	public AudioMixerGroup mixerGroup;

	public Sound[] sounds;

	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
		}

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;

			s.source.outputAudioMixerGroup = mixerGroup;
		}
	}

	public void Play(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		float desiredVolume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		if (s.isSoundtrack)
		{
			StopCoroutine(FadeOut(s));
			StartCoroutine(FadeIn(s, desiredVolume));
		}
		else
		{
			s.source.volume = desiredVolume;
		}
		
		
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Play();
	}

	IEnumerator FadeIn(Sound s, float desiredVolume)
	{
		s.source.volume = 0;
		while (s.source.volume < desiredVolume)
		{
			s.source.volume = Mathf.Lerp(s.source.volume, desiredVolume+0.2f, 0.7f*Time.deltaTime);
			yield return null;
		}
	}

	public void StopFadeIn(Sound s)
	{
		StopCoroutine(FadeIn(s,0));
	}

	public void Stop(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		if (s.isSoundtrack)
		{
			StartCoroutine(FadeOut(s));
		}
		else
		{
			s.source.volume = 0;
			s.source.Stop();
		}
		
		
	}

	IEnumerator FadeOut(Sound s)
	{
		StopCoroutine(FadeIn(s, 0.5f));
		while (s.source.volume > 0)
		{
			s.source.volume = Mathf.Lerp(s.source.volume, -0.2f, 2*Time.deltaTime);
			yield return null;
		}
		s.source.Stop();
	}

}
