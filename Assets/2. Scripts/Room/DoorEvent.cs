using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;

public class DoorEvent : MonoBehaviour
{

    public void DoorExit()
    {
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
