using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachinGunS_Bullet : MonoBehaviour
{
    // 총알이 오브젝트 풀링으로 비활성화되면 circle collider 다시 원상복귀
    [SerializeField] float originColliderRadius;

    private void OnDisable()
    {
        GetComponent<CircleCollider2D>().radius = originColliderRadius;
    }
}
