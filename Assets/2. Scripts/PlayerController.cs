using UnityEngine;
using DG.Tweening;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : ActiveObject
{
    #region Member Var
    public enum PlayerStates
    {
        Idle,
        Move,
        Roll,
        Dead,
    }
    public  static PlayerStates PlayerState { get; private set; } = PlayerStates.Idle;

    [SerializeField] private float rollDistance;

    private float moveX;
    private float moveY;
    private bool isRolling = false;

    #endregion

    void Start()
    {
        PlayerState = PlayerStates.Idle;
    }

    void Update()
    {
        if (isDead)
            return;

        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");

        //ReorderSortingLayer();
        TryMove();
        TryRoll();

        switch (PlayerState)
        {
            case PlayerStates.Idle:
                break;
            case PlayerStates.Move:
                Move();
                break;
            case PlayerStates.Dead:
                break;
            default:
                break;
        }
    }
       

    
    protected override void TryMove()
    {
        if (moveX == 0 && moveY == 0)
        {
            PlayerState = PlayerStates.Idle;
            return;
        }
        if (isRolling)
            return;

        PlayerState = PlayerStates.Move;
    }

    private void Move()
    {
        Vector3 moveVelocity = new Vector3(moveX, moveY, 0).normalized * moveSpeed * Time.deltaTime;
        transform.position += moveVelocity;
    }

    private void TryRoll()
    {
        if (!Input.GetKeyDown(KeyCode.Space) || isRolling)
            return;
        PlayerState = PlayerStates.Roll;
        StartCoroutine(Roll());
    }

    IEnumerator Roll()
    {
        isRolling = true;
        iFrame = true;
        var roll = transform.DOMove(new Vector3(moveX, moveY, 0).normalized * rollDistance, 0.5f).SetRelative().SetEase(Ease.OutQuad);
        yield return roll.WaitForCompletion();

        PlayerState = PlayerStates.Idle;
        isRolling = false;
        iFrame = false;
    }

    protected override void Die()
    {
        throw new System.NotImplementedException();
    }
}
