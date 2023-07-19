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
        TryMove();
        TryAttack();
    }
    
    protected override void DoAttack()
    {
        base.DoAttack();
        StartCoroutine(AttackPattern.PatternA(bullet, 6, 10, 10, transform.position));
    }

    protected override void Die()
    {
        throw new System.NotImplementedException();
    }
}
