using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// The Base class for NPC Actors
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public abstract class MonsterActor : Actor
{
    protected NavMeshAgent agent;
    public DungeonManager dungeonManager;

    /// <summary>
    /// Will this mob use the navMesh or custome controller
    /// </summary>
    [SerializeField]
    public bool useNavMeshOnSpawn;

    /// <summary>
    /// the target the NPC attacking
    /// </summary>
    public Transform target;

    public override void TakeDamage(float damage, DamageTypes damageType)
    {
        health -= damageResistance.CalculateDamagetoTake(damage, damageType);

        if (health <= 0)
            Die();
    }

    protected override void init()
    {
        base.init();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
    }

    public override void Die()
    {
        Destroy(gameObject);
    }
}
