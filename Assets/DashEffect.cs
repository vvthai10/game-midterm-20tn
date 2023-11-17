using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEffect : MonoBehaviour
{
    public static string anim_dash_effect = "dash";
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

    public void playDashEffectAnimation()
    {
        spriteRenderer.flipX = main_character.instance.flipX;
        Debug.Log("Animation Dash Effect...");
        if (main_character.instance.flipX)
        {
            gameObject.transform.position = new Vector3(main_character.instance.transform.position.x + 1, main_character.instance.transform.position.y - 0.5f);
        }
        else
        {
            gameObject.transform.position = new Vector3(main_character.instance.transform.position.x - 1, main_character.instance.transform.position.y - 0.5f);
        }
        anim.SetTrigger(anim_dash_effect);
    }
}