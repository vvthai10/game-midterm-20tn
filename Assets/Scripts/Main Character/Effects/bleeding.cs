using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bleeding : MonoBehaviour
{
    public static string anim_bleed = "bleeding";
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void playBleedAnimation()
    {
        anim.SetTrigger(anim_bleed);
    }
}
