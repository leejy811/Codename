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
        character._animator.SetFloat("xDir", characterHandle.WeaponAimComponent.GetMousePosition().x);
        character._animator.SetFloat("yDir", characterHandle.WeaponAimComponent.GetMousePosition().y);
    }
}
