using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void MoveToPlayer()
    {

    }

    public virtual void Attack()
    {

    }
}
