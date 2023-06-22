using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Linq;
using UnityEngine.Tilemaps;

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
    [SerializeField] private Tile areaTile;
    [SerializeField] private Tilemap tileMap;

    private float moveX;
    private float moveY;
    private bool isDead = false;
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
            mousePosition = new Vector2Int(Mathf.RoundToInt(mousePosition.x),Mathf.RoundToInt(mousePosition.y));

            transform.position = mousePosition;

            Debug.Log(tileMap.GetTile(new Vector3Int(Mathf.RoundToInt(mousePosition.x), Mathf.RoundToInt(mousePosition.y), 0)), areaTile);
        }
    }
    private static RaycastHit2D? GetFocusedOnTile()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);

        if (hits.Length > 0)
        {
            return hits.OrderByDescending(i => i.collider.transform.position.z).First();
        }

        return null;
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
