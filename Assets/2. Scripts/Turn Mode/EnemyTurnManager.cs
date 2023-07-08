using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurnManager : MonoBehaviour
{
    private static EnemyTurnManager instance = null;

    void Awake()
    {
        instance = this;
    }

    public static EnemyTurnManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }


    public void AllEnemyDetectPlayer()
    {
        for(int i = 0; i < this.transform.childCount; i++)
        {
            this.transform.GetChild(i).GetComponent<EnemyTurn>().DetectPlayer();
        }
    }
}
