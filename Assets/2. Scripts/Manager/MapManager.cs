using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Node
{
    public Node leftNode;
    public Node rightNode;
    public Node parNode;
    public RectInt nodeRect; //분리된 공간의 rect정보
    public RectInt roomRect; //분리된 공간 속 방의 rect정보
    public Vector2Int center
    {
        get
        {
            return new Vector2Int(roomRect.x + roomRect.width / 2, roomRect.y + roomRect.height / 2);
        }
        //방의 가운데 점. 방과 방을 이을 때 사용
    }
    public Node(RectInt rect)
    {
        this.nodeRect = rect;
    }
}
public class MapManager : MonoBehaviour
{
    [SerializeField] public Vector2Int mapSize;
    [SerializeField] float minimumDevideRate; //공간이 나눠지는 최소 비율
    [SerializeField] float maximumDivideRate; //공간이 나눠지는 최대 비율

    [SerializeField] public int maxDepth; //트리의 높이, 높을 수록 방을 더 자세히 나누게 됨

    [SerializeField] public Tilemap tileMap;
    [SerializeField] public Tile roomTile; //방을 구성하는 타일
    [SerializeField] Tile wallTile; //방과 외부를 구분지어줄 벽 타일
    [SerializeField] Tile outTile; //방 외부의 타일
    [SerializeField] public Tile ckTile; //방 외부의 타일

    [Header("=== Room Manager / Road Manager ===")]
    [SerializeField] public GameObject roomManager;
    [SerializeField] public GameObject roadManager;

    private bool isPlayerSpawned;
    //private GameObject player;

    //private void Awake()
    //{
    //    player = GameManager.Instance.player;
    //}
    void Start()
    {
        FillBackground();//신 로드 시 전부다 바깥타일로 덮음
        Node root = new Node(new RectInt(0, 0, mapSize.x, mapSize.y));
        Divide(root, 0);
        GenerateRoom(root, 0);
        
        GenerateLoad(root, 0);
        FillWall(); //바깥과 방이 만나는 지점을 벽으로 칠해주는 함수
    }

    void Divide(Node tree, int n)
    {
        if (n == maxDepth) return; //내가 원하는 높이에 도달하면 더 나눠주지 않는다.
                                       //그 외의 경우에는

        int maxLength = Mathf.Max(tree.nodeRect.width, tree.nodeRect.height);
        //가로와 세로중 더 긴것을 구한후, 가로가 길다면 위 좌, 우로 세로가 더 길다면 위, 아래로 나눠주게 될 것이다.
        int split = Mathf.RoundToInt(Random.Range(maxLength * minimumDevideRate, maxLength * maximumDivideRate));
        //나올 수 있는 최대 길이와 최소 길이중에서 랜덤으로 한 값을 선택
        if (tree.nodeRect.width >= tree.nodeRect.height) //가로가 더 길었던 경우에는 좌 우로 나누게 될 것이며, 이 경우에는 세로 길이는 변하지 않는다.
        {

            tree.leftNode = new Node(new RectInt(tree.nodeRect.x, tree.nodeRect.y, split, tree.nodeRect.height));
            //왼쪽 노드에 대한 정보다 
            //위치는 좌측 하단 기준이므로 변하지 않으며, 가로 길이는 위에서 구한 랜덤값을 넣어준다.
            tree.rightNode = new Node(new RectInt(tree.nodeRect.x + split, tree.nodeRect.y, tree.nodeRect.width - split, tree.nodeRect.height));
            //우측 노드에 대한 정보다 
            //위치는 좌측 하단에서 오른쪽으로 가로 길이만큼 이동한 위치이며, 가로 길이는 기존 가로길이에서 새로 구한 가로값을 뺀 나머지 부분이 된다. 
        }
        else
        {

            tree.leftNode = new Node(new RectInt(tree.nodeRect.x, tree.nodeRect.y, tree.nodeRect.width, split));
            tree.rightNode = new Node(new RectInt(tree.nodeRect.x, tree.nodeRect.y + split, tree.nodeRect.width, tree.nodeRect.height - split));
            //DrawLine(new Vector2(tree.nodeRect.x , tree.nodeRect.y+ split), new Vector2(tree.nodeRect.x + tree.nodeRect.width, tree.nodeRect.y  + split));
        }
        tree.leftNode.parNode = tree; //자식노드들의 부모노드를 나누기전 노드로 설정
        tree.rightNode.parNode = tree;
        Divide(tree.leftNode, n + 1); //왼쪽, 오른쪽 자식 노드들도 나눠준다.
        Divide(tree.rightNode, n + 1);//왼쪽, 오른쪽 자식 노드들도 나눠준다.
    }
    private RectInt GenerateRoom(Node tree, int n)
    {
        RectInt rect;
        if (n == maxDepth) //해당 노드가 리프노드라면 방을 만들어 줄 것이다.
        {
            rect = tree.nodeRect;
            int width = Random.Range(rect.width / 2, rect.width - 1);
            //방의 가로 최소 크기는 노드의 가로길이의 절반, 최대 크기는 가로길이보다 1 작게 설정한 후 그 사이 값중 랜덤한 값을 구해준다.
            int height = Random.Range(rect.height / 2, rect.height - 1);
            //높이도 위와 같다.
            int x = rect.x + Random.Range(1, rect.width - width);
            //방의 x좌표이다. 만약 0이 된다면 붙어 있는 방과 합쳐지기 때문에,최솟값은 1로 해주고, 최댓값은 기존 노드의 가로에서 방의 가로길이를 빼 준 값이다.
            int y = rect.y + Random.Range(1, rect.height - height);
            //y좌표도 위와 같다.
            rect = new RectInt(x, y, width, height);
            FillRoom(rect);
            Room room = new Room();
            room.rect = rect;
            room.roomCenter = new Vector2Int(x + width / 2, y + height / 2);
            roomManager.GetComponent<RoomManager>().roomLIst.Add(room);
            roomManager.GetComponent<RoomManager>().roomCnt++;

            int player_x = Random.Range(0, 10);
            if(!isPlayerSpawned && player_x == 0 || !isPlayerSpawned && roomManager.GetComponent<RoomManager>().roomCnt == 15)
            {
                Instantiate(GameManager.Instance.player, new Vector3(room.roomCenter.x-mapSize.x/2, room.roomCenter.y-mapSize.y / 2, 0), Quaternion.identity);
                Instantiate(GameManager.Instance.enemyA, new Vector3(Random.RandomRange(room.rect.x, room.rect.x + room.rect.width) - mapSize.x / 2, Random.RandomRange(room.rect.y, room.rect.y + room.rect.height) - mapSize.y / 2, 0), Quaternion.identity);
                Instantiate(GameManager.Instance.enemyB, new Vector3(Random.RandomRange(room.rect.x, room.rect.x + room.rect.width) - mapSize.x / 2, Random.RandomRange(room.rect.y, room.rect.y + room.rect.height) - mapSize.y / 2, 0), Quaternion.identity);
                Instantiate(GameManager.Instance.enemyC, new Vector3(Random.RandomRange(room.rect.x, room.rect.x + room.rect.width) - mapSize.x / 2, Random.RandomRange(room.rect.y, room.rect.y + room.rect.height) - mapSize.y / 2, 0), Quaternion.identity);


                isPlayerSpawned = true;
            }
        }
        else
        {
            tree.leftNode.roomRect = GenerateRoom(tree.leftNode, n + 1);
            tree.rightNode.roomRect = GenerateRoom(tree.rightNode, n + 1);
            rect = tree.leftNode.roomRect;
        }
        return rect;
    }
    private void GenerateLoad(Node tree, int n)
    {
        if (n == maxDepth) //리프 노드라면 이을 자식이 없다.
            return;

        roadManager.GetComponent<RoadManager>().GenerateRoad();
        {
            //Vector2Int leftNodeCenter = tree.leftNode.center;
            //Vector2Int rightNodeCenter = tree.rightNode.center;

            //bool flag = false;

            //if (Mathf.Min(leftNodeCenter.x, rightNodeCenter.x) == leftNodeCenter.x)
            //{
            //    for (int num = 0; num < tree.leftNode.roomRect.height; num++)
            //    {
            //        if (tileMap.GetTile(new Vector3Int(tree.leftNode.roomRect.x + tree.leftNode.roomRect.width - mapSize.x / 2, tree.leftNode.roomRect.y + num - mapSize.y/2 , 0)) == roomTile)
            //        {
            //            flag = true;
            //            break;
            //        }
            //    }
            //}
            //else
            //{
            //    for (int num = 0; num < tree.rightNode.roomRect.height; num++)
            //    {
            //        if (tileMap.GetTile(new Vector3Int(tree.rightNode.roomRect.x - tree.rightNode.roomRect.width - mapSize.x / 2, tree.rightNode.roomRect.y + num - mapSize.y / 2, 0)) == roomTile)
            //        {
            //            flag = true;
            //            break;
            //        }
            //    }
            //}
            //// 가로방향 길
            //for (int i = Mathf.Min(leftNodeCenter.x, rightNodeCenter.x); i <= Mathf.Max(leftNodeCenter.x, rightNodeCenter.x); i++)
            //{
            //    if (tileMap.GetTile(new Vector3Int(i - mapSize.x / 2, leftNodeCenter.y - mapSize.y / 2, 0)) == roomTile)
            //        continue;
            //    // 가로방향 길 이미 만들어졌는지 검사
            //    if (!flag)
            //        tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, leftNodeCenter.y - mapSize.y / 2, 0), ckTile);
            //}

            //flag = false;
            //if (Mathf.Min(leftNodeCenter.y, rightNodeCenter.y) == leftNodeCenter.y)
            //{
            //    for (int num = 0; num < tree.leftNode.roomRect.width; num++)
            //    {
            //        if (tileMap.GetTile(new Vector3Int(tree.leftNode.roomRect.x + num - mapSize.x/2, tree.leftNode.roomRect.y + tree.leftNode.roomRect.height - mapSize.y/2 , 0)) == roomTile)
            //        {
            //            flag = true;
            //            break;
            //        }
            //    }
            //}
            //else
            //{
            //    for (int num = 0; num < tree.rightNode.roomRect.width; num++)
            //    {

            //        if (tileMap.GetTile(new Vector3Int(tree.rightNode.roomRect.x + num - mapSize.x/2, tree.rightNode.roomRect.y - tree.rightNode.roomRect.height - mapSize.y/2 , 0)) == roomTile)
            //        {
            //            flag = true;
            //            break;
            //        }
            //    }
            //}
            //// 세로방향 길
            //for (int j = Mathf.Min(leftNodeCenter.y, rightNodeCenter.y); j <= Mathf.Max(leftNodeCenter.y, rightNodeCenter.y); j++)
            //{
            //    if (tileMap.GetTile(new Vector3Int(rightNodeCenter.x - mapSize.x / 2, j - mapSize.y / 2, 0)) == roomTile)
            //        continue;

            //    if (!flag)
            //        tileMap.SetTile(new Vector3Int(rightNodeCenter.x - mapSize.x / 2, j - mapSize.y / 2, 0), ckTile);
            //}
            ////이전 포스팅에서 선으로 만들었던 부분을 room tile로 채우는 과정
            //GenerateLoad(tree.leftNode, n + 1); //자식 노드들도 탐색
            //GenerateLoad(tree.rightNode, n + 1);
        }
    }

    void FillBackground() //배경을 채우는 함수, 씬 load시 가장 먼저 해준다.
    {
        for (int i = -10; i < mapSize.x + 10; i++) //바깥타일은 맵 가장자리에 가도 어색하지 않게
        //맵 크기보다 넓게 채워준다.
        {
            for (int j = -10; j < mapSize.y + 10; j++)
            {
                tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), outTile);
            }
        }
    }
    void FillWall() //룸 타일과 바깥 타일이 만나는 부분
    {
        for (int i = 0; i < mapSize.x; i++) //타일 전체를 순회
        {
            for (int j = 0; j < mapSize.y; j++)
            {
                if (tileMap.GetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0)) == outTile)
                {
                    //바깥타일 일 경우
                    for (int x = -1; x <= 1; x++)
                    {
                        for (int y = -1; y <= 1; y++)
                        {
                            if (x == 0 && y == 0) continue;//바깥 타일 기준 8방향을 탐색해서 room tile이 있다면 wall tile로 바꿔준다.
                            if (tileMap.GetTile(new Vector3Int(i - mapSize.x / 2 + x, j - mapSize.y / 2 + y, 0)) == roomTile)
                            {
                                tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), wallTile);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
    private void FillRoom(RectInt rect)
    { 
        // 방 위치 내에서 랜덤으로 오브젝트, 방의 정보(enum으로 무슨 방인지 표시, 2차원 배열로 맵 상태 표시)


        //room의 rect정보를 받아서 tile을 set해주는 함수
        for (int i = rect.x; i < rect.x + rect.width; i++)
        {
            for (int j = rect.y; j < rect.y + rect.height; j++)
            {
                tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), roomTile);
            }
        }
    }

}