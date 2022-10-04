using UnityEngine.Audio;
using UnityEngine;
using System;
public class AudioManager : MonoBehaviour
{

     [System.Serializable]
    public class Sound{
        public string name;
        public AudioClip clip;

        [Range(0f, 1f)]
        public float volume;

        [Range(0.1f, 3f)]

        public float pitch;

    [HideInInspector]
        public AudioSource source;
    }
   

    public Sound[] sounds;
    // Start is called before the first frame update
    void Awake()
    {
        foreach (Sound s in sounds) 
        {
            s.source=gameObject.AddComponent<AudioSource>();
            s.source.clip=s.clip;
            s.source.volume=s.volume;
            s.source.pitch=s.pitch;
        }
        
    }

    public void Play(string name)
    {
        Sound s =  Array.Find(sounds, sound=> sound.name==name );
        s.source.Play();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
