using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnModePlayerController : MonoBehaviour
{
    [SerializeField] private int playerMoveCount;

    private bool isAreaActive = false;
    private bool isPlayerCanMove = false;
    List<TurnMoveNode> turnMoves;
    Vector2 mousePosition;
    Vector3 targetPosition, prevPosition;

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
        }

        // Player의 이동 가능한 타일이 나타났을 때
        if (isAreaActive)
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition = new Vector2(Mathf.RoundToInt(mousePosition.x), Mathf.RoundToInt(mousePosition.y));

            targetPosition = new Vector3(mousePosition.x, mousePosition.y, 0);

            if (turnMoves == null)
                turnMoves = GameManager.Instance.turnPathFinder.GenerateRoad(transform.position, targetPosition, this.transform);
            else
            {
                Vector3 prevPos = new Vector3(turnMoves[turnMoves.Count - 1].x, turnMoves[turnMoves.Count - 1].y, 0);

                List<TurnMoveNode> tempNodeList = turnMoves;
                if(targetPosition != prevPos)
                {
                    tempNodeList.RemoveAt(tempNodeList.Count - 1);
                    tempNodeList.AddRange(GameManager.Instance.turnPathFinder.GenerateRoad(prevPos, targetPosition, this.transform));
                }
                turnMoves = GameManager.Instance.turnPathFinder.GenerateRoad(transform.position, targetPosition, this.transform);
                if (tempNodeList.Count == turnMoves.Count)
                {
                    turnMoves = tempNodeList;
                }
            }

            //// LineRenderer 설정, 적 이동경로(A* 최단거리 알고리즘)대로 선으로 표시
            LineRenderer lr = this.GetComponent<LineRenderer>();
            lr.positionCount = turnMoves.Count;
            if (turnMoves.Count > playerMoveCount)
                lr.SetColors(new Color(1, 0, 0), new Color(1, 0, 0));
            else
                lr.SetColors(new Color(1, 1, 1), new Color(1, 1, 1));
            for (int i = 0; i < turnMoves.Count; i++)
            {
                lr.SetPosition(i, new Vector3(turnMoves[i].x, turnMoves[i].y));
            }
            StartCoroutine("roadUX");

            if (Input.GetMouseButtonDown(0))
            {
                PlayerMoveAlongPath();
            }
        }
    }

    private void PlayerMoveAlongPath()
    {

    }
    private void ShowPlayerMoveArea()
    {
        isAreaActive = true;
        for (int x = -playerMoveCount; x <= playerMoveCount; x++)
        {
            for (int y = -playerMoveCount; y <= playerMoveCount; y++)
            {
                if (Mathf.Abs(x) + Mathf.Abs(y) > playerMoveCount)
                    continue;
                GameManager.Instance.mapManger.GetComponent<MapManager>().tileMap.SetTile(new Vector3Int((int)transform.position.x + x, (int)transform.position.y + y, 0), GameManager.Instance.mapManger.GetComponent<MapManager>().ckTile);
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
