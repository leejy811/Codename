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

public class EnemyTurn : MonoBehaviour
{
    // 현재 타일맵 정보
    [SerializeField]
    public Tilemap tilemap;

    // 적 타입
    [SerializeField]
    public EnemyType enemyType;

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
                    Debug.Log("Tile at position (" + x + ", " + y + "): " + tilemap.GetTile(new Vector3Int(x, y)).name);
                    
                }
                else
                {
                    // 타일이 없는 경우에 대한 처리
                    Debug.Log("No tile at position (" + x + ", " + y + ")");
                }
            }
        }
    }
}
