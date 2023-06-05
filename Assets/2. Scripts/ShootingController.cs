using System.Collections.Generic;
using UnityEngine;

public class ShootingController : MonoBehaviour
{
    public enum PlayerStates
    {
        Idle,
        Move,
        Roll,
        Dead,
    }
    public PlayerStates PlayerState { get; private set; } = PlayerStates.Idle;

    [SerializeField] private float moveSpeed;

    private float moveX;
    private float moveY;
    private bool isDead = false;


    void Start()
    {
        PlayerState = PlayerStates.Idle;
    }

    void Update()
    {
        if (isDead)
            return;

        PlayerState = PlayerStates.Idle;
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
        }
    }

    private void TryMove()
    {
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
            return;

        PlayerState = PlayerStates.Move;
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");
    }

    private void Move()
    {
        Vector3 moveVelocity = new Vector3(moveX, moveY, 0).normalized * moveSpeed* Time.deltaTime;
        transform.position += moveVelocity;
    }

    private void TryRoll()
    {

    }
}
