using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterDialog : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    private CanvasGroup canvasGroup;
    public string[] lines;
    public float textSpeed;
    public static CharacterDialog instance;
    private int start;
    private int end;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex == 0)
        {

        }
        else if (currentSceneIndex == 1)
        {
            StartDialog(0, 2);
        }

        else if (currentSceneIndex == 2)
        {
            StartDialog(7, 8);
        }
        else if (currentSceneIndex == 3)
        {
            StartDialog(14, 22);
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(textComponent.text == lines[start])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines(); ;
                textComponent.text = lines[start]; 
            }
        }
    }

    public void StartDialog(int s, int e)
    {
        canvasGroup.alpha = 1;
        gameObject.SetActive(true);
        start = s;
        end = e;
        StartCoroutine(TypeLine());
        
    }

    private IEnumerator TypeLine()
    {
        textComponent.text = "";
        foreach (char letter in lines[start].ToCharArray())
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (start < end)
        {
            start++;
            textComponent.text = "";
            StartCoroutine (TypeLine());
        }
        else
        {
            canvasGroup.alpha = 0;
        }
    }

}
