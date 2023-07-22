using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MonsterBulletController : MonoBehaviour
{
    /// <summary>
    /// test script for temporary. will be merged with 'BulletController'. 
    /// </summary>

    [SerializeField] private float speed;
    [SerializeField] private float distance;

    private float damage = 1;
    private string shooter = "Monster";

    public void Shoot()
    {
        Vector3 playerPos = StageManager.instance.PlayerWorldPos();
        Vector3 bulletPos = transform.position;
        Vector3 targetDir = (playerPos - bulletPos);
        float targetAngle = playerPos.x < bulletPos.x ? Vector3.Angle(targetDir, transform.up) : -Vector3.Angle(targetDir, transform.up);
        transform.Rotate(0, 0, targetAngle);
        StartCoroutine(IShoot());
    }

    public void Init(float _damage, string _shooter)
    {
        damage = _damage;
        shooter = _shooter;
    }

    IEnumerator IShoot()
    {
        var tween = transform.DOMove(transform.up * distance, 1 / speed).SetRelative();
        yield return tween.WaitForCompletion();

        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        try
        {
            if (collision.transform.CompareTag(shooter))
                return;
        }
        catch { Debug.Log("object does not set to any tag. nothing to compare."); };

        if (collision.gameObject.GetComponent<ActiveObject>())
            collision.gameObject.GetComponent<ActiveObject>().GetDamage(damage);
        /*
         * Effect(sfx+vfx)
         * 
         */
        Destroy(this.gameObject);
    }
}
