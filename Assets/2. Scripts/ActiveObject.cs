using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections;

abstract public class ActiveObject : MonoBehaviour
{
    [SerializeField] protected float hp;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected int sizeX;
    [SerializeField] protected int sizeY;
    [SerializeField] protected float iFrameTime;
    [SerializeField] protected SpriteRenderer sprite;

    protected bool isMoving = false;
    protected bool iFrame = false;
    protected bool isDead = false;

    protected int posX;
    protected int posY; 

    protected abstract void TryMove();
    protected abstract void Die();

    public void GetDamage(float value)
    {
        if (isDead)
            return;
        if(iFrame) 
            return;

        hp -= value;
        if (hp <= 0)
        {
            isDead = true;
            Die();
            return;
        }
        StartCoroutine(StartIFrame());
    }

    IEnumerator StartIFrame()
    {
        iFrame = true;
        yield return new WaitForSeconds(iFrameTime);
        iFrame = false; ;
    }

    protected void ReorderSortingLayer()
    {
        sprite.sortingOrder = -(int)transform.position.y;
    }
}
