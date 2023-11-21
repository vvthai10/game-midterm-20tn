using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{

    public static bool IsOpenShop = false;
    public Canvas Dialogue;
    public GameObject shopDialogController;

    private DialogueMng dialogueManager;
    private bool userInRange = false; 
  
    // Start is called before the first frame update
    void Start()
    {
        Dialogue.enabled = false;
        dialogueManager = FindObjectOfType<DialogueMng>();
    }

    // Update is called once per frame
    void Update()
    {
        if(userInRange)
        {
            Debug.Log("User in range");
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("User in range and click E");
                shopDialogController.SetActive(true);
                IsOpenShop = true;
            }
        }

        if (IsOpenShop) {
            Time.timeScale = 0;
        }
        else {
            Time.timeScale = 1;
        }
    }

    public void OnShopClose() {
        IsOpenShop = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.tag);
        if(collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Callllll");
            //show dialog 
            //click E to open shop
            ShouldShowDialog(true);
            dialogueManager.ShowDialogue();
            userInRange = true;
            return;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            ShouldShowDialog(false);
            dialogueManager.ClearText();
            userInRange = false;
            return;
        }
    }

    private void ShouldShowDialog(bool isShow)
    {
        Dialogue.enabled = isShow;
    }
}
