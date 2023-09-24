using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;
using MoreMountains.Tools;
using MoreMountains.InventoryEngine;

public class PlayerStatus : MonoBehaviour, MMEventListener<MMInventoryEvent>, MMEventListener<MMDamageTakenEvent>
{
    CharacterMovement movement;
    CharacterInventory inventory;
    CharacterHandleWeapon weapon;
    DamageResistance damageRatio;

    bool hasReaction;
    bool hasCushion;
    bool hasSniper;

    Coroutine damageUpCoroutine;

    private void Start()
    {
        movement = gameObject.GetComponent<CharacterMovement>();
        inventory = gameObject.GetComponent<CharacterInventory>();
        damageRatio = gameObject.GetComponent<DamageResistance>();
        weapon = gameObject.GetComponent<CharacterHandleWeapon>();
    }

    private void ApplyZigzagItem(bool enable)
    {
        movement.MovementSpeed *= enable ? -2f : -0.5f;
    }

    private void ApplyReactionItem(bool enable)
    {
        damageRatio.DamageMultiplier = enable ? 2f : 1f;
        hasReaction = enable;
    }
    
    private void ApplyCushionItem(bool enable)
    {
        if (weapon.CurrentWeapon == null) return;
        if (weapon.CurrentWeapon.gameObject.GetComponent<ProjectileWeapon>() == null) return;
        if (weapon.CurrentWeapon.gameObject.GetComponent<MMSimpleObjectPooler>() == null) return;

        weapon.CurrentWeapon.MagazineSize = enable ? (int)(weapon.CurrentWeapon.MagazineSize * 0.8f) : (int)(weapon.CurrentWeapon.MagazineSize * 1.25f);

        List<GameObject> projectiles =  weapon.CurrentWeapon.gameObject.GetComponent<MMSimpleObjectPooler>().GetAllPooledObject();

        foreach(GameObject projectile in projectiles)
        {
            projectile.GetComponent<BouncyProjectile>().AmountOfBounces = enable ? new Vector2Int(2, 4) : new Vector2Int(0, 0);
        }
    }

    private void ApplySniperItem(bool enable)
    {
        hasSniper = enable;

        ApplyDamageUp(true);
        ApplyShootTime(enable);
    }

    private void ApplyShootTime(bool enable)
    {
        if (weapon.CurrentWeapon == null) return;
        if (weapon.CurrentWeapon.gameObject.GetComponent<ProjectileWeapon>() == null) return;
        if (weapon.CurrentWeapon.gameObject.GetComponent<MMSimpleObjectPooler>() == null) return;

        weapon.CurrentWeapon.TimeBetweenUses *= enable ? 1.25f : 0.8f;
    }


    IEnumerator DamageUp()
    {
        ApplyDamageUp(true);

        yield return new WaitForSeconds(3f);

        ApplyDamageUp(false);
    }

    private void ApplyDamageUp(bool enable)
    {
        foreach(DungeonRoom room in RoomController.Instance.loadedRooms)
        {
            if (room.roomType == "Single") return;

            foreach (GameObject enemy in room.enemyList)
            {
                enemy.GetComponent<DamageResistance>().DamageMultiplier = enable ? 2f : 1f;
            }
        }
    }

    public void OnMMEvent(MMInventoryEvent inventoryEvent)
    {
		if (inventoryEvent.InventoryEventType == MMInventoryEventType.Pick)
		{
            switch (inventoryEvent.EventItem.ItemID)
            {
                case "Zigzag":
                    ApplyZigzagItem(true);
                    break;
                case "Reaction":
                    ApplyReactionItem(true);
                    break;
                case "Cushion":
                    hasCushion = true;
                    ApplyCushionItem(true);
                    break;
                case "Sniper":
                    ApplySniperItem(true);
                    break;
            }
		}
        else if (inventoryEvent.InventoryEventType == MMInventoryEventType.Drop)
        {
            switch (inventoryEvent.EventItem.ItemID)
            {
                case "Zigzag":
                    ApplyZigzagItem(false);
                    break;
                case "Reaction":
                    ApplyReactionItem(false);
                    break;
                case "Cushion":
                    hasCushion = false;
                    ApplyCushionItem(false);
                    break;
                case "Sniper":
                    ApplySniperItem(false);
                    break;
            }
        }
        else if(inventoryEvent.InventoryEventType == MMInventoryEventType.ItemEquipped)
        {
            if (hasCushion)
            {
                ApplyCushionItem(true);
            }
            if(hasSniper)
            {
                ApplyShootTime(true);
            }
        }
        else if (inventoryEvent.InventoryEventType == MMInventoryEventType.EquipRequest || inventoryEvent.InventoryEventType == MMInventoryEventType.UnEquipRequest)
        {
            if (hasCushion)
            {
                ApplyCushionItem(false);
            }
            if (hasSniper)
            {
                ApplyShootTime(false);
            }
        }
    }

    public void OnMMEvent(MMDamageTakenEvent damageTakenEvent)
    {
        if (hasReaction && damageTakenEvent.AffectedHealth.tag ==  "Player")
        {
            if(damageUpCoroutine != null)
                StopCoroutine(damageUpCoroutine);

            damageUpCoroutine = StartCoroutine(DamageUp());
        }
    }


    private void OnEnable()
	{
		this.MMEventStartListening<MMInventoryEvent>();
		this.MMEventStartListening<MMDamageTakenEvent>();
    }

	private void OnDisable()
	{
		this.MMEventStopListening<MMInventoryEvent>();
		this.MMEventStopListening<MMDamageTakenEvent>();
    }
}
