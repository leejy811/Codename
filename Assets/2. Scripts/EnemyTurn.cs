using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// 의견 : 적 타입에 따라서 나중에 클래스나 인터페이스를 통해서 구현하는게 좋을 것 같아요
//        지금은 하드코딩으로 enemyA,B,C인지 체크하는 방식이라.. 
public enum EnemyType
{
    enemyA,
    enemyB,
    enemyC,
}

partial class EnemyTurn : MonoBehaviour
{
    // 현재 타일맵 정보
    [SerializeField]
    public Tilemap tilemap;

    // 적 타입
    [SerializeField]
    public EnemyType enemyType;

    // 부채꼴 탐색범위
    [SerializeField]
    public GameObject circularSector;

    // 이동범위
    [SerializeField]
    public List<Vector2> enemyMovePos;

    // 이동할 방향
    Vector3 nextMovePos;

    private void Start()
    {
        // Tilemap의 전체 영역을 가져옴
        BoundsInt bounds = tilemap.cellBounds;

        // 영역에 대한 타일 정보를 2차원 배열로 받아옴
        TileBase[] tiles = tilemap.GetTilesBlock(bounds);

        // 배열을 출력하여 타일 정보 확인
        for (int y = bounds.yMin; y < bounds.yMax; y++)
        {
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                TileBase tile = tiles[(x - bounds.xMin) + (y - bounds.yMin) * bounds.size.x];

                if (tile != null)
                {
                    // 타일이 있는 경우에 대한 처리
                    //Debug.Log("Tile at position (" + x + ", " + y + "): " + tilemap.GetTile(new Vector3Int(x, y)).name);
                    
                }
                else
                {
                    // 타일이 없는 경우에 대한 처리
                    //Debug.Log("No tile at position (" + x + ", " + y + ")");
                }
            }
        }

        // Enemy A
        if(enemyType == EnemyType.enemyA) 
        {
            StartCoroutine(enemyA_move());
        }

        // Enemy C
        if (enemyType == EnemyType.enemyC)
        {
            enemyC_move();
        }
    }


}

partial class EnemyTurn
{
    int curPosIdx = 0;
    float velocity = 2f;
    public Tweener tweener;

    IEnumerator enemyA_move()
    {
        // 만약 들켰으면 실행안함
        if (circularSector.GetComponent<CircularSector>().isCollision == true) yield return null;
        else
        {
            nextMovePos = enemyMovePos[(curPosIdx++) % enemyMovePos.Count];
            float duration = Vector2.Distance(transform.position, nextMovePos) / velocity;

            // 부채꼴 크기의 탐색 범위 회전
            Vector3 _dir = nextMovePos - transform.position;
            circularSector.transform.DORotate(new Vector3(0, 0, (Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg)), .3f); // circularSector.transform.rotation = Quaternion.Euler(new Vector3(0, 0, (Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg)));// = Quaternion.Euler(_dir);
                                                                                                                      //Debug.Log(Mathf.Atan2(_dir.y, _dir.x)*Mathf.Rad2Deg+180);

            // 이동 경로를 설정합니다.
            tweener = transform.DOMove(nextMovePos, duration).SetEase(Ease.Linear);



            // 반복
            yield return new WaitForSeconds(duration);

            StartCoroutine(enemyA_move());
        }
    }


    private void enemyC_move(bool isUserTurn=true)
    {

        // 유저 턴인 경우 이동할 경로 표시
        if (isUserTurn)
        {
            int Rand_X = Random.Range((int)this.transform.localPosition.x-3, (int)this.transform.localPosition.x+3);
            int Rand_Y = Random.Range((int)this.transform.localPosition.y-3, (int)this.transform.localPosition.y+3);
            nextMovePos = new Vector2(Rand_X, Rand_Y);

            GenerateRoad();
        }
        // 상대 턴인 경우 이동
        else
        {
            float duration = Vector2.Distance(transform.position, nextMovePos) / velocity;

            // 부채꼴 크기의 탐색 범위 회전
            Vector3 _dir = nextMovePos - transform.position;
            circularSector.transform.DORotate(new Vector3(0, 0, (Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg)), .3f); // circularSector.transform.rotation = Quaternion.Euler(new Vector3(0, 0, (Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg)));// = Quaternion.Euler(_dir);
                                                                                                                      //Debug.Log(Mathf.Atan2(_dir.y, _dir.x)*Mathf.Rad2Deg+180);

            newEnemyRoad.SetActive(false);
            tweener = transform.DOMove(nextMovePos, duration).SetEase(Ease.Linear)
                .OnComplete(() => { 
                    enemyC_move(true);
                    newEnemyRoad.SetActive(true);
                }); // 이동 끝나면 유저턴으로 패스
        }
    }

    public void TurnPass()
    {
        enemyC_move(false);
    }
}

// 적 이동경로 표시
partial class EnemyTurn
{
    [SerializeField] GameObject roadPrefab;
    private GameObject newEnemyRoad;

    private void GenerateRoad()
    {
        newEnemyRoad = Instantiate(roadPrefab,this.transform);
        newEnemyRoad.transform.localPosition = new Vector3(0,0,0);

        GameObject _road = roadPrefab.transform.GetChild(0).GetChild(0).gameObject;
        GameObject _point = roadPrefab.transform.GetChild(0).GetChild(1).gameObject;

        // 시작점 및 끝점
        GameObject startPoint = Instantiate(_point, newEnemyRoad.transform);
        GameObject endPoint = Instantiate(_point, newEnemyRoad.transform);
        startPoint.SetActive(true);
        endPoint.SetActive(true);
        endPoint.transform.position = nextMovePos;


        // 길 경로 및 위치 설정
        float dist = Vector2.Distance(startPoint.transform.localPosition, endPoint.transform.localPosition);
        for (int i = 1; i <= (int)dist; i++)
        {
            GameObject road = Instantiate(_road, newEnemyRoad.transform.GetChild(0));
            road.SetActive(true);
            road.transform.localPosition = new Vector2(startPoint.transform.localPosition.x + i, 0);
            //float dist = Vector2.Distance(startPoint.transform.position, endPoint.transform.position);
        }
        float angle = Mathf.Atan2(endPoint.transform.localPosition.y, endPoint.transform.localPosition.x) * Mathf.Rad2Deg;
        newEnemyRoad.transform.GetChild(0).DORotate(new Vector3(0, 0, angle),0f);

    }
}