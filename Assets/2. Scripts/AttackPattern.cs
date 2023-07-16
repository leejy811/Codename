using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AttackPattern : MonoBehaviour
{
    public static IEnumerator PatternA(GameObject bullet, int bulletCount, float bulletSpeed,float damage, Vector3 originPos)
    {
        for(int theta=0; theta < 360; theta += (360/bulletCount))
        {
            Vector2 bulletPos = new (originPos.x + Mathf.Cos(theta)*3, originPos.y + Mathf.Sin(theta)*3);
            var b = Instantiate(bullet, new Vector3(bulletPos.x, bulletPos.y, 0), Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
        }

        yield return null;
    }
}
