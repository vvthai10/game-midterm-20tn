using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public GameObject uiGuide;
    public TextMeshProUGUI instructionText;
    private int step = 0;
    private bool instructionsCompleted = false;
    private float keyHoldStartTime = 0f;
    private float keyHoldDuration = 2f;

    public GameObject object1 = null, object2 = null;

    void Start()
    {
        // uiGuide.SetActive(false);
        // if (!PlayerPrefs.HasKey("FIRSTTIMEOPENING"))
        // {
        //     PlayerPrefs.SetInt("FIRSTTIMEOPENING", 0);
        // }

        if (!FindObjectOfType<ModeManager>().isContinueClick || SceneManager.GetActiveScene().buildIndex == 1) {
            main_character.instance.healthBar.health = 70;
            uiGuide.SetActive(true);
            if (object1 != null)
            {
                object1.SetActive(false);
            }
            if (object2 != null)
            {
                object2.SetActive(false);
            }
            StartCoroutine(ShowInstructions());
        }
    }

    IEnumerator ShowInstructions()
    {
        instructionText.text = "Player's guide";
        yield return new WaitForSeconds(2f);

        while (step < 15)
        {
            if (step < 2)
            {
                instructionText.text = "Press the D key to move right";
                if (Input.GetKeyDown(KeyCode.D))
                {
                    keyHoldStartTime = Time.time;
                    step++;
                }
            }
            else if (step < 4)
            {
                instructionText.text = "Press A to move left";
                if (Input.GetKeyDown(KeyCode.A))
                {
                    step++;
                }
            }
            else if (step < 6)
            {
                instructionText.text = "Press F key to jump";
                if (Input.GetKeyDown(KeyCode.F))
                {
                    step++;
                }
            }
            else if (step < 8)
            {
                instructionText.text = "Press the Space key to roll";
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    step++;
                }
            }
            else if (step < 10)
            {
                instructionText.text = "Left mouse click to attack";
                if (Input.GetMouseButtonDown(0))
                {
                    step++;
                }
            }
            else if (step < 12)
            {
                instructionText.text = "Right mouse click to defend. You will be healed if caught at the right time";
                if (Input.GetMouseButtonDown(1))
                {
                    step++;
                }
            }
            else if (step < 13)
            {
                instructionText.text = "Press R key to heal";
                if (Input.GetKeyDown(KeyCode.R))
                {
                    step++;
                }
            }
            else if (step < 15)
            {
                instructionText.text = "Press the F key continuously to climb the wall";
                if (Input.GetKeyDown(KeyCode.F))
                {
                    step++;
                }
            }
            yield return null;
        }

        instructionText.text = "Instructions complete!";
        yield return new WaitForSeconds(2f);
        instructionText.text = "Start playing!";
        yield return new WaitForSeconds(2f);
        instructionText.text = "";
        instructionsCompleted = true;
        uiGuide.SetActive(false);
        if (object1 != null) {
            object1.SetActive(true);
        }
        if (object2 != null) {
            object2.SetActive(true);
        }
    }

    void Update()
    {
        if (instructionsCompleted && !string.IsNullOrEmpty(instructionText.text))
        {
            // Xóa Text khi hướng dẫn hoàn thành
            instructionText.text = "";
        }
    }
}
