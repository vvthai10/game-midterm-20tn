using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene2Unique : MonoBehaviour
{
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        gameManager.LoadMainStateForTransitionScene();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
