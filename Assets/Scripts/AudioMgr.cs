using System;
using UnityEngine.Assertions;
using UnityEngine;

public class AudioMgr : PersistentSingleton<AudioMgr>
{
	[System.Serializable]
	public class Sound {
		public string m_name;
		public AudioClip m_audioClip;

		[Range(0f,1f)]
		public float m_volume;
		[Range(.1f,3f)]
		public float m_pitch;
		public bool m_loop;

		[HideInInspector]
		public AudioSource m_audioSource;
	}

	public Sound[] m_sounds;

	protected override void Awake()
	{
		base.Awake();

		foreach(Sound sound in m_sounds) {
			sound.m_audioSource = gameObject.AddComponent<AudioSource>();
			sound.m_audioSource.clip = sound.m_audioClip;
			sound.m_audioSource.volume = sound.m_volume;
			sound.m_audioSource.pitch = sound.m_pitch;
			sound.m_audioSource.loop = sound.m_loop;
		}
	}
	
	private void Start() 
	{
		Play("Theme");
	}

	public void Play(string soundName)
	{
		Sound sound = Array.Find(m_sounds, sound => sound.m_name == soundName);
		Assert.IsNotNull(sound);
		sound.m_audioSource.Play();
	}

	public void Stop(string soundName)
	{
		Sound sound = Array.Find(m_sounds, sound => sound.m_name == soundName);
		Assert.IsNotNull(sound);
		sound.m_audioSource.Stop();
	}

	public void Stop()
	{
		Array.ForEach(m_sounds, sound => sound.m_audioSource.Stop());
	}
}
