using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class ActiveObject : MonoBehaviour
{
    [SerializeField] protected float hp;
    [SerializeField] protected float moveSpeed;

    protected bool isMoving = false;
    protected bool isDead = false;

    protected int posX;
    protected int posY;

    protected virtual void TryMove()
    {
        
    }

    public void GetDamage(float value)
    {
        Debug.Log("111");
        hp -= value;
        if (hp <= 0)
        {
            isDead = true;
            Die();
        }
    }

    protected virtual void Die()
    {

    }

    protected void ReorderSortingLayer()
    {
        if(GetComponent<SpriteRenderer>())
            GetComponent<SpriteRenderer>().sortingOrder = -(int)transform.position.y;
        else
        {
            var children = transform.GetComponentsInChildren<SpriteRenderer>();
            foreach(var child in children)
                child.sortingOrder= -(int)transform.position.y;
        }
    }
}
