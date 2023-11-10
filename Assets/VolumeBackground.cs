using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeBackground : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private Text volumeTextUI = null;
    public  AudioSource audioSource;

    public string typeAudio = "";

    void Start()
    {
        // audioSource = GetComponent<AudioSource>();

        if(!PlayerPrefs.HasKey(typeAudio)) {
            PlayerPrefs.SetFloat(typeAudio, 0);
        }
        LoadValues();
    }

    public void VolumeSlider(float volume){
        volumeTextUI.text = volume.ToString("0.0");
        SaveVolumeButton();
    }

    void SaveVolumeButton(){
        float volumeValue = volumeSlider.value;
        PlayerPrefs.SetFloat(typeAudio, volumeValue);
        LoadValues();
    }
    
    void LoadValues(){
        float volumeValue = PlayerPrefs.GetFloat(typeAudio);
        volumeSlider.value = volumeValue;
        audioSource.volume = volumeValue;
    }
}
