using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeManager : MonoBehaviour
{
    private string type = "easy";

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        
    }

    public void SetMode(string mode)
    {
        this.type = mode;
    }

    public string GetMode()
    {
        return type;
    }
}
