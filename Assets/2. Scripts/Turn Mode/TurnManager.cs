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
    public TurnType turnType = TurnType.player;
    [SerializeField]
    public int playerTurnCount;

    private void Start()
    {
        GameManager.Instance.player.GetComponent<TurnModePlayerController>().playerMoveCount = playerTurnCount;
    }
    public void PressTurnPassButton()
    {
        if(turnType == TurnType.enemy)
        {
            GameManager.Instance.player.GetComponent<TurnModePlayerController>().playerMoveCount = playerTurnCount;
            turnType = TurnType.player;
            GameManager.Instance.enemyManager.Enemy1111(turnType);

        }
        else
        {
            turnType = TurnType.enemy;
            GameManager.Instance.enemyManager.Enemy1111(turnType);
        }

    }
}
