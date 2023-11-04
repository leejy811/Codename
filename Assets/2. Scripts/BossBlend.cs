using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBlend : MonoBehaviour
{
    private CharacterHandleWeapon characterHandle;
    private Character character;
    private CharacterMovement characterMovement;

    [SerializeField]
    private GameObject characterModel;
    Vector2 characterOrient2D;

    private void Start()
    {
        characterHandle = gameObject.GetComponent<CharacterHandleWeapon>();
        character = gameObject.GetComponent<Character>();
        characterMovement = gameObject.GetComponent<CharacterMovement>();

    }

    private void Update()
    {
        if (characterHandle == null || character == null || characterMovement == null)
            return;
        if (characterHandle.WeaponAimComponent == null)
            characterOrient2D = new Vector2(1, 1);
        else
        {
            characterOrient2D = new Vector2(characterHandle.WeaponAimComponent.CurrentAim.x, characterHandle.WeaponAimComponent.CurrentAim.y);
        }
        if (characterOrient2D.x < 0)
            characterModel.transform.localScale = new Vector3(-1, 1, 1);
        else
            characterModel.transform.localScale = new Vector3(1, 1, 1);

        character._animator.SetFloat("xDir", characterOrient2D.x);
        character._animator.SetFloat("yDir", characterOrient2D.y);
    }
}
