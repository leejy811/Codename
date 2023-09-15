using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunS_HitFeedback : MonoBehaviour
{
    GameObject collisionTarget;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Character>() != null)
        {
            Debug.Log(collision.gameObject.name + " 기절");
            collisionTarget = collision.gameObject;
            collisionTarget.gameObject.GetComponent<AIBrain>().enabled = false;
            collisionTarget.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            collisionTarget.gameObject.GetComponent<Character>().CharacterModel.GetComponent<SpriteRenderer>().color = Color.magenta;
            
            // 적 무기 비활성화
            ProjectileWeapon pw = collisionTarget.gameObject.GetComponent<CharacterHandleWeapon>().WeaponAttachment.GetChild(0)?.GetComponent<ProjectileWeapon>();
            if (pw != null) { pw.enabled = false; }

            Invoke("freezeUnlock", 2f);
        }

    }

    void freezeUnlock()
    {
        collisionTarget.gameObject.GetComponent<Character>().CharacterModel.GetComponent<SpriteRenderer>().color = Color.white;

        collisionTarget.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        collisionTarget.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        collisionTarget.gameObject.GetComponent<AIBrain>().enabled = true;

        // 적 무기 활성화
        ProjectileWeapon pw = collisionTarget.gameObject.GetComponent<CharacterHandleWeapon>().WeaponAttachment.GetChild(0)?.GetComponent<ProjectileWeapon>();
        if(pw != null) { pw.enabled = true; }

    }
}
