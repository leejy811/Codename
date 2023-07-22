using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MonsterTemp : MonsterController
{
    void Start()
    {
        Init();
    }

    private void Update()
    {
        if (isDead)
            return;
        ReorderSortingLayer();
        StateMachine();

        switch (monsterState)
        {
            case MonsterState.Move:
                TryMove();
                break;
            case MonsterState.Attack:
                PatternRoll();
                TryAttack();
                break;
            default:
                Debug.LogError("error : undefined enemy state");
                break;
        }
    }

    protected override void Die()
    {
        Debug.Log("Monster died");
    }

    protected override void PatternRoll()
    {
        // random roll to pick which attack pattern enenmy will use
    }
}
