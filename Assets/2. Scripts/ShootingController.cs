using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShootingController : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float shootingSpeed;

    private bool isCoolTime = false;

    private void Update()
    {
        if (PlayerController.PlayerState == PlayerController.PlayerStates.Dead || PlayerController.PlayerState == PlayerController.PlayerStates.Roll)
            return;

        Rotate();
        TryShoot();
    }

    private void Rotate()
    {

    }

    private void TryShoot()
    {
        if (isCoolTime)
            return;
        StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {

        yield return null;
        isCoolTime = false;
    }
}
