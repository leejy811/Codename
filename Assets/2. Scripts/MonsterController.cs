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

public abstract class MonsterController : MonoBehaviour
{
    [SerializeField] protected string monsterName;
    [SerializeField] protected float monsterHp;
    [SerializeField] protected float monsterSpeed;
    [SerializeField] protected float monsterDamage;
    [SerializeField] protected MonsterType monsterType;

    protected bool isMoving = false;
    protected bool isAttacking = false;

    protected MonsterState monsterState=MonsterState.Idle;

    protected int posX;
    protected int posY;

    protected void Init()
    {

    }

    public void GetDamage()
    {

    }

    protected void TryMove()
    {
        if (isMoving)
            return;
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        isMoving = true;
        var d = PathFinder.GetDir(new int[] { posX, posY }, 3, 3);
        var tween = transform.DOMove(new Vector3(d[1], -d[0], 0), 1f).SetRelative().SetEase(Ease.Linear);
        posX += d[0];
        posY += d[1];

        yield return tween.WaitForCompletion();
        isMoving = false;
    }

    public virtual void Attack()
    {

    }
}
