using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum MonsterType
{
    temp,
}

public enum MonsterState
{
    Idle,
    Move,
    Attack,
}

public abstract class MonsterController : ActiveObject
{
    [SerializeField] protected string monsterName;
    [SerializeField] protected float attackDelay;
    [SerializeField] protected MonsterType monsterType;

    protected bool isAttacking;

    protected MonsterState monsterState=MonsterState.Idle;


    protected void Init()
    {
        isAttacking = false;
        isMoving = false;
        posX = 4;
        posY = 8;
    }

    protected override void TryMove()
    {
        if (isMoving)
            return;
        StartCoroutine(Move());
    }

    protected IEnumerator Move()
    {
        isMoving = true;
        var d = PathFinder.GetDir(new int[] { posX, posY }, 1, 3);
        var tween = transform.DOMove(new Vector3(d[1], -d[0], 0), 1f).SetRelative().SetEase(Ease.Linear);
        posX += d[0];
        posY += d[1];

        yield return tween.WaitForCompletion();
        isMoving = false;
    }

    protected  void TryAttack()
    {
        if (isAttacking)
            return;
        StartCoroutine(Attack());
    }

    protected IEnumerator Attack()
    {
        isAttacking = true;
        DoAttack();
        yield return new WaitForSeconds(attackDelay);
        isAttacking = false;
    }

    protected virtual void DoAttack()
    {

    }
}
