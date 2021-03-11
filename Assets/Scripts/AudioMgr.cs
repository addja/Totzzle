using System;
using UnityEngine.Assertions;
using UnityEngine.Audio;
using UnityEngine;

public class AudioMgr : MonoBehaviour
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

    // BEGIN Singleton stuff
    public static AudioMgr Instance
    {
        get { return s_Instance; }
    }
    protected static AudioMgr s_Instance;

    void SingletonAwake()
    {
        if (s_Instance == null)
            s_Instance = this;
        else
            throw new UnityException("There cannot be more than one GridMgr script.  The instances are " + s_Instance.name + " and " + name + ".");
    }

    void OnEnable()
    {
        if (s_Instance == null)
            s_Instance = this;
        else if (s_Instance != this)
            throw new UnityException("There cannot be more than one GridMgr script.  The instances are " + s_Instance.name + " and " + name + ".");
    }

    void OnDisable()
    {
        s_Instance = null;
    }
    // END Singleton stuff

    public Sound[] m_sounds;

    private void Awake()
    {
        SingletonAwake();

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
}
