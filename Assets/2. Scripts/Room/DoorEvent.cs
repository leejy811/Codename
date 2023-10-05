using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;

public class DoorEvent : MonoBehaviour
{

    public void DoorExit()
    {
        if (transform.parent.GetComponent<Door>().nextRoom.GetComponent<DungeonRoom>().roomName == "Boss")
        {
            transform.parent.GetComponent<Door>().nextRoom.GetComponent<DungeonRoom>().childRooms.gameObject.SetActive(false);
            gameObject.GetComponentInParent<DungeonRoom>().childRooms.gameObject.SetActive(false);
            RoomPrefabsSet.Instance.realBossRoom.SetActive(true);
            RoomPrefabsSet.Instance.realBossRoom.GetComponent<Room>().CinemachineCameraConfiner.m_BoundingVolume = RoomPrefabsSet.Instance.realBossRoom.GetComponent<Room>().Confiner;
        }
        else
            transform.parent.GetComponent<Door>().ChangeCurrentRoom();

        LevelManager.Instance.Players[0].gameObject.GetComponent<CharacterMovement>().MovementSpeedMultiplier = 1;
        LevelManager.Instance.Players[0].gameObject.GetComponent<Character>().ConditionState.ChangeState(CharacterStates.CharacterConditions.Normal);
    }

    public void DoorEnter()
    {
        LevelManager.Instance.Players[0].gameObject.GetComponent<CharacterMovement>().MovementSpeedMultiplier = 0;
        LevelManager.Instance.Players[0].gameObject.GetComponent<Character>().ConditionState.ChangeState(CharacterStates.CharacterConditions.Frozen);
    }
}
