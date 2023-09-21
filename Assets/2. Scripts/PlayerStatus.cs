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
    DamageResistance damageRatio;

    bool hasReaction;

    Coroutine damageUpCoroutine;

    private void Start()
    {
        movement = gameObject.GetComponent<CharacterMovement>();
        inventory = gameObject.GetComponent<CharacterInventory>();
        damageRatio = gameObject.GetComponent<DamageResistance>();
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

    IEnumerator DamageUp()
    {
        ApplyDamageUp(true);

        yield return new WaitForSeconds(3f);

        ApplyDamageUp(false);
    }

    private void ApplyDamageUp(bool enable)
    {
        foreach (DungeonRoom room in RoomController.Instance.loadedRooms)
        {
            if (room.roomType == "Single") continue;

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
