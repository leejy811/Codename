using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType
{
    spawn,
    boss,
    normal,
    special
}

public class Room
{
    public RectInt rect;
    public RoomType type;
    public Vector2Int roomCenter;
    public int roomIdx;

    public bool isLeftRoad = false;
    public bool isRightRoad = false;
    public bool isDownRoad = false;
    public bool isUpRoad = false;
}

public class RoomManager : MonoBehaviour
{
    public List<Room> roomLIst = new List<Room>();
    public int roomCnt;

    private static RoomManager instance = null; // ΩÃ±€≈Ê ∞¥√º
    public static RoomManager Instance // ΩÃ±€≈Ê πﬁæ∆ø¿±‚
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
}
