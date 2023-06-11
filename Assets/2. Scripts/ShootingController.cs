using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShootingController : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float shootingSpeed;

    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletPos;

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
        if(Input.GetMouseButtonDown(0))
            StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {
        isCoolTime = true;
        Instantiate(bullet, new Vector3(bulletPos.position.x, bulletPos.position.y, bulletPos.position.z), transform.rotation);
        yield return new WaitForSeconds(1/shootingSpeed);
        isCoolTime = false;
    }
}
