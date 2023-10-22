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
        else if (inventoryEvent.InventoryEventType == MMInventoryEventType.EquipRequest || inventoryEvent.InventoryEventType == MMInventoryEventType.UnEquipRequest)
        {
            EqipWeapon(false, inventoryEvent.EventItem.ItemID);
        }
    }

    private void EqipWeapon(bool enable, string weaponID)
    {
        weapon.CurrentWeapon.GetComponent<ProjectileWeapon>().weapon_damage *= enable ? weaponDB[weaponID].damage : 1 / weaponDB[weaponID].damage;
        weapon.CurrentWeapon.GetComponent<ProjectileWeapon>().ReloadTime *= enable ? weaponDB[weaponID].reloadSpeed : 1 / weaponDB[weaponID].reloadSpeed;
        weapon.CurrentWeapon.GetComponent<ProjectileWeapon>().MagazineSize *= enable ? weaponDB[weaponID].magazine : 1 / weaponDB[weaponID].magazine;
        weapon.CurrentWeapon.GetComponent<ProjectileWeapon>().TimeBetweenUses *= enable ? weaponDB[weaponID].useTime : 1 / weaponDB[weaponID].useTime;
        //weapon.CurrentWeapon.GetComponent<ProjectileWeapon>().bulletSpeed *= weaponDB[weaponID].bulletSpeed;
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
