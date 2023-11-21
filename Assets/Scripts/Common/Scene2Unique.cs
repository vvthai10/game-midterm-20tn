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
        gameManager.SetDeathReason("none");
        gameManager.LoadMainStateForTransitionScene();
        main_character.instance.oldSouls = main_character.instance.souls;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
