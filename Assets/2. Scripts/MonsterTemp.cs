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
        ReorderSortingLayer();
        TryMove();
        TryAttack();
    }
    
    protected override void DoAttack()
    {
        base.DoAttack();

    }

    protected override void Die()
    {
        throw new System.NotImplementedException();
    }
}
