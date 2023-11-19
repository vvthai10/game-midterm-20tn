using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentFade : MonoBehaviour
{

    public static TransparentFade Instance;

    public int numSteps = 10;
    public float timeBetweenSteps = 1.0f;
    public SpriteRenderer spriteRenderer1;
    public SpriteRenderer spriteRenderer2;

    void Start() {
        Debug.Log("Init TransparentFade");
    }

    private void Awake() {
        if(TransparentFade.Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void StartIncrease()
    {
        StartCoroutine(IncreaseTransparencyOverTime());
    }

    public void StartDecrease()
    {
        StartCoroutine(DecreaseTransparencyOverTime());
    }

    System.Collections.IEnumerator IncreaseTransparencyOverTime()
    {
        for (int i = 0; i <= 5; i++)
        {
            float newTransparency = i / (float)numSteps;
            SetTransparency(newTransparency);
            yield return new WaitForSeconds(timeBetweenSteps);
        }
    }

    System.Collections.IEnumerator DecreaseTransparencyOverTime()
    {
        for (int i = 5; i >= 0; i--)
        {
            float newTransparency = i / (float)numSteps;
            SetTransparency(newTransparency);
            yield return new WaitForSeconds(timeBetweenSteps);
        }
    }

    void SetTransparency(float alpha)
    {
        Color currentColor1 = spriteRenderer1.color;
        currentColor1.a = alpha;

        spriteRenderer1.color = currentColor1;

        Color currentColor2 = spriteRenderer2.color;
        currentColor2.a = alpha;

        spriteRenderer2.color = currentColor2;
    }
}
