using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;
using MoreMountains.Tools;
using MoreMountains.InventoryEngine;

public class PlayerStatus : MonoBehaviour, MMEventListener<MMInventoryEvent>
{
    CharacterMovement movement;
    CharacterInventory inventory;

    private void Start()
    {
        movement = gameObject.GetComponent<CharacterMovement>();
        inventory = gameObject.GetComponent<CharacterInventory>();
    }

    private void ApplyZigzagItem(bool enable)
    {
        movement.MovementSpeed *= enable ? -2f : -0.5f;
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
            }
		}
        else if (inventoryEvent.InventoryEventType == MMInventoryEventType.Drop)
        {
            switch (inventoryEvent.EventItem.ItemID)
            {
                case "Zigzag":
                    ApplyZigzagItem(true);
                    break;
            }
        }
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
