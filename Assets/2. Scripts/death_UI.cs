using DG.Tweening;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class death_UI : MonoBehaviour
{
    [SerializeField] Image deathBar;
    [SerializeField] TextMeshProUGUI deathText1; // You Died
    [SerializeField] TextMeshProUGUI deathText2; // Go to Lobby Any Button

    private bool isDied;

    private void OnEnable()
    {
        // 사먕 바 페이드인되면서 등장
        deathBar.DOFade(.9f, 3f).From(0f);

        // YOU DIED 텍스트 페이드인되면서 시작 및 scale 증가
        deathText1.DOFade(.8f, 3f).From(0f).SetDelay(.5f);
        deathText1.transform.DOScale(1.2f, 2f).SetDelay(.5f);

        // Go to Lobby Any Button 텍스트 깜빡깜빡효과
        deathText2.DOFade(.5f, 2f).From(0f).SetDelay(2f).OnComplete(() =>
        {
            deathText2.DOFade(.2f, 1f).From(.7f).SetLoops(-1, LoopType.Yoyo);
            isDied = true;
        });
    }

    private void Update()
    {
        // 아무 키 입력시 로비 화면으로 이동, 안 죽었으면 실행X
        if (!isDied) return;
        if (Input.anyKeyDown)
            MMSceneLoadingManager.LoadScene("1. Scenes/StartScreen", "StartScreen");
    }
}
