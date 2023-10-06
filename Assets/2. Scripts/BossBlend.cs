using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBlend : MonoBehaviour
{
    private CharacterHandleWeapon characterHandle;
    private Character character;

    private void Start()
    {
        characterHandle = gameObject.GetComponent<CharacterHandleWeapon>();
        character = gameObject.GetComponent<Character>();
    }

    private void Update()
    {
        character._animator.SetFloat("xDir", characterHandle.WeaponAimComponent.CurrentAim.x);
        character._animator.SetFloat("yDir", characterHandle.WeaponAimComponent.CurrentAim.y);
    }
}
