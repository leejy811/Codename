using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Transform player;

    private void Start()
    {

    }

    private void FixedUpdate()
    {
        if(player == null)
        {
            player = GameObject.Find("Turn Mode Player(Clone)").transform;
            transform.position = player.position;
        }

        Vector3 targetPos = new Vector3(player.position.x, player.position.y, -10);

        //Debug.Log(targetPos);

        transform.position = Vector3.Lerp(transform.position, targetPos, 10f);
    }
}