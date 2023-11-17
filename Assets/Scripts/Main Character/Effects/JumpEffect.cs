using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpEffect : MonoBehaviour
{
    public static string anim_jump_start_effect = "jump_start";
    public static string anim_jump_end_effect = "jump_end";

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

    public void playJumpEffectAnimation()
    {
        spriteRenderer.flipX = main_character.instance.flipX;
        gameObject.transform.position = new Vector3(main_character.instance.transform.position.x, main_character.instance.transform.position.y - 1);
        anim.SetTrigger(anim_jump_start_effect);
    }

    public void playLandEffectAnimation()
    {
        spriteRenderer.flipX = main_character.instance.flipX;
        gameObject.transform.position = new Vector3(main_character.instance.transform.position.x, main_character.instance.transform.position.y - 1);
        anim.SetTrigger(anim_jump_end_effect);
    }
}
