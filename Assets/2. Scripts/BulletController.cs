using System.Collections;
using UnityEngine;
using DG.Tweening;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;

    private void OnEnable()
    {
        StartCoroutine(Shoot());
    }
    
    IEnumerator Shoot()
    {
        var tween = transform.DOMoveY(lifeTime, 1 / speed).SetRelative();
        yield return tween.WaitForCompletion();

        gameObject.SetActive(false);
    }
    
}
