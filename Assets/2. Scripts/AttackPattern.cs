using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AttackPattern : MonoBehaviour
{
    public static readonly int patternCount = 1; 

    /// <summary>
    /// Enemy Attack Pattern, make number of bullets sequentially and shoot
    /// </summary>
    /// <param name="bullet">bullet prefab</param>
    /// <param name="bulletCount">the number of bullet to be used</param>
    /// <param name="bulletSpeed">speed of bullet</param>
    /// <param name="damage">damage of bullet</param>
    /// <param name="originPos">base position to be used for bullet generation. give the 'enemy position' in general</param>
    /// <param name="monster">the object who starts attack</param>
    /// <returns></returns>
    public static IEnumerator PatternA(MonsterBulletController bullet, int bulletCount, float bulletSpeed,float damage, Vector3 originPos, MonsterController monster)
    {
        var bullets = new List<MonsterBulletController>();

        for(int theta=0; theta < 360; theta += (360/bulletCount))
        {
            // set bullet pos to draw a circle from the monster
            Vector2 bulletPos = new (originPos.x + Mathf.Cos(theta*Mathf.Deg2Rad)*1.5f, originPos.y + Mathf.Sin(theta * Mathf.Deg2Rad) *1.5f);
            bullets.Add(Instantiate(bullet, new Vector3(bulletPos.x, bulletPos.y, 0), Quaternion.identity));
            yield return new WaitForSeconds(0.5f);
        }

        //shoot all bullets at once after all bullets generated
        foreach (var b in bullets)
        {
            try
            {
                b.Shoot();
            }
            catch { Debug.Log("bullet does not exist"); }
        }

        yield return new WaitForSeconds(1);

        monster.FinishAttack();
    }
}
