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
            Time.timeScale = 1f; // ȭ�� ���� ����
            MMSceneLoadingManager.LoadScene("1. Scenes/StartScreen", "StartScreen");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player") return;
        Debug.Log(other.gameObject.name);

        // �� ������Ǵ°� ����
        if (isDoorEntered) return;
        isDoorEntered = true;

        // Ŭ���� �� ���̵��εǸ鼭 ����
        clearUI.SetActive(true);
        clearhBar.DOFade(.9f, .3f).From(0f);

        Time.timeScale = 0.1f; // ȭ�� ���߱� ���� ���ο���

        // YOU DIED �ؽ�Ʈ ���̵��εǸ鼭 ���� �� scale ����
        clearText1.DOFade(.8f, .3f).From(0f).SetDelay(.05f);
        clearText2.transform.DOScale(1.2f, .2f).SetDelay(.05f);

        // Go to Lobby Any Button �ؽ�Ʈ ��������ȿ��
        clearText2.DOFade(.5f, .2f).From(0f).SetDelay(.2f).OnComplete(() =>
        {
            Time.timeScale = 0f; // ȭ�� ����
            clearText2.DOFade(.2f, .1f).From(.7f).SetLoops(-1, LoopType.Yoyo);
            uxDone = true;
        });
    }
}
