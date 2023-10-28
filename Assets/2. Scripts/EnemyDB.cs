using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyDB : MonoBehaviour
{
    [SerializeField] private int index;

    bool isApplyData = false;
    bool isApplyWeaponData = false;

    private void Update()
    {
        EnemyDBEntity enemyDB = DataBaseManager.instance.data.EnemyDB[index];

        if (!isApplyData)
        {
            gameObject.GetComponent<Health>().MaximumHealth = enemyDB.health;
            gameObject.GetComponent<Health>().InitialHealth = enemyDB.health;
            gameObject.GetComponent<CharacterMovement>().MovementSpeed = enemyDB.speed;
            isApplyData = true;
        }

        if (!isApplyWeaponData && gameObject.GetComponent<CharacterHandleWeapon>().CurrentWeapon != null)
        {
            if(gameObject.GetComponent<CharacterHandleWeapon>().CurrentWeapon.gameObject.GetComponent<ProjectileWeapon>() != null)
            {
                gameObject.GetComponent<CharacterHandleWeapon>().CurrentWeapon.gameObject.GetComponent<ProjectileWeapon>().weapon_damage = enemyDB.damage;
                gameObject.GetComponent<CharacterHandleWeapon>().CurrentWeapon.gameObject.GetComponent<ProjectileWeapon>().TimeBetweenUses = enemyDB.useTime;
                isApplyWeaponData = true;
            }
        }
    }
}
