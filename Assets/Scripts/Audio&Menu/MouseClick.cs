using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MouseClick : MonoBehaviour
{

    public static MouseClick Instance;
    private void Awake() {
        if(MouseClick.Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public Sound clickSound, hoverSound;
    public AudioSource mouseSource;
    // Start is called before the first frame update
    public void PlayMouseClick() {
        mouseSource.PlayOneShot(clickSound.clip);
    }

    public void PlayMouseHover() {
        mouseSource.PlayOneShot(hoverSound.clip);
    }
}
