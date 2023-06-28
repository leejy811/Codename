using System.Collections;
using UnityEngine;
using DG.Tweening;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float distance;

    private float damage;

    private void OnEnable()
    {
        StartCoroutine(Shoot());
    }

    public void Init(float _damage)
    {
        damage = _damage;
    }
    
    IEnumerator Shoot()
    {
        var tween = transform.DOMove(transform.right*distance, 1 / speed).SetRelative();
        yield return tween.WaitForCompletion();

        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Monster")
            collision.gameObject.GetComponent<MonsterController>().GetDamage(damage);
    }

}
