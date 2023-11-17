using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollEffect : MonoBehaviour
{
    public static string anim_roll_effect = "roll";
    Animator anim;
    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void playRollEffectAnimation()
    {
        spriteRenderer.flipX = main_character.instance.flipX;
        if (main_character.instance.flipX)
        {
            gameObject.transform.position = new Vector3(main_character.instance.transform.position.x + 1, main_character.instance.transform.position.y - 1);
        }
        else
        {
            gameObject.transform.position = new Vector3(main_character.instance.transform.position.x - 1, main_character.instance.transform.position.y - 1);
        }
        anim.SetTrigger(anim_roll_effect);
    }
}
