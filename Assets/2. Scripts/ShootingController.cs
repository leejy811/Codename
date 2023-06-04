using System.Collections.Generic;
using UnityEngine;

public class ShootingController : MonoBehaviour
{
    public enum PlayerStates
    {
        Idle,
        Move,
        Roll,
        Dead,
    }
    public PlayerStates PlayerState { get; private set; } = PlayerStates.Idle;

    [SerializeField] private float moveSpeed;

    private float moveX;
    private float moveY;


    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
