using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heal : MonoBehaviour
{
    public static string anim_heal = "heal";
    Animator anim;
    public static heal instance;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void playHealAnimation()
    {
        anim.SetTrigger(anim_heal);
    }
}
