using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Transform player;

    private void Start()
    {
        player = GameManager.Instance.player.transform;
        transform.position = player.position;
    }

    private void FixedUpdate()
    {
        Vector3 targetPos = new Vector3(player.position.x, player.position.y);//, this.transform.position.z);

        Debug.Log(targetPos);
        transform.position = Vector3.Lerp(transform.position, targetPos, 10f);
    }
}