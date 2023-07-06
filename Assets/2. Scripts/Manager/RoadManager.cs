using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoadManager : MonoBehaviour
{

    //private MapManager mapManager;

    //private void Awake()
    //{
    //    mapManager = GameManager.Instance.mapManger;
    //}

    public void GenerateRoad()
    {
        Tilemap tileMap = GameManager.Instance.mapManger.tileMap;
        Tile roomTile = GameManager.Instance.mapManger.roomTile;
        Vector2Int mapSize = GameManager.Instance.mapManger.mapSize;

        int maxRoomCnt_Y = (int)Mathf.Pow(2, GameManager.Instance.mapManger.maxDepth / 2);
        int maxRoomCnt_X = GameManager.Instance.mapManger.roomManager.GetComponent<RoomManager>().roomCnt / maxRoomCnt_Y;

        for (int idx = 0; idx < GameManager.Instance.mapManger.roomManager.GetComponent<RoomManager>().roomCnt; idx++)
        {
            Room targetRoom = GameManager.Instance.mapManger.roomManager.GetComponent<RoomManager>().roomLIst[idx];
            targetRoom.roomIdx = (targetRoom.roomCenter.x / (mapSize.x / maxRoomCnt_X)) + (targetRoom.roomCenter.y / (mapSize.y / maxRoomCnt_Y)) * maxRoomCnt_Y;
        }
        GameManager.Instance.mapManger.roomManager.GetComponent<RoomManager>().roomLIst = GameManager.Instance.mapManger.roomManager.GetComponent<RoomManager>().roomLIst.OrderBy(x => x.roomIdx).ToList();


        for (int idx = 0; idx < GameManager.Instance.mapManger.roomManager.GetComponent<RoomManager>().roomCnt; idx++)
        {
            Room targetRoom = GameManager.Instance.mapManger.roomManager.GetComponent<RoomManager>().roomLIst[idx];

            Room leftRoom = null;
            Room rightRoom = null;
            Room upRoom = null;
            Room downRoom = null;

            if ((idx % maxRoomCnt_X) > 0) leftRoom = GameManager.Instance.mapManger.roomManager.GetComponent<RoomManager>().roomLIst[idx - 1];
            if ((idx % maxRoomCnt_X) < maxRoomCnt_X - 1) rightRoom = GameManager.Instance.mapManger.roomManager.GetComponent<RoomManager>().roomLIst[idx + 1];
            if ((idx / maxRoomCnt_X) > 0) upRoom = GameManager.Instance.mapManger.roomManager.GetComponent<RoomManager>().roomLIst[idx - maxRoomCnt_Y];
            if ((idx / maxRoomCnt_X) < maxRoomCnt_Y - 1) downRoom = GameManager.Instance.mapManger.roomManager.GetComponent<RoomManager>().roomLIst[idx + maxRoomCnt_Y];

            // 상, 하, 좌, 우 모든 위치에 방이 존재한다면 4개 중 한개의 길은 사라지게 만들 것이다.
            // 0 - 왼쪽 길, 1 - 오른쪽 길, 2 - 위쪽 길, 3 - 아래쪽 길, 4 - 모든 길 연결
            int delRoadDir;

            if (leftRoom != null && rightRoom != null && upRoom != null && downRoom != null)
                delRoadDir = Random.Range(0, 4);
            else
                delRoadDir = 4;

            if (leftRoom != null)
            {
                if (delRoadDir == 0)
                    continue;
                if (!leftRoom.isRightRoad)
                {
                    leftRoom.isRightRoad = true;
                    targetRoom.isLeftRoad = true;
                    for (int i = targetRoom.roomCenter.x; i > leftRoom.roomCenter.x; i--)
                        tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, (leftRoom.roomCenter.y + targetRoom.roomCenter.y) / 2 - mapSize.y / 2, 0), roomTile);
                }
            }

            if (rightRoom != null)
            {
                if (delRoadDir == 1)
                    continue;

                if (!rightRoom.isLeftRoad)
                {
                    rightRoom.isLeftRoad = true;
                    targetRoom.isRightRoad = true;
                    for (int i = targetRoom.roomCenter.x; i < rightRoom.roomCenter.x; i++)
                        tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, (targetRoom.roomCenter.y + rightRoom.roomCenter.y) / 2 - mapSize.y / 2, 0), roomTile);
                }
            }

            if (upRoom != null)
            {
                if (delRoadDir == 2)
                    continue;

                if (!upRoom.isDownRoad)
                {
                    upRoom.isDownRoad = true;
                    targetRoom.isUpRoad = true;
                    for (int i = targetRoom.roomCenter.y; i > upRoom.roomCenter.y; i--)
                        tileMap.SetTile(new Vector3Int((targetRoom.roomCenter.x + upRoom.roomCenter.x) / 2 - mapSize.x / 2, i - mapSize.y / 2, 0), roomTile);
                }
            }
            if (downRoom != null)
            {
                if (delRoadDir == 3)
                    continue;
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