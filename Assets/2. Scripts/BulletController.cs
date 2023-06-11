using System.Collections;
using UnityEngine;
using DG.Tweening;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float distance;

    private void OnEnable()
    {
        StartCoroutine(Shoot());
    }
    
    IEnumerator Shoot()
    {
        var tween = transform.DOMove(transform.right*distance, 1 / speed).SetRelative();
        yield return tween.WaitForCompletion();

        gameObject.SetActive(false);
    }
    
}
