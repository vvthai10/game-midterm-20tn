using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SoulAmount : MonoBehaviour
{
    // Start is called before the first frame update
    private float souls = 0f;
    private float updatedSouls = 0f;
    private TextMeshProUGUI _textMeshPro;
    public static SoulAmount instance;
    void Start()
    {
        instance = this;
        _textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (souls < updatedSouls)
        {

            souls += Mathf.Ceil((updatedSouls - souls) * Time.deltaTime);
            
        }
        if (souls > updatedSouls)
        {
            souls = updatedSouls;
        }
        _textMeshPro.text = Mathf.Floor(souls).ToString();
    }

    public void UpdateSouls(float amount)
    {
        updatedSouls = amount;
        SoulsIncrease.instance.ShowUI((int)(amount - souls));
    }
}
