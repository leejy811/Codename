using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;  // OnDrawGizmos

public class CircularSector : MonoBehaviour
{
    public Transform target;    // 부채꼴에 포함되는지 판별할 타겟
    public float angleRange = 30f;
    public float radius = 3f;

    Color _blue = new Color(0f, 0f, 1f, 0.2f);
    Color _red = new Color(1f, 0f, 0f, 0.2f);

    public bool isCollision = false;

    void Update()
    {
        Vector3 interV = target.position - transform.position;

        // target과 나 사이의 거리가 radius 보다 작다면
        if (interV.magnitude <= radius)
        {
            // '타겟-나 벡터'와 '내 정면 벡터'를 내적
            float dot = Vector3.Dot(interV.normalized, transform.right);
            // 두 벡터 모두 단위 벡터이므로 내적 결과에 cos의 역을 취해서 theta를 구함
            float theta = Mathf.Acos(dot);
            // angleRange와 비교하기 위해 degree로 변환
            float degree = Mathf.Rad2Deg * theta;

            // 시야각 판별
            if (degree <= angleRange / 2f)
                isCollision = true;
            else
                isCollision = false;

        }
        else
            isCollision = false;
    }

    // 유니티 에디터에 부채꼴을 그려줄 메소드
    private void OnDrawGizmos()
    {
        // 부채꼴의 중심 위치
        Vector3 position = transform.position;

        // 부채꼴의 시작 방향 벡터
        Vector3 startDirection = Quaternion.Euler(0f, 0f, -angleRange / 2f) * transform.right;

        // 부채꼴의 시작점
        Vector3 startPoint = position + startDirection * radius;

        // 부채꼴의 중심과 시작점 사이의 선
        Gizmos.DrawLine(position, startPoint);

        // 부채꼴의 원
        Gizmos.DrawWireSphere(position, radius);

        // 부채꼴의 시작점과 끝점 사이의 선
        Vector3 endPoint = Quaternion.Euler(0f, 0f, angleRange / 2f) * transform.right * radius;
        Gizmos.DrawLine(position, position + endPoint);

        // 부채꼴의 끝점
        Vector3 arcPoint = Quaternion.Euler(0f, 0f, angleRange / 2f) * startDirection;
        Gizmos.DrawLine(position, position + arcPoint * radius);
    }
}