using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BossClearDoor : MonoBehaviour
{
    [SerializeField] GameObject clearUI;
    [SerializeField] Image clearhBar;
    [SerializeField] TextMeshProUGUI clearText1; // To be continue...
    [SerializeField] TextMeshProUGUI clearText2; // Go to Lobby Any Button

    private bool uxDone = false;
    private bool isDoorEntered = false;

    private void Update()
    {
        if (!uxDone) return;
        if (Input.anyKeyDown)
        {
            Time.timeScale = 1f; // 화면 멈춤 해제
            MMSceneLoadingManager.LoadScene("1. Scenes/StartScreen", "StartScreen");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player") return;
        Debug.Log(other.gameObject.name);

        // 문 재입장되는거 막기
        if (isDoorEntered) return;
        isDoorEntered = true;

        // 클리어 바 페이드인되면서 등장
        clearUI.SetActive(true);
        clearhBar.DOFade(.9f, .3f).From(0f);

        Time.timeScale = 0.1f; // 화면 멈추기 전에 슬로우모션

        // YOU DIED 텍스트 페이드인되면서 시작 및 scale 증가
        clearText1.DOFade(.8f, .3f).From(0f).SetDelay(.05f);
        clearText2.transform.DOScale(1.2f, .2f).SetDelay(.05f);

        // Go to Lobby Any Button 텍스트 깜빡깜빡효과
        clearText2.DOFade(.5f, .2f).From(0f).SetDelay(.2f).OnComplete(() =>
        {
            Time.timeScale = 0f; // 화면 멈춤
            clearText2.DOFade(.2f, .1f).From(.7f).SetLoops(-1, LoopType.Yoyo);
            uxDone = true;
        });
    }
}
