using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageNumbersPro;
using MoreMountains.TopDownEngine;
using MoreMountains.Tools;

public class FloatingDamageText : MonoBehaviour, MMEventListener<MMDamageTakenEvent>
{
    [SerializeField]
    private DamageNumber damageNumber;
    private Health health;
    private bool isBoss;


    private void Start()
    {
        health = this.GetComponent<Health>();
        isBoss = this.gameObject.name == "Boss" ? true : false;
    }
    public void FloatingText()
    {
        damageNumber.Spawn(new Vector3(transform.position.x, transform.position.y + 0.5f, 0), health.LastDamage);

    }
    public void OnMMEvent(MMDamageTakenEvent damageTakenEvent)
    {
        if(damageTakenEvent.AffectedHealth == health)
        {
            Debug.Log("Damage : " + damageTakenEvent.DamageCaused);
            if(isBoss)
                damageNumber.Spawn(new Vector3(transform.position.x, transform.position.y + 3f, 0), damageTakenEvent.DamageCaused);
            else
                damageNumber.Spawn(new Vector3(transform.position.x, transform.position.y + 1.5f, 0), damageTakenEvent.DamageCaused);
        }

    }

    private void OnEnable()
    {
        this.MMEventStartListening<MMDamageTakenEvent>();
    }

    private void OnDisable()
    {
        this.MMEventStopListening<MMDamageTakenEvent>();
    }

}
