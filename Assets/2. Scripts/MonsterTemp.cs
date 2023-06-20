using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MonsterTemp : MonsterController
{
    
    int x = 4;
    int y = 8;
    bool ismoving=false;
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

    private void TryMove()
    {
        if (ismoving)
            return;
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        ismoving = true;
        var d= PathFinder.GetDir(new int[] { x, y }, 3, 3);
        var tween = transform.DOMove(new Vector3(d[1], -d[0], 0), 1f).SetRelative().SetEase(Ease.Linear);
        x += d[0];
        y += d[1];
        
        yield return tween.WaitForCompletion();
        ismoving = false;
    }
}
