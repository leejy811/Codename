using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurnType
{
    player,
    enemy
}

public class TurnManager : MonoBehaviour
{
    public TurnType turnType;
    [SerializeField]
    public int playerTurnCount;

    public void PressTurnPassButton()
    {
        if(turnType == TurnType.enemy)
        {
            GameManager.Instance.player.GetComponent<TurnModePlayerController>().playerMoveCount = playerTurnCount;
            turnType = TurnType.player;
        }
        else
            turnType = TurnType.enemy;

    }
}
