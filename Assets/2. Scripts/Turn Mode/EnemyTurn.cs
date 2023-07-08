using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// 의견 : 적 타입에 따라서 나중에 클래스나 인터페이스를 통해서 구현하는게 좋을 것 같아요
//        지금은 하드코딩으로 enemyA,B,C인지 체크하는 방식이라.. 
//public enum EnemyType
//{
//    enemyA,
//    enemyB,
//    enemyC,
//}

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
    Vector3 prevMovePos;
    public Sequence s;

    // 들켰는지 여부
    public bool isAlert = false;
    [SerializeField] GameObject alertSprite;

    // 길 담는 리스트
    GameObject RoadList;

    private void Start()
    {
        RoadList = GameObject.Find("RoadList");

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
            enemyA_move();
        }

        // Enemy C
        if (enemyType == EnemyType.enemyC)
        {
            enemyC_move();
        }

        StartCoroutine(roadUX());
    }

    
}

partial class EnemyTurn
{
    int curPosIdx = 0;
    float velocity = .5f;
    public Tweener tweener;

    private void enemyA_move(bool isUserTurn=true)
    {
        // 만약 들켰으면 실행안함
        //if (circularSector.GetComponent<CircularSector>().isCollision == true) { return; }

        // 유저 턴인 경우  경로 표시
        if (isUserTurn)
        {
            nextMovePos = enemyMovePos[(curPosIdx++) % enemyMovePos.Count];

            GenerateRoad();
        }
        // 상대 턴인 경우 이동
        else
        {
            s = DOTween.Sequence();
            for (int i = 0; i < FinalNodeList.Count; i++)
            {
                nextMovePos = new Vector3(FinalNodeList[i].x, FinalNodeList[i].y);
                if (i != 0) prevMovePos = new Vector3(FinalNodeList[i - 1].x, FinalNodeList[i - 1].y);

                // 부채꼴 크기의 탐색 범위 회전
                Vector3 _dir = nextMovePos - prevMovePos;
                s.Append(transform.DOMove(nextMovePos, velocity).SetEase(Ease.Linear)).Join(circularSector.transform.DORotate(new Vector3(0, 0, (Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg)), .3f));


            }

            // 이동 완료시
            s.Play().OnComplete(() => {
                // 이동경로 끄고 다음 이동경로 설정
                newEnemyRoad.SetActive(false);
                GetComponent<LineRenderer>().positionCount = 0;
                this.transform.GetComponent<EnemyTurn>().enemyA_move(); // 유저턴으로 패스
            });

        }
    }


    private void enemyC_move(bool isUserTurn=true)
    {
        if (this.enemyType != EnemyType.enemyC) return;

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
            s = DOTween.Sequence();
            for (int i = 0; i < FinalNodeList.Count; i++)
            {
                nextMovePos = new Vector3(FinalNodeList[i].x, FinalNodeList[i].y);
                if(i!=0) prevMovePos = new Vector3(FinalNodeList[i-1].x, FinalNodeList[i-1].y);

                // 부채꼴 크기의 탐색 범위 회전
                Vector3 _dir = nextMovePos - prevMovePos;
                s.Append(transform.DOMove(nextMovePos, velocity).SetEase(Ease.Linear)).Join(circularSector.transform.DORotate(new Vector3(0, 0, (Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg)), .3f));
                //circularSector.transform.DORotate(new Vector3(0, 0, (Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg)), .3f);

            }

            // 이동 완료시
            s.Play().OnComplete(() => {
                // 이동경로 끄고 다음 이동경로 설정
                newEnemyRoad.SetActive(false);
                GetComponent<LineRenderer>().positionCount = 0;
                this.transform.GetComponent<EnemyTurn>().enemyC_move(); // 유저턴으로 패스
            });

        }
    }

    // 턴 넘기기
    // 턴은 턴매니저나 게임매니저에서 관리하는게 좋을 것 같다
    public void TurnPass()
    {
        bool isUserTurn = false;
        GameObject.Find("EnemyA").GetComponent<EnemyTurn>().enemyA_move(isUserTurn);
        GameObject.Find("EnemyC").GetComponent<EnemyTurn>().enemyC_move(isUserTurn);
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
        newEnemyRoad.transform.SetParent(RoadList.transform);

        GameObject _road = roadPrefab.transform.GetChild(0).GetChild(0).gameObject;
        GameObject _point = roadPrefab.transform.GetChild(0).GetChild(1).gameObject;
        
        // 시작점 및 끝점
        GameObject startPoint = Instantiate(_point, newEnemyRoad.transform);
        GameObject endPoint = Instantiate(_point, newEnemyRoad.transform);
        startPoint.SetActive(true);
        endPoint.SetActive(true);
        endPoint.transform.position = nextMovePos;
        startPos = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        targetPos = new Vector2Int((int)nextMovePos.x, (int)nextMovePos.y);
        PathFinding();


        // LineRenderer 설정, 적 이동경로(A* 최단거리 알고리즘)대로 선으로 표시
        LineRenderer lr = this.GetComponent<LineRenderer>();
        lr.positionCount = FinalNodeList.Count;
        for (int i = 0; i < FinalNodeList.Count; i++)
        {
            lr.SetPosition(i, new Vector3(FinalNodeList[i].x, FinalNodeList[i].y));
        }

        //// 길 경로 및 위치 설정
        //float dist = Vector2.Distance(startPoint.transform.localPosition, endPoint.transform.localPosition);
        //for (int i = 1; i <= (int)dist; i++)
        //{
        //    GameObject road = Instantiate(_road, newEnemyRoad.transform.GetChild(0));
        //    road.SetActive(true);
        //    road.transform.localPosition = new Vector2(startPoint.transform.localPosition.x + i, 0);
        //    //float dist = Vector2.Distance(startPoint.transform.position, endPoint.transform.position);
        //}
        //float angle = Mathf.Atan2(endPoint.transform.localPosition.y, endPoint.transform.localPosition.x) * Mathf.Rad2Deg;
        //newEnemyRoad.transform.GetChild(0).DORotate(new Vector3(0, 0, angle),0f);

    }
    IEnumerator roadUX()
    {
        LineRenderer lr = this.GetComponent<LineRenderer>();
        lr.material.SetTextureOffset("_MainTex", new Vector2(lr.material.GetTextureOffset("_MainTex").x - 0.1f, 0f));
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(roadUX());
    }
}



// 적 이동경로 - Astar
public class EnemyNode
{
    public EnemyNode(bool _isWall, int _x, int _y) { isWall = _isWall; x = _x; y = _y; }

    public bool isWall;
    public EnemyNode ParentNode;

    // G : 시작으로부터 이동했던 거리, H : |가로|+|세로| 장애물 무시하여 목표까지의 거리, F : G + H
    public int x, y, G, H;
    public int F { get { return G + H; } }
}
partial class EnemyTurn
{
    public Vector2Int bottomLeft, topRight, startPos, targetPos;
    public List<EnemyNode> FinalNodeList;
    public bool allowDiagonal, dontCrossCorner;

    int sizeX, sizeY;
    EnemyNode[,] NodeArray;
    EnemyNode StartNode, TargetNode, CurNode;
    List<EnemyNode> OpenList, ClosedList;


    public void PathFinding()
    {
        // NodeArray의 크기 정해주고, isWall, x, y 대입
        sizeX = topRight.x - bottomLeft.x + 1;
        sizeY = topRight.y - bottomLeft.y + 1;
        NodeArray = new EnemyNode[sizeX, sizeY];

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                bool isWall = false;
                foreach (Collider2D col in Physics2D.OverlapCircleAll(new Vector2(i + bottomLeft.x, j + bottomLeft.y), 0.4f))
                    if (col.gameObject.layer == LayerMask.NameToLayer("Wall")) isWall = true;

                NodeArray[i, j] = new EnemyNode(isWall, i + bottomLeft.x, j + bottomLeft.y);
            }
        }


        // 시작과 끝 노드, 열린리스트와 닫힌리스트, 마지막리스트 초기화
        StartNode = NodeArray[startPos.x - bottomLeft.x, startPos.y - bottomLeft.y];
        TargetNode = NodeArray[targetPos.x - bottomLeft.x, targetPos.y - bottomLeft.y];

        OpenList = new List<EnemyNode>() { StartNode };
        ClosedList = new List<EnemyNode>();
        FinalNodeList = new List<EnemyNode>();


        while (OpenList.Count > 0)
        {
            // 열린리스트 중 가장 F가 작고 F가 같다면 H가 작은 걸 현재노드로 하고 열린리스트에서 닫힌리스트로 옮기기
            CurNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
                if (OpenList[i].F <= CurNode.F && OpenList[i].H < CurNode.H) CurNode = OpenList[i];

            OpenList.Remove(CurNode);
            ClosedList.Add(CurNode);


            // 마지막
            if (CurNode == TargetNode)
            {
                EnemyNode TargetCurNode = TargetNode;
                while (TargetCurNode != StartNode)
                {
                    FinalNodeList.Add(TargetCurNode);
                    TargetCurNode = TargetCurNode.ParentNode;
                }
                FinalNodeList.Add(StartNode);
                FinalNodeList.Reverse();

                for (int i = 0; i < FinalNodeList.Count; i++)
                {
                    //print(i + "번째는 " + FinalNodeList[i].x + ", " + FinalNodeList[i].y);
                }
                return;
            }


            // ↗↖↙↘
            if (allowDiagonal)
            {
                OpenListAdd(CurNode.x + 1, CurNode.y + 1);
                OpenListAdd(CurNode.x - 1, CurNode.y + 1);
                OpenListAdd(CurNode.x - 1, CurNode.y - 1);
                OpenListAdd(CurNode.x + 1, CurNode.y - 1);
            }

            // ↑ → ↓ ←
            OpenListAdd(CurNode.x, CurNode.y + 1);
            OpenListAdd(CurNode.x + 1, CurNode.y);
            OpenListAdd(CurNode.x, CurNode.y - 1);
            OpenListAdd(CurNode.x - 1, CurNode.y);
        }
    }

    void OpenListAdd(int checkX, int checkY)
    {
        // 상하좌우 범위를 벗어나지 않고, 벽이 아니면서, 닫힌리스트에 없다면
        if (checkX >= bottomLeft.x && checkX < topRight.x + 1 && checkY >= bottomLeft.y && checkY < topRight.y + 1 && !NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y].isWall && !ClosedList.Contains(NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y]))
        {
            // 대각선 허용시, 벽 사이로 통과 안됨
            if (allowDiagonal) if (NodeArray[CurNode.x - bottomLeft.x, checkY - bottomLeft.y].isWall && NodeArray[checkX - bottomLeft.x, CurNode.y - bottomLeft.y].isWall) return;

            // 코너를 가로질러 가지 않을시, 이동 중에 수직수평 장애물이 있으면 안됨
            if (dontCrossCorner) if (NodeArray[CurNode.x - bottomLeft.x, checkY - bottomLeft.y].isWall || NodeArray[checkX - bottomLeft.x, CurNode.y - bottomLeft.y].isWall) return;


            // 이웃노드에 넣고, 직선은 10, 대각선은 14비용
            EnemyNode NeighborNode = NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y];
            int MoveCost = CurNode.G + (CurNode.x - checkX == 0 || CurNode.y - checkY == 0 ? 10 : 14);


            // 이동비용이 이웃노드G보다 작거나 또는 열린리스트에 이웃노드가 없다면 G, H, ParentNode를 설정 후 열린리스트에 추가
            if (MoveCost < NeighborNode.G || !OpenList.Contains(NeighborNode))
            {
                NeighborNode.G = MoveCost;
                NeighborNode.H = (Mathf.Abs(NeighborNode.x - TargetNode.x) + Mathf.Abs(NeighborNode.y - TargetNode.y)) * 10;
                NeighborNode.ParentNode = CurNode;

                OpenList.Add(NeighborNode);
            }
        }
    }

    void OnDrawGizmos()
    {
        if (FinalNodeList != null)
        {
            if (FinalNodeList.Count != 0) for (int i = 0; i < FinalNodeList.Count - 1; i++)
                    Gizmos.DrawLine(new Vector2(FinalNodeList[i].x, FinalNodeList[i].y), new Vector2(FinalNodeList[i + 1].x, FinalNodeList[i + 1].y));
        }
    }
}

// 들킴 여부
partial class EnemyTurn
{
    // 플레이어 감지
    public void DetectPlayer()
    {
        s.Pause();
        this.isAlert = true;
        alertSprite.SetActive(true);
    }
}