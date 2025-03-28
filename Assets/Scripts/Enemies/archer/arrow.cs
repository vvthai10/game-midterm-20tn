using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrow : MonoBehaviour
{
    public float speed = 75f;
    Rigidbody2D rd;
    Transform player;
    private float attackDamage = 20f;
    // Start is called before the first frame update
    void Start()
    {
        rd = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        Vector2 direction = player.transform.position - transform.position;
        rd.velocity = new Vector2(direction.x, direction.y).normalized * speed;

        float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg; 
        transform.rotation = Quaternion.Euler(0,0 ,rot);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            //Deal damage to player
            main_character.instance.TakeDameage(attackDamage);
            GameManager.instance.SetDeathReason("monster");
            Destroy(gameObject);
        }
       
    }

    public void SetAttackDamage(float damage)
    {
        attackDamage = damage;
    }
}
