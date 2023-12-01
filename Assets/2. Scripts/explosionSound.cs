using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosionSound : MonoBehaviour
{
    [SerializeField] AudioClip sound;

    private void OnDisable()
    {
        if (this.transform.parent.GetComponent<MachinGunS_Bullet>().soundRef == 5)
        {
            this.transform.parent.GetComponent<AudioSource>().PlayOneShot(sound);

        }
    }
}
