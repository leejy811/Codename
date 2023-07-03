using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    [SerializeField] public MapManager mapManger;
    [SerializeField] public GameObject player;
    [SerializeField] public TurnModePathFinder turnPathFinder;

    public static GameManager Instance
    {
        get
        {
            if(null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    void Awake()
    {
        if(null == instance)
        {
            instance = this;
            
        }
    }
}
