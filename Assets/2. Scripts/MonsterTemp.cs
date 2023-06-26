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

    public override void Attack()
    {
        base.Attack();

    }

    private void Update()
    {
        TryMove();
    }

}
