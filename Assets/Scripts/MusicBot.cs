using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBot : MonoBehaviour
{
    GameObject[] musicObject;
    [HideInInspector]
    public AudioSource audio;
    bool muted = false;
    public AudioClip theme;
    public AudioClip gameSong;


    void Start()
    {
        audio = GetComponent<AudioSource>();
        musicObject = GameObject.FindGameObjectsWithTag("MusicBot");
        if (musicObject.Length == 1)
        {
            audio.Play();
        }
        else
        {
            for (int i = 1; i < musicObject.Length; i++)
            {
                Destroy(musicObject[i]) ;
            }
        }
    }

    public void PlayTheme()
    {
        audio.clip = theme;
        audio.Play();
    }

    public void PlayGameMusic()
    {
        audio.clip = gameSong;
        audio.Play();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M) && !muted)
            muted = true;
        else if(Input.GetKeyDown(KeyCode.M) && muted)
            muted = false;

        if (!muted)
            audio.UnPause();
        else
            audio.Pause();
    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
