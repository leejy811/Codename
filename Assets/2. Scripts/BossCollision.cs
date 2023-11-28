using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCollision : MonoBehaviour
{
    [SerializeField]
    private float collisionDamage = 10f;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Health>().Damage(collisionDamage, this.gameObject, 0, 0, collision.gameObject.transform.position - transform.position, null);
        }
    }
}
