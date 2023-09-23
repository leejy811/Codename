using MoreMountains.TopDownEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachinGunS_Bullet : MonoBehaviour
{
    // 총알이 오브젝트 풀링으로 비활성화되면 circle collider 다시 원상복귀
    [SerializeField] float originColliderRadius;
    [SerializeField] GameObject impactEffect;

    [SerializeField] GameObject enchantedEffect;
    [SerializeField] GameObject originEffect;

    public static int shootCnt=0;

    private void OnEnable()
    {
        shootCnt++;
        Debug.Log(shootCnt);

        if (shootCnt % 5 == 0)
        {
            shootCnt = 0;
            impactEffect.transform.GetChild(0).localScale = new Vector3(1f, 1f, 1f);
            impactEffect.transform.GetChild(1).localScale = new Vector3(1f, 1f, 1f);

            this.GetComponent<DamageOnTouch>().HitAnythingEvent.RemoveAllListeners();
        }
        else
        {
            impactEffect.transform.GetChild(0).localScale = new Vector3(.1f, .1f,.1f);
            impactEffect.transform.GetChild(1).localScale = new Vector3(.1f, .1f, .1f);

            this.GetComponent<DamageOnTouch>().HitAnythingEvent.AddListener(grenadeEvent);
        }
    }

    private void grenadeEvent(GameObject arg0)
    {
        GetComponent<CircleCollider2D>().radius = 5f;
        GetComponent<CircleCollider2D>().enabled = true;
        GetComponent<CircleCollider2D>().radius = 0f;
        GetComponent<CircleCollider2D>().enabled = false;
    }

    private void OnDisable()
    {
        GetComponent<CircleCollider2D>().radius = originColliderRadius;
    }
}
