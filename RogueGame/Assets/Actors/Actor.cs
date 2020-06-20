using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour
{
    public Vector3 room;

    protected float maxHealth;
    public float health;
    protected float speed;

    [SerializeField] protected DamageResistances damageResistance;

    public abstract void TakeDamage(float damage, DamageTypes damageType);
    protected abstract void Die();

    /// <summary>
    /// Set the starting variablles for the Actor
    /// </summary>
    protected abstract void init();
}
