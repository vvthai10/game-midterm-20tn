using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeteorsController : MonoBehaviour
{
    private MeteorController[] meteorControllers;

    public float offsetXFromPlayer = 10f;

    private void Awake()
    {
        meteorControllers = GetComponentsInChildren<MeteorController>();
    }

    public void Cast()
    {
        float playerX = main_character.instance?.transform?.position.x ?? 0;

        foreach (MeteorController controller in meteorControllers)
        {
            controller.RepositionX(Random.Range(playerX - offsetXFromPlayer, playerX + offsetXFromPlayer));
            controller.Cast();
        }
    }
}
