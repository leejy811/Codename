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
    [SerializeField] protected float attackRange;
    [SerializeField] protected MonsterType monsterType;
    [SerializeField] protected MonsterBulletController bullet;

    protected bool isAttacking;

    protected MonsterState monsterState=MonsterState.Idle;

    protected void Init()
    {
        isAttacking = false;
        isMoving = false;
        posX = 4;
        posY = 8;
    }

    //TODO : make this function virtual. every each monsters will redefine its own state pattern
    protected void StateMachine() {
        if (isMoving || isAttacking)
            return;
        if (Vector3.Distance(transform.position, StageManager.instance.PlayerWorldPos()) <= attackRange)
            monsterState = MonsterState.Attack;
        else
            monsterState = MonsterState.Move;
    }

    protected override void TryMove()
    {
        if (isMoving || isAttacking)
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

    //TODO : make the body of the function. it will choose atk pattern randomly
    protected abstract void PatternRoll();

    protected  void TryAttack()
    {
        if (isMoving||isAttacking)
            return;
        Attack();
    }

    //TODO : make all parameter as variable. 
    protected void Attack()
    {
        isAttacking = true;
        StartCoroutine(AttackPattern.PatternA(bullet, 6, 10, 10, transform.position,this));
    } 

    /// <summary>
    /// this function will be called from 'AttackPattern'. it allows to know that the attack is done
    /// </summary>
    public void FinishAttack()
    {
        isAttacking = false;
    }
}
