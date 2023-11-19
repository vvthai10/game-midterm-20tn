using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboAttack2 : MonoBehaviour
{
    public static string anim_attack_effect = "combo_attack_2";
    Animator anim;
    SpriteRenderer spriteRenderer;
    public static ComboAttack2 instance;
    private void Awake()
    {
        instance = this;
    }

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

    public void playAttack2EffectAnimation()
    {
        spriteRenderer.flipX = main_character.instance.flipX;
        if (main_character.instance.flipX)
        {
            gameObject.transform.position = new Vector3(main_character.instance.transform.position.x - 1.5f, main_character.instance.transform.position.y - 0.8f);
        }
        else
        {
            gameObject.transform.position = new Vector3(main_character.instance.transform.position.x + 1.5f, main_character.instance.transform.position.y - 0.8f);
        }
        anim.SetTrigger(anim_attack_effect);
    }
}
