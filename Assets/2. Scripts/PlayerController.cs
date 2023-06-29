using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Linq;
using UnityEngine.Tilemaps;

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
    [SerializeField] private int playerMoveCount;
    // 이 부분 수정
    [SerializeField] private Tile areaTile;
    [SerializeField] private Tilemap tileMap;

    private float moveX;
    private float moveY;
    private bool isRolling = false;
    private bool isAreaActive = false;
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

        ReorderSortingLayer();
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
                if(!isAreaActive)
                    ShowPlayerMoveArea();
            }
        }
        if(isAreaActive)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition = new Vector2(Mathf.RoundToInt(mousePosition.x) - 0.6f, Mathf.RoundToInt(mousePosition.y) + 0.7f);

            if(Input.GetMouseButtonDown(0))
                transform.position = mousePosition;

            
        }
    }
       
    private void PlayerMove()
    { 
    }
    private void ShowPlayerMoveArea()
    {
        isAreaActive = true;
        for(int x = -playerMoveCount; x<=playerMoveCount; x++)
        {
            for(int y = -playerMoveCount; y<=playerMoveCount; y++)
            {
                if (Mathf.Abs(x) + Mathf.Abs(y) > playerMoveCount)
                    continue;
                tileMap.SetTile(new Vector3Int(x, y, 0), areaTile);
            }
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
        var roll = transform.DOMove(new Vector3(moveX, moveY, 0).normalized * rollDistance, 0.5f).SetRelative().SetEase(Ease.OutQuad);
        yield return roll.WaitForCompletion();

        PlayerState = PlayerStates.Idle;
        isRolling = false;
    }
}
