using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class ActiveObject : MonoBehaviour
{
    [SerializeField] protected float hp;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected int sizeX;
    [SerializeField] protected int sizeY;
    [SerializeField] protected SpriteRenderer sprite;

    protected bool isMoving = false;
    protected bool isDead = false;

    protected int posX;
    protected int posY; 

    protected abstract void TryMove();
    protected abstract void Die();

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


    protected void ReorderSortingLayer()
    {
        sprite.sortingOrder = -(int)transform.position.y;
    }
}
