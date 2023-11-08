using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack_range : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform areaAttackStart;
    public Transform areaAttackEnd;
    public float detectAreaHeight = 4f;

    Vector2 topLeftPoint;
    Vector2 bottomRightPoint;
    enemy_patrol patrol;
    void Start()
    {
        topLeftPoint = new Vector2(areaAttackStart.position.x, areaAttackStart.position.y + detectAreaHeight / 2);
        bottomRightPoint = new Vector2(areaAttackEnd.position.x, areaAttackEnd.position.y - detectAreaHeight / 2);


        patrol = GetComponent<enemy_patrol>(); 
    }

    // Update is called once per frame
    void Update()
    {
        detectPlayer();
    }

    public void detectPlayer()
    {
        Collider2D[] colliders = Physics2D.OverlapAreaAll(topLeftPoint, bottomRightPoint);

        foreach(Collider2D collider in colliders)
        {
            
            if(collider.tag == "Player")
            {
                patrol.setPatrolState(false);
                return;
            }
        }

        patrol.setPatrolState(true);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLineStrip(new Vector3[4] {
            new Vector3(topLeftPoint.x, topLeftPoint.y, 0),
             new Vector3(areaAttackEnd.position.x, areaAttackEnd.position.y + detectAreaHeight / 2, 0),
             new Vector3(bottomRightPoint.x, bottomRightPoint.y, 0),
             new Vector3(areaAttackStart.position.x, areaAttackStart.position.y - detectAreaHeight / 2, 0)
        }, true);
    }
}
