using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public abstract class MonsterActor : Actor
{
    protected NavMeshAgent agent;

    public override void TakeDamage(float damage, DamageTypes damageType)
    {
        health -= damageResistance.CalculateDamagetoTake(damage, damageType);

        if (health <= 0)
            Die();
    }

    protected override void Die()
    {
        Destroy(gameObject);
    }
}
