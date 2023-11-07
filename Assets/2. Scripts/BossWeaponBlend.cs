using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;

public class BossWeaponBlend : MonoBehaviour
{
    [SerializeField] Transform[] points;
    [SerializeField] Transform weapon;
    [SerializeField] BossBlend blend;

    private void Update()
    {
        Vector2 dir = blend.characterOrient2D;
        Vector3 pos = Vector3.zero;
        float angle = GetAngle(dir);

        if (angle <= 112.5f && angle > 67.5f)
            pos = points[0].localPosition;
        else if (angle <= 67.5f && angle > 22.5f || angle <= 157.5f && angle > 112.5f)
            pos = points[1].localPosition;
        else if (angle <= 22.5f && angle > -22.5f)
            pos = points[2].localPosition;
        else if (angle <= -22.5f && angle > -67.5f || angle <= -112.5f && angle > -157.5f)
            pos = points[3].localPosition;
        else if (angle <= -67.5f && angle > -112.5f)
            pos = points[4].localPosition;
        else
            pos = points[2].localPosition;

        if (dir.x < 0)
            pos = new Vector3(pos.x * -1, pos.y, pos.z);

        weapon.localPosition = pos;

    }

    private float GetAngle(Vector2 vec)
    {
        return Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
    }
}
