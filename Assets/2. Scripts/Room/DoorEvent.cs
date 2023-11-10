using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;
using MoreMountains.Tools;

public class DoorEvent : MonoBehaviour
{
    [SerializeField] GameObject doorLight;
    [SerializeField] AudioClip bossBgm;

    public void DoorExit()
    {
        if (transform.parent.GetComponent<Door>().nextRoom.GetComponent<DungeonRoom>().roomName == "Boss")
        {
            BossDoorExit();
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

    public void DoorLightControl(bool enable)
    {
        doorLight.SetActive(enable);
    }

    public void BossDoorExit()
    {
        transform.parent.GetComponent<Door>().nextRoom.GetComponent<DungeonRoom>().childRooms.gameObject.SetActive(false);
        gameObject.GetComponentInParent<DungeonRoom>().childRooms.gameObject.SetActive(false);
        RoomPrefabsSet.Instance.realBossRoom.SetActive(true);
        RoomPrefabsSet.Instance.realBossRoom.GetComponent<Room>().CinemachineCameraConfiner.m_BoundingVolume = RoomPrefabsSet.Instance.realBossRoom.GetComponent<Room>().Confiner;

        MMSoundManagerPlayOptions options = MMSoundManagerPlayOptions.Default;
        options.ID = 255;
        options.Loop = true;
        options.Location = Vector3.zero;
        options.MmSoundManagerTrack = MMSoundManager.MMSoundManagerTracks.Music;

        MMSoundManager.Instance.StopSound(MMSoundManager.Instance.FindByID(255));
        MMSoundManagerSoundPlayEvent.Trigger(bossBgm, options);
    }
}
