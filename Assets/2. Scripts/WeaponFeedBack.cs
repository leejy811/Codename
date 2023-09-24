using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFeedBack : MonoBehaviour
{
    // 피드백 모음
    [SerializeField] GameObject ShotGunS_feedback;

    // 무기 교체 시 피드백
    // 권총 S 등급 일 경우 -> 플레이어 이속 증가

    public enum WeaponType{
        None,
        PistolS,
        MachinGunS,
    }

    [SerializeField] WeaponType weaponType;

    private void OnEnable()
    {
        switch (weaponType)
        {
            case WeaponType.PistolS:
                Debug.Log("플레이어 이속 증가");
                LevelManager.Instance.Players[0].GetComponent<CharacterMovement>().MovementSpeed *= 2f;
                break;
            case WeaponType.MachinGunS:
                //this.GetComponent<MMSimpleObjectPooler>().GameObjectToPool.GetComponent<DamageOnTouch>().HitAnythingFeedback = ShotGunS_feedback.GetComponent<MMF_Player>();
                //Debug.Log(this.GetComponent<MMSimpleObjectPooler>().GameObjectToPool.GetComponent<DamageOnTouch>().HitAnythingFeedback);
                //ShotGunS_feedback.transform.parent = this.GetComponent<MMSimpleObjectPooler>().GameObjectToPool.transform;
                break;
        }   
    }

    private void OnDisable()
    {
        switch (weaponType)
        {
            case WeaponType.PistolS:
                Debug.Log("플레이어 이속 원상복귀");
                LevelManager.Instance.Players[0].GetComponent<CharacterMovement>().MovementSpeed *= .5f;
                break;
            case WeaponType.MachinGunS:
                break;
        }
    }
}
