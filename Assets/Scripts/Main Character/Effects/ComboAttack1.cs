using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboAttack1 : MonoBehaviour
{
    public static string anim_attack_effect = "combo_attack_1";
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

    public void playAttack1EffectAnimation()
    {
        spriteRenderer.flipX = main_character.instance.flipX;
        if (main_character.instance.flipX)
        {
            gameObject.transform.position = new Vector3(main_character.instance.transform.position.x - 1.2f, main_character.instance.transform.position.y - 0.75f);
        }
        else
        {
            gameObject.transform.position = new Vector3(main_character.instance.transform.position.x + 1.2f, main_character.instance.transform.position.y - 0.75f);
        }
        anim.SetTrigger(anim_attack_effect);
    }
}
