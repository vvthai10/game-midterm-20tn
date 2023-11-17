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
                        sfxArcher,
                        sfxBossSource;

    public static string RUN = "run";
    public static string WALK = "walk";
    List<string> sfx_have_time_exit = new  List<string> () { RUN, WALK };

    private void Awake() {
        if(AudioManager.Instance == null) {
            Debug.Log("[INFO] init");
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Debug.Log("[INFO] exist");
            Destroy(gameObject);
        }
    }

    private void Start(){
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex == 0) {
            PlayBackgroundMusic("Menu");
        }
        else{
            PlayBackgroundMusic("Elphael");
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
