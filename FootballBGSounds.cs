using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootballBGSounds : MonoBehaviour
{
    [SerializeField]
    List<AudioClip> audioClips;
    int i;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<AudioSource>().clip = audioClips[0];
        i = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameObject.GetComponent<AudioSource>().isPlaying)
        {
            i++;
            if(i >= audioClips.Count)
            {
                return;
            }
            gameObject.GetComponent<AudioSource>().clip = audioClips[i];
            gameObject.GetComponent<AudioSource>().Play();
        }
    }
}
