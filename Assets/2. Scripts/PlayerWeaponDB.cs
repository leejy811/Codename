using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;
using MoreMountains.Tools;
using MoreMountains.InventoryEngine;
using System;

public class PlayerWeaponDB : MonoBehaviour, MMEventListener<MMInventoryEvent>
{
    DataBaseManager DBManager;
    CharacterHandleWeapon weapon;
    Dictionary<string, WeaponDBEntity> weaponDB = new Dictionary<string, WeaponDBEntity>();

    private void Start()
    {
        DBManager = DataBaseManager.instance;
        weapon = gameObject.GetComponent<CharacterHandleWeapon>();
        InitWeaponDB();
    }

    private void InitWeaponDB()
    {
        foreach(WeaponDBEntity weaponEntity in DBManager.data.WeaponDB)
        {
            weaponDB[weaponEntity.weaponID] = weaponEntity;
        }
    }

    public void OnMMEvent(MMInventoryEvent inventoryEvent)
    {
        if (inventoryEvent.InventoryEventType == MMInventoryEventType.ItemEquipped)
        {
            EqipWeapon(true, inventoryEvent.EventItem.ItemID);
        }
        else if (inventoryEvent.InventoryEventType == MMInventoryEventType.Pick)
        {
            if (inventoryEvent.EventItem.ItemClass == ItemClasses.Weapon)
            {
                inventoryEvent.EventItem.Description = "Attack Damage : " + weaponDB[inventoryEvent.EventItem.ItemID].damage +
                                                                                "\nReload Time : " + weaponDB[inventoryEvent.EventItem.ItemID].reloadSpeed +
                                                                                "\nMagazine Size : " + weaponDB[inventoryEvent.EventItem.ItemID].magazine +
                                                                                "\nTime Between Use : " + weaponDB[inventoryEvent.EventItem.ItemID].useTime +
                                                                                "\nBullet Speed : " + weaponDB[inventoryEvent.EventItem.ItemID].bulletSpeed;
            }
        }
    }

    private void EqipWeapon(bool enable, string weaponID)
    {
        weapon.CurrentWeapon.GetComponent<ProjectileWeapon>().weapon_damage = weaponDB[weaponID].damage;
        weapon.CurrentWeapon.GetComponent<ProjectileWeapon>().ReloadTime = weaponDB[weaponID].reloadSpeed;
        weapon.CurrentWeapon.GetComponent<ProjectileWeapon>().MagazineSize = weaponDB[weaponID].magazine;
        weapon.CurrentWeapon.GetComponent<ProjectileWeapon>().TimeBetweenUses = weaponDB[weaponID].useTime;
        weapon.CurrentWeapon.GetComponent<ProjectileWeapon>().weapon_speed = weaponDB[weaponID].bulletSpeed;
    }

    private void OnEnable()
    {
        this.MMEventStartListening<MMInventoryEvent>();
    }

    private void OnDisable()
    {
        this.MMEventStopListening<MMInventoryEvent>();
    }
}
