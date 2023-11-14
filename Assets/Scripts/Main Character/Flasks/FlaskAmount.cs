using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlaskAmount : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI _textMeshPro;
    void Start()
    {
        _textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        _textMeshPro.text = main_character.instance.GetCurrentFlaskAmount().ToString();
    }
}
