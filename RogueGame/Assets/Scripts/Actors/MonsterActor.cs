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

    public Transform boundsCollider;

    public override void TakeDamage(DamageClass damage)
    {
        health -= damageResistance.CalculateDamagetoTake(damage.TotalDamage, damage.damageType);

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
