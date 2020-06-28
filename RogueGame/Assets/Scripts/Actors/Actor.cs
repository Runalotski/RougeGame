﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The base class for the Player and NPCs
/// </summary>
public abstract class Actor : MonoBehaviour
{
    /// <summary>
    /// The Room the Actor is in
    /// </summary>
    public Vector3 room;

    /// <summary>
    /// Maximum Health for the Actor
    /// </summary>
    [SerializeField]
    public float maxHealth;

    /// <summary>
    /// The Current Health of the actor
    /// </summary>
    public float health;

    /// <summary>
    /// Movement speed of the actor
    /// </summary>
    [SerializeField]
    protected float speed;

    /// <summary>
    /// The base resistances for the actor
    /// </summary>
    [SerializeField]
    protected DamageResistances damageResistance;

    /// <summary>
    /// Apply amount of damage to remove from the Actor
    /// </summary>
    /// <param name="damage">Base damage to take</param>
    /// <param name="damageType">The elemental tpy eof the damage</param>
    public abstract void TakeDamage(float damage, DamageTypes damageType);

    /// <summary>
    /// Called when Acotr is reduced to 0 health or less
    /// </summary>
    public abstract void Die();

    /// <summary>
    /// Set the starting variables for the Actor
    /// </summary>
    protected virtual void init()
    {
        health = maxHealth;
    }
}
