using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource ambientSource;
    [SerializeField] AudioSource sfxSource;

    [Header("Audio Clip")]
    public AudioClip music_background;
    public AudioClip ambient_soft_rain;

    public AudioClip sfx_walk;
    public AudioClip sfx_run;
    // Start is called before the first frame update
    void Start()
    {
        musicSource.clip = music_background;
        ambientSource.clip = ambient_soft_rain;
        musicSource.Play();
        ambientSource.volume = 0.8f;
        ambientSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
