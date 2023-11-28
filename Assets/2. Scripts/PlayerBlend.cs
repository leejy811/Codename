using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;

public class PlayerBlend : MonoBehaviour
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
        if (characterHandle.WeaponAimComponent == null)
            return;
        character._animator.SetFloat("xDir", characterHandle.WeaponAimComponent.GetMousePosition().x - transform.position.x);
        character._animator.SetFloat("yDir", characterHandle.WeaponAimComponent.GetMousePosition().y - transform.position.y);
    }
}
