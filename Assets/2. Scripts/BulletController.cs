using System.Collections;
using UnityEngine;
using DG.Tweening;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float distance;

    private float damage=1;
    private string shooter="null";

    private void OnEnable()
    {
        StartCoroutine(Shoot());
    }

    public void Init(float _damage, string _shooter)
    {
        damage = _damage;
        shooter = _shooter;
    }
    
    IEnumerator Shoot()
    {
        var tween = transform.DOMove(transform.right*distance, 1 / speed).SetRelative();
        yield return tween.WaitForCompletion();

        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag(shooter))
            return;
        if (collision.gameObject.GetComponent<ActiveObject>()) 
            collision.gameObject.GetComponent<ActiveObject>().GetDamage(damage);

        /*
         * Effect(sfx+vfx)
         * 
         */
        Destroy(this.gameObject);
    }

}
