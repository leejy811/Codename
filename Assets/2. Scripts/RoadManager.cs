using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoadManager : MonoBehaviour
{

    private static RoadManager instance = null; // 싱글톤 객체

    public static RoadManager Instance // 싱글톤 받아오기
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    private void Awake()
    {
        instance = this;
    }

    public void GenerateRoad()
    {
        Tilemap tileMap = MapGenerateManager.Instance.tileMap;
        Tile roomTile = MapGenerateManager.Instance.roomTile;
        Vector2Int mapSize = MapGenerateManager.Instance.mapSize;

        // 세로 방 개수 = 2를 (트리깊이/2)만큼 제곱한 값
        int maxRoomCnt_Y = (int)Mathf.Pow(2, MapGenerateManager.Instance.maximumDepth / 2);
        // 가로 방 개수 = 최대 방 수 / 세로 방 개수
        int maxRoomCnt_X = RoomManager.Instance.roomCnt / maxRoomCnt_Y;

        // 방 인덱스 번호 메기고, 다시 인덱스대로 방 리스트 정렬
        for (int idx = 0; idx < RoomManager.Instance.roomCnt; idx++)
        {
            Room targetRoom = RoomManager.Instance.roomLIst[idx];
            targetRoom.roomIdx = (targetRoom.roomCenter.x / (mapSize.x / maxRoomCnt_X)) + (targetRoom.roomCenter.y / (mapSize.y / maxRoomCnt_Y)) * maxRoomCnt_Y;
        }
        RoomManager.Instance.roomLIst = RoomManager.Instance.roomLIst.OrderBy(x => x.roomIdx).ToList();


        for (int idx = 0; idx < RoomManager.Instance.roomCnt; idx++)
        {
            // 길 생성할 타겟 room
            Room targetRoom = RoomManager.Instance.roomLIst[idx];

            // 생성된 Room 수만큼 상하좌우 방 존재여부 검사
            Room leftRoom = null;
            Room rightRoom = null;
            Room upRoom = null;
            Room downRoom = null;
            if ((idx % maxRoomCnt_X) > 0) leftRoom = RoomManager.Instance.roomLIst[idx-1];
            if ((idx % maxRoomCnt_X) < maxRoomCnt_X-1) rightRoom = RoomManager.Instance.roomLIst[idx+1];
            if ((idx / maxRoomCnt_X) > 0) upRoom = RoomManager.Instance.roomLIst[idx-maxRoomCnt_Y];
            if ((idx / maxRoomCnt_X) < maxRoomCnt_Y-1) downRoom = RoomManager.Instance.roomLIst[idx+maxRoomCnt_Y];

            // 왼쪽 길 생성
            if (leftRoom != null)
            {
                // 이미 왼쪽 방에 오른쪽 길이 생성되어 있다면 생성하지 않음
                if (!leftRoom.isRightRoad)
                {
                    leftRoom.isRightRoad = true;
                    targetRoom.isLeftRoad = true;
                    for (int i = targetRoom.roomCenter.x; i > leftRoom.roomCenter.x; i--)
                        tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, (leftRoom.roomCenter.y+targetRoom.roomCenter.y)/2 - mapSize.y / 2, 0), roomTile);
                }
            }

            // 오른쪽 길 생성
            if (rightRoom != null)
            {
                // 이미 오른쪽 방에 왼쪽 길이 생성되어 있다면 생성하지 않음
                if (!rightRoom.isLeftRoad)
                {
                    rightRoom.isLeftRoad = true;
                    targetRoom.isRightRoad = true;
                    for (int i = targetRoom.roomCenter.x; i < rightRoom.roomCenter.x; i++)
                        tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, (targetRoom.roomCenter.y + rightRoom.roomCenter.y) / 2 - mapSize.y / 2, 0), roomTile);
                }
            }

            // 윗쪽 길 생성
            if (upRoom != null)
            {
                // 이미 위쪽 방에 아랫길이 생성되어 있다면 생성하지 않음
                if (!upRoom.isDownRoad)
                {
                    upRoom.isDownRoad = true;
                    targetRoom.isUpRoad = true;
                    for (int i = targetRoom.roomCenter.y; i > upRoom.roomCenter.y; i--)
                        tileMap.SetTile(new Vector3Int((targetRoom.roomCenter.x+upRoom.roomCenter.x)/2 - mapSize.x / 2, i - mapSize.y / 2, 0), roomTile);
                }
            }

            // 아래쪽 길 생성
            if (downRoom != null)
            {
                // 이미 아래쪽 방에 윗길이 생성되어 있다면 생성하지 않음
                if (!downRoom.isUpRoad)
                {
                    downRoom.isUpRoad = true;
                    targetRoom.isDownRoad = true;
                    for (int i = targetRoom.roomCenter.y; i < downRoom.roomCenter.y; i++)
                        tileMap.SetTile(new Vector3Int((targetRoom.roomCenter.x + downRoom.roomCenter.x) / 2 - mapSize.x / 2, i - mapSize.y / 2, 0), roomTile);
                }
            }

        }


    }
}
