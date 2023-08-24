using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathEvent : MonoBehaviour
{
    public void RemoveEnemyInList()
    {
        DungeonRoom parent = GetComponentInParent<DungeonRoom>();
        parent.RemoveEnemy(gameObject);
    }
}
