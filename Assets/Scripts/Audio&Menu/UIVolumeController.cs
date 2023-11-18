using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIVolumeController : MonoBehaviour
{
    public Slider _bgSlider, _ambientSlider, _sfxSlider;
    public Text bgVolumeText, ambientVolumeText, sfxVolumeText;

    void Start() {
        if(!PlayerPrefs.HasKey("bgVolume")) {
            PlayerPrefs.SetFloat("bgVolume", 0.5f);
        }
        if(!PlayerPrefs.HasKey("ambientVolume")) {
            PlayerPrefs.SetFloat("ambientVolume", 0.2f);
        }
        if(!PlayerPrefs.HasKey("sfxVolume")) {
            PlayerPrefs.SetFloat("sfxVolume", 1);
        }

        LoadBackgroundVolume();
        LoadAmbientVolume();
        LoadSFXVolume();

        UpdateUITextBackgroundVolume();
        UpdateUITextAmbientVolume();
        UpdateUITextSFXVolume();

        BackgroundVolume();
        AmbientVolume();
        SFXVolume();
    }

    public void ToggleBackground() {
        AudioManager.Instance.ToggleBackground();
    }
    public void ToggleAmbient() {
        AudioManager.Instance.ToggleAmbient();
    }
    public void ToggleSFX() {
        AudioManager.Instance.ToggleSFX();
    }

    public void BackgroundVolume() {
        SaveBackgroundVolume();
        UpdateUITextBackgroundVolume();
        AudioManager.Instance.BackgroundVolume(_bgSlider.value);
    }
    public void AmbientVolume() {
        SaveAmbientVolume();
        UpdateUITextAmbientVolume();
        AudioManager.Instance.AmbientVolume(_ambientSlider.value);
    }
    public void SFXVolume() {
        SaveSFXVolume();
        UpdateUITextSFXVolume();
        AudioManager.Instance.SFXVolume(_sfxSlider.value);
    }

    private void SaveBackgroundVolume() {
        float bgVolumeValue = _bgSlider.value;
        PlayerPrefs.SetFloat("bgVolume",bgVolumeValue);
    }
    private void SaveAmbientVolume() {
        float ambientVolumeValue = _ambientSlider.value;
        PlayerPrefs.SetFloat("ambientVolume",ambientVolumeValue);
    }
    private void SaveSFXVolume() {
        float sfxVolumeValue = _sfxSlider.value;
        PlayerPrefs.SetFloat("sfxVolume",sfxVolumeValue);
    }

    private void LoadBackgroundVolume() {
        float bgVolumeValue = PlayerPrefs.GetFloat("bgVolume");
        _bgSlider.value = bgVolumeValue;
    }
    private void LoadAmbientVolume() {
        float ambientVolumeValue = PlayerPrefs.GetFloat("ambientVolume");
        _ambientSlider.value = ambientVolumeValue;
    }
    private void LoadSFXVolume() {
        float sfxVolumeValue = PlayerPrefs.GetFloat("sfxVolume");
        _sfxSlider.value = sfxVolumeValue;
    }

    private void UpdateUITextBackgroundVolume() {
        bgVolumeText.text = _bgSlider.value.ToString("0.0");
    }
    private void UpdateUITextAmbientVolume() {
        ambientVolumeText.text = _ambientSlider.value.ToString("0.0");
    }
    private void UpdateUITextSFXVolume() {
        sfxVolumeText.text = _sfxSlider.value.ToString("0.0");
    }
}
