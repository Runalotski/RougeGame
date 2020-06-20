using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActor : Actor
{
    public Transform playerManager;
    PlayerManager manager;

    private void Awake()
    {
        manager = playerManager.GetComponent<PlayerManager>();
    }

    public override void TakeDamage(float damage, DamageTypes damageType)
    {
        health -= damageResistance.CalculateDamagetoTake(damage, damageType);
    }

    protected override void Die()
    {
        Destroy(gameObject);
    }

    protected override void init()
    {
        throw new System.NotImplementedException();
    }
}
