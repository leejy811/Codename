using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEvent : MonoBehaviour
{
    public void ImmunePlayer()
    {
        LevelManager.Instance.Players[0].GetComponent<Health>().ImmuneToDamage = true;
    }
}
