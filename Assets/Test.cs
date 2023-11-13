using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rd;
    void Start()
    {
        rd = GetComponent<Rigidbody2D>();
        rd.velocity = new Vector2(1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            rd.velocity = new Vector2(-1, 0);
        } else if(Input.GetKeyDown(KeyCode.S))
        {
            rd.velocity = new Vector2(0, 0);
        } else if( Input.GetKeyDown(KeyCode.D))
        {
            rd.velocity = new Vector2(1, 0);
        }
    }
}
