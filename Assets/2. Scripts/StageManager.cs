using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;
    

    [SerializeField] PlayerController player;

    private int mapX;
    private int mapY;
    public int[,] map = {
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,1,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }
        };

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        Init();
    }

    private void Init()
    {
        mapX = map.GetLength(1);
        mapY = map.GetLength(0);
    }

    public int[] PlayerPos()
    {
        return new int[]{ (int)((mapY / 2) - player.transform.position.y), (int)(player.transform.position.x + (mapX / 2)) };
    }

    public Vector3 PlayerWorldPos()
    {
        return player.transform.position;
    }
}
