using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType
{
    temp,
}


public abstract class MonsterController : MonoBehaviour
{
    [SerializeField] protected string monsterName;
    [SerializeField] protected float monsterHp;
    [SerializeField] protected float monsterSpeed;
    [SerializeField] protected MonsterType monsterType;
}
