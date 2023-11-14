using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 1;
    public float damage = 20;
    public float lifetime = 5;
    private bool hit;
    private float direction;
    private float lifetimeTimer = 0;

    private Animator animator;
    private BoxCollider2D boxcollider;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        boxcollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hit) 
            return;
        float movementSpeed = speed * Time.deltaTime * direction;

        //Debug.Log("transform position x: " + transform.position.x.ToString());
        //Debug.Log("movement speed: " + movementSpeed.ToString());
        //Debug.Log("transform position x + movement speed: " + (transform.position.x + movementSpeed).ToString());

        //transform.position = new Vector3(transform.position.x + movementSpeed, transform.position.y, transform.position.z);

        transform.Translate(movementSpeed, 0, 0);

        lifetimeTimer += Time.deltaTime;
        if(lifetimeTimer > lifetime)
            this.Deactivate();

        //Debug.Log("new transform position x: " + transform.position.x.ToString());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        boxcollider.enabled = false;
        animator.SetTrigger("explode");
        if (collision.gameObject.CompareTag("Player"))
        {
            main_character.instance.takeDameage(damage);
        }
    }

    public void SetDirection(float _direction)
    {
        lifetimeTimer = 0;
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        boxcollider.enabled = true;

        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != direction)
            localScaleX = -localScaleX;

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }
    
    public void Hit()
    {
        this.Deactivate();
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
