using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TurnModePlayerController : MonoBehaviour
{
    [SerializeField] private int playerMoveCount;
    [SerializeField] private GameObject playerMoveRoad;
    private GameObject startPoint;
    private GameObject endPoint;

    private bool isAreaActive = false;
    private bool isPlayerCanMove = false;
    List<TurnMoveNode> turnMoves;
    LineRenderer lr;
    Vector2 mousePosition;
    Vector2 prevPlayerPos;

    Vector3 targetPosition, prevPos;

    private void Start()
    {
        startPoint = playerMoveRoad.transform.GetChild(1).gameObject;
        endPoint = playerMoveRoad.transform.GetChild(2).gameObject;
        //// LineRenderer 설정, 적 이동경로(A* 최단거리 알고리즘)대로 선으로 표시
        lr = this.GetComponent<LineRenderer>();
        startPoint.SetActive(true);
        endPoint.SetActive(true);
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);

            if (hit.collider != null)
            {
                Debug.Log(hit.transform.gameObject);
                if (!isAreaActive)
                    ShowPlayerMoveArea();
            }

            if (isAreaActive && isPlayerCanMove)
            {
                //PlayerMoveAlongPath();
                StartCoroutine("PlayerMovePath");
                isAreaActive = false;
            }
        }
        // Player의 이동 가능한 타일이 나타났을 때
        if (isAreaActive)
        {
            CheckPlayerMove();
        }


    }
    private void CheckPlayerMove()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition = new Vector2(Mathf.RoundToInt(mousePosition.x), Mathf.RoundToInt(mousePosition.y));
        targetPosition = new Vector3(mousePosition.x, mousePosition.y, 0);
        if (transform.position == targetPosition)
            return;
        endPoint.transform.position = targetPosition;
        lr.enabled = true;

        if (turnMoves == null)
        {
            turnMoves = GameManager.Instance.turnPathFinder.GenerateRoad(transform.position, targetPosition, this.transform);
            prevPos = targetPosition;

        }
        else
        {
            List<TurnMoveNode> tempNodeList = turnMoves;

            if (targetPosition != prevPos)
            {
                tempNodeList.RemoveAt(tempNodeList.Count - 1);
                tempNodeList.AddRange(GameManager.Instance.turnPathFinder.GenerateRoad(prevPos, targetPosition, this.transform));
            }
            turnMoves = GameManager.Instance.turnPathFinder.GenerateRoad(transform.position, targetPosition, this.transform);
            if (tempNodeList.Count == turnMoves.Count)
            {
                turnMoves = tempNodeList;
            }
            prevPos = targetPosition;
        }
        endPoint.SetActive(true);

        lr.positionCount = turnMoves.Count;
        if (turnMoves.Count > playerMoveCount + 1)
            isPlayerCanMove = false;
        else
            isPlayerCanMove = true;
        //if (turnMoves.Count > playerMoveCount + 1)
        //    lr.SetColors(new Color(1, 0, 0), new Color(1, 0, 0));
        //else
        //    lr.SetColors(new Color(1, 1, 1), new Color(1, 1, 1));
        for (int i = 0; i < turnMoves.Count; i++)
        {
            lr.SetPosition(i, new Vector3(turnMoves[i].x, turnMoves[i].y));
        }


    }
    IEnumerator PlayerMovePath()
    {
        for (int x = -playerMoveCount; x <= playerMoveCount; x++)
        {
            for (int y = -playerMoveCount; y <= playerMoveCount; y++)
            {
                if (Mathf.Abs(x) + Mathf.Abs(y) > playerMoveCount)
                    continue;
                GameManager.Instance.mapManger.GetComponent<MapManager>().tileMap.SetTile(new Vector3Int((int)prevPlayerPos.x + x, (int)prevPlayerPos.y + y, 0), GameManager.Instance.mapManger.GetComponent<MapManager>().roomTile);
            }
        }
        isAreaActive = false;
        for (int i = 0; i < turnMoves.Count; i++)
        {
            transform.DOMove(new Vector3(turnMoves[i].x, turnMoves[i].y, 0), 1f);
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(2f);
        isPlayerCanMove = false;
        endPoint.SetActive(false);
        lr.enabled = false;
    }
    private void PlayerMoveAlongPath()
    {
        for(int i=0; i<turnMoves.Count; i++)
        {
            transform.DOMove(new Vector3(turnMoves[i].x, turnMoves[i].y, 0), 0.1f);
        }
    }
    private void ShowPlayerMoveArea()
    {
        isAreaActive = true;
        prevPlayerPos = transform.position;
        for (int x = -playerMoveCount; x <= playerMoveCount; x++)
        {
            for (int y = -playerMoveCount; y <= playerMoveCount; y++)
            {
                if (Mathf.Abs(x) + Mathf.Abs(y) > playerMoveCount)
                    continue;
                GameManager.Instance.mapManger.GetComponent<MapManager>().tileMap.SetTile(new Vector3Int((int)prevPlayerPos.x + x, (int)prevPlayerPos.y + y, 0), GameManager.Instance.mapManger.GetComponent<MapManager>().ckTile);
            }
        }
    }
    IEnumerator roadUX()
    {
        LineRenderer lr = this.GetComponent<LineRenderer>();
        lr.material.SetTextureOffset("_MainTex", new Vector2(lr.material.GetTextureOffset("_MainTex").x - 0.1f, 0f));
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(roadUX());
    }
}
