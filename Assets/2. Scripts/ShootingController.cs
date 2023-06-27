using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShootingController : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float shootingSpeed;
    [SerializeField] private float reloadTime;

    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletPos;

    [SerializeField] private int maxBulletCount;
    [SerializeField] private int totalBulletCount;

    private int currentBulletCount = 0;

    private bool isCoolTime = false;
    private bool isReloading = false;

    private void Awake()
    {
        currentBulletCount = maxBulletCount < totalBulletCount ? maxBulletCount : totalBulletCount;
        totalBulletCount -= currentBulletCount;
    }

    private void Update()
    {
        if (PlayerController.PlayerState == PlayerController.PlayerStates.Dead || PlayerController.PlayerState == PlayerController.PlayerStates.Roll)
            return;

        Rotate();
        TryReload();
        TryShoot();
    }

    private void Rotate()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dirVec = mousePos - (Vector2)transform.position;

        transform.right = dirVec.normalized;
    }

    private void TryShoot()
    {
        if (isCoolTime || isReloading)
            return;
        if(Input.GetMouseButtonDown(0))
            if(currentBulletCount>0)
                StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {
        isCoolTime = true;
        currentBulletCount -= 1;
        Instantiate(bullet, new Vector3(bulletPos.position.x, bulletPos.position.y, bulletPos.position.z), transform.rotation);
        yield return new WaitForSeconds(1/shootingSpeed);
        isCoolTime = false;
    }

    private void TryReload()
    {
        if (isReloading)
            return;
        if(currentBulletCount<=0 || Input.GetKeyDown(KeyCode.R))
        {
            if (totalBulletCount > 0)
                StartCoroutine(Reload());
            else
                Debug.Log("No bullet left");
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        int targetBulletCount = maxBulletCount - currentBulletCount;

        if (totalBulletCount >= targetBulletCount)
        {
            currentBulletCount += targetBulletCount;
            totalBulletCount -= targetBulletCount;
        }
        else
        {
            currentBulletCount += totalBulletCount;
            totalBulletCount = 0;
        }

        yield return new WaitForSeconds(reloadTime);
        isReloading = false;
    }
}
