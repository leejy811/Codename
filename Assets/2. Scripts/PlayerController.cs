using UnityEngine;
using DG.Tweening;
using System.Collections;

public class PlayerController : MonoBehaviour
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

    [SerializeField] private float playerHp;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rollDistance;
    [SerializeField] private int playerMoveCount;

    private float moveX;
    private float moveY;
    private bool isDead = false;
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
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);

            if (hit.collider != null)
            {
                Debug.Log(hit.transform.gameObject);
                Debug.Log(hit.transform.gameObject.tag);
                
            }
        }
    }

    private void TryMove()
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
        isRolling = true;
        StartCoroutine(Roll());
    }

    IEnumerator Roll()
    {
        var roll = transform.DOMove(new Vector3(moveX, moveY, 0).normalized * rollDistance, 0.5f).SetRelative().SetEase(Ease.OutQuad);
        yield return roll.WaitForCompletion();

        PlayerState = PlayerStates.Idle;
        isRolling = false;
    }
}
