using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueMng : MonoBehaviour
{
    public TextMeshProUGUI dialogTxt;
    public string sentence;
    public float showSpeed;

    private Coroutine showCo;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            StartCoroutine(ShowCo());
        }
    }

    private IEnumerator ShowCo()
    {
        dialogTxt.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogTxt.text += letter;
            yield return new WaitForSeconds(showSpeed);
        }
    }

    public void ShowDialogue()
    {
        StartCoroutine(ShowCo());
    }
    public void ClearText()
    {
        dialogTxt.text = "";
    }
}
