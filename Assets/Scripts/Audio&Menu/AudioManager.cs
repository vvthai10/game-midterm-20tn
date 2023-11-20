using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance;
    public Sound[] bgSounds, ambientSounds,
                    sfxSounds,
                    sfxPuncherSounds,
                    sfxLazerSounds,
                    sfxArcherSounds,
                    sfxBossSounds;

    public AudioSource bgSource, ambientSource,
                        sfxSource,
                        sfxPuncherSource,
                        sfxLazerSource,
                        sfxArcherSource,
                        sfxBossSource;

    public static string RUN = "run";
    public static string WALK = "walk";
    public static string SLIDE = "slide";
    List<string> sfx_have_time_exit = new  List<string> () { RUN, WALK, SLIDE };

    private void Awake() {
        if(AudioManager.Instance == null) {
            Debug.Log("[INFO] init");
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Debug.Log("[INFO] exist");
            //Destroy(gameObject);
        }
    }

    private void Start(){
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex == 0) {
            PlayBackgroundMusic("Menu");
        }
        else if (currentSceneIndex == 1)
        {
            PlayBackgroundMusic("Elphael");
            PlayAmbientMusic("rain");
        }

        else if (currentSceneIndex == 2)
        {
            PlayBackgroundMusic("Caves");
            PlayAmbientMusic("rain");
        }
    }

    public void PlayBackgroundMusic(string name) {
        Sound s = Array.Find(bgSounds, x => x.name == name);
        if (s == null) {
            Debug.Log(name + " Background Sound Not Found");
        } else {
            bgSource.enabled = true;
            bgSource.clip = s.clip;
            Debug.Log("Music name: " + name);
            bgSource.Play();
        }
    }

    public void PlayAmbientMusic(string name) {
        Sound s = Array.Find(ambientSounds, x => x.name == name);
        if (s == null) {
            Debug.Log(name + " Ambient Sound Not Found");
        } else {
            ambientSource.enabled = true;
            ambientSource.clip = s.clip;
            ambientSource.Play();
        }
    }

    public void PlaySFXMusic(string name, float speed = 1f) {
        Sound s = Array.Find(sfxSounds, x => x.name == name);
        if (s == null) {
            Debug.Log(name + " SFX Sound Not Found");
            return;
        } else if (sfx_have_time_exit.Find(x => x == name) == null || !sfxSource.isPlaying) {
            sfxSource.enabled = true;
            sfxSource.pitch = speed;
            sfxSource.PlayOneShot(s.clip);
        }
    }

    public void PlaySFXLazerMusic(string name, float speed = 1f)
    {
        Sound s = Array.Find(sfxLazerSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log(name + " SFX Sound Not Found");
            return;
        }
        else
        {
            sfxLazerSource.enabled = true;
            sfxLazerSource.pitch = speed;
            sfxLazerSource.PlayOneShot(s.clip);
        }
    }

    public void PlaySFXPuncherMusic(string name, float speed = 1f)
    {
        Sound s = Array.Find(sfxPuncherSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log(name + " SFX Sound Not Found");
            return;
        }
        else
        {
            sfxPuncherSource.enabled = true;
            sfxPuncherSource.pitch = speed;
            sfxPuncherSource.PlayOneShot(s.clip);
        }
    }

    public void PlaySFXArcherMusic(string name, float speed = 1f)
    {
        Sound s = Array.Find(sfxArcherSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log(name + " SFX Sound Not Found");
            return;
        }
        else
        {
            sfxArcherSource.enabled = true;
            sfxArcherSource.pitch = speed;
            sfxArcherSource.PlayOneShot(s.clip);
        }
    }


    public void PlaySFXBossMusic(string name, float speed = 1f)
    {
        Sound s = Array.Find(sfxBossSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log(name + " SFX Sound Not Found");
            return;
        }
        else
        {
            sfxBossSource.enabled = true;
            sfxBossSource.pitch = speed;
            sfxBossSource.PlayOneShot(s.clip);
        }
    }

    public void ToggleBackground() {
        bgSource.mute = !bgSource.mute;
    }

    public void ToggleAmbient() {
        ambientSource.mute = !ambientSource.mute;
    }

    public void ToggleSFX() {
        sfxSource.mute = !sfxSource.mute;
    }

    public void BackgroundVolume(float volume){
        bgSource.volume = volume;
    }

    public void AmbientVolume(float volume){
        ambientSource.volume = volume;
    }

    public void SFXVolume(float volume) {
        sfxSource.volume = volume;
    }
}
