using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float lerpSpeed = 1.0f;

    private Vector3 offset;

    private Vector3 targetPos;

    private void Update()
    {
        if (Camera.current == null) return;

        if (target == null)
        {
            target = LevelManager.Instance.Players[0].transform;
            offset = transform.position - target.position;
        }

        if(this.GetComponent<Camera>().enabled==false) this.GetComponent<Camera>().enabled = true;

        targetPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);
    }
}