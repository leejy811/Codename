using DG.Tweening;
using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadAnimation : MonoBehaviour
{
    [SerializeField] ProjectileWeapon weapon;
    [SerializeField] GameObject progressBar;
    bool isProgressBarOnHead = false;

    private void OnEnable()
    {
        if (!isProgressBarOnHead)
        {
            this.transform.parent = LevelManager.Instance.Players[0].transform;
            this.transform.localPosition = new Vector3(0, 1.5f, 0);
            this.transform.localRotation = Quaternion.Euler(0, 0, 0);
            this.transform.localScale = new Vector3(1f, 1.5f, 1.5f);
            isProgressBarOnHead = true;
        }
        progressBar.transform.DOScaleX(1f, weapon.ReloadTime).SetEase(Ease.Linear).OnComplete(() =>
        {
            this.gameObject.SetActive(false);
            progressBar.transform.DOScaleX(0f, 0f);
        });
    }

}
