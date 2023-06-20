using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MonsterTemp : MonsterController
{
    int[,] map = {
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,1,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }
        };
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
        var d= PathFinder.GetDir(map, new int[] { 2, 2 }, new int[] { x, y }, 3, 3);
        var tween = transform.DOMove(new Vector3(d[1], -d[0], 0), 1f).SetRelative().SetEase(Ease.Linear);
        x += d[0];
        y += d[1];
        Debug.Log(x + "," + y);
        yield return tween.WaitForCompletion();
        ismoving = false;
    }
}
