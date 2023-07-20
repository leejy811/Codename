using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    enemyA,
    enemyB,
    enemyC,
}
public class TurnModeEnemy : MonoBehaviour
{
    int curPosIdx = 0;
    float velocity = .5f;
    public Tweener tweener;
    // 적 타입
    [SerializeField]
    public EnemyType enemyType;

    // 부채꼴 탐색범위
    [SerializeField]
    public GameObject circularSector;

    // 이동범위
    [SerializeField]
    public List<Vector2> enemyMovePos;
    [SerializeField]
    private GameObject roadPrefab;
    private GameObject startPoint;
    private GameObject endPoint;
    // 이동할 방향
    Vector3 nextMovePos;
    Vector3 prevMovePos;
    List<TurnMoveNode> turnMoves;

    public Sequence s;

    // 들켰는지 여부
    public bool isAlert = false;

    [SerializeField] GameObject alertSprite;

    TurnType turnType, prevTurnType;

    void Start()
    {
        startPoint = roadPrefab.transform.GetChild(1).gameObject;
        endPoint = roadPrefab.transform.GetChild(2).gameObject;
        if (GameManager.Instance.turnManager == null)
            return;
        turnType = GameManager.Instance.turnManager.turnType;
        prevTurnType = turnType;
        EnemyMove(turnType);

    }

    // Update is called once per frame

    private void Update()
    {
        if(GameManager.Instance.turnManager == null)
        {
            turnType = GameManager.Instance.turnManager.turnType;
            prevTurnType = turnType;
            EnemyMove(turnType);
        }
    }

    public void EnemyMove(TurnType turnType)
    {
        if (enemyType == EnemyType.enemyA)
        {
            enemyA_move(turnType);
        }
        // Enemy C
        if (enemyType == EnemyType.enemyC)
        {
            enemyC_move(turnType);
        }
        StartCoroutine(roadUX());
    }
    private void enemyA_move(TurnType turnType)
    {
        // 만약 들켰으면 실행안함
        //if (circularSector.GetComponent<CircularSector>().isCollision == true) { return; }

        // 유저 턴인 경우  경로 표시
        if (turnType == TurnType.player)
        {
            nextMovePos = transform.position + new Vector3(enemyMovePos[(curPosIdx) % enemyMovePos.Count].x, enemyMovePos[(curPosIdx) % enemyMovePos.Count].y, 0);

            startPoint.transform.position = transform.position;
            endPoint.transform.position = nextMovePos;
;
            turnMoves = GameManager.Instance.turnPathFinder.GenerateRoad(transform.position, nextMovePos, this.transform);
            curPosIdx++;
            LineRenderer lr = this.GetComponent<LineRenderer>();
            lr.positionCount = turnMoves.Count;

            for (int i = 0; i < turnMoves.Count; i++)
            {
                lr.SetPosition(i, new Vector3(turnMoves[i].x, turnMoves[i].y));
            }
            StartCoroutine("roadUX");

        }
        // 상대 턴인 경우 이동
        else
        {
            s = DOTween.Sequence();
            for (int i = 0; i < turnMoves.Count; i++)
            {
                nextMovePos = new Vector3(turnMoves[i].x, turnMoves[i].y);
                if (i != 0) prevMovePos = new Vector3(turnMoves[i - 1].x, turnMoves[i - 1].y);

                // 부채꼴 크기의 탐색 범위 회전
                Vector3 _dir = nextMovePos - prevMovePos;
                s.Append(transform.DOMove(nextMovePos, velocity).SetEase(Ease.Linear)).Join(circularSector.transform.DORotate(new Vector3(0, 0, (Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg)), .3f));


            }

            // 이동 완료시
            s.Play().OnComplete(() =>
            {
                // 이동경로 끄고 다음 이동경로 설정
                GameObject newEnemyRoad = transform.Find("road(Clone)").gameObject;
                newEnemyRoad.SetActive(false);

                GetComponent<LineRenderer>().positionCount = 0;
                this.transform.GetComponent<TurnModeEnemy>().enemyA_move(TurnType.player); // 유저턴으로 패스
            });

        }
    }
    private void enemyC_move(TurnType turnType)
    {
        if (this.enemyType != EnemyType.enemyC) return;

        // 유저 턴인 경우 이동할 경로 표시
        if (turnType == TurnType.player)
        {
            int Rand_X = Random.Range((int)this.transform.position.x - 3, (int)this.transform.position.x + 3);
            int Rand_Y = Random.Range((int)this.transform.position.y - 3, (int)this.transform.position.y + 3);
            
            nextMovePos = new Vector2(Rand_X, Rand_Y);

            turnMoves = GameManager.Instance.turnPathFinder.GenerateRoad(transform.position, nextMovePos, this.transform);
            startPoint.transform.position = Vector3.zero;
            endPoint.transform.position = new Vector3(nextMovePos.x, nextMovePos.y, 0);

            LineRenderer lr = this.GetComponent<LineRenderer>();
            lr.positionCount = turnMoves.Count;

            for (int i = 0; i < turnMoves.Count; i++)
            {
                lr.SetPosition(i, new Vector3(turnMoves[i].x, turnMoves[i].y));
            }
        }
        // 상대 턴인 경우 이동
        else
        {
            s = DOTween.Sequence();
            for (int i = 0; i < turnMoves.Count; i++)
            {
                nextMovePos = new Vector3(turnMoves[i].x, turnMoves[i].y);
                if (i != 0) prevMovePos = new Vector3(turnMoves[i - 1].x, turnMoves[i - 1].y);

                // 부채꼴 크기의 탐색 범위 회전
                Vector3 _dir = nextMovePos - prevMovePos;
                s.Append(transform.DOMove(nextMovePos, velocity).SetEase(Ease.Linear)).Join(circularSector.transform.DORotate(new Vector3(0, 0, (Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg)), .3f));
                //circularSector.transform.DORotate(new Vector3(0, 0, (Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg)), .3f);

            }

            // 이동 완료시
            s.Play().OnComplete(() =>
            {
                // 이동경로 끄고 다음 이동경로 설정
                GameObject newEnemyRoad = transform.Find("road(Clone)").gameObject;
                newEnemyRoad.SetActive(false);
                GetComponent<LineRenderer>().positionCount = 0;
                this.transform.GetComponent<TurnModeEnemy>().enemyC_move(TurnType.player); // 유저턴으로 패스
            });

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
