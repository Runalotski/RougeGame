using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A package of damage form a weapon for affect.
/// </summary>
public class DamageClass
{
    //Base damage based on the weapon and actor stats
    public float baseDamage;

    //What multiplyer to add to the base attack
    public float damageMultiplyer = 1;

    //Damage Changed by a flat amount after the multiplyer has been applied
    public float damageShift;

    public DamageTypes damageType;

    public float TotalDamage {get { return (baseDamage * damageMultiplyer) + damageShift; }}

    public DamageClass(float baseDamage, DamageTypes damageType)
    {
        this.baseDamage = baseDamage;
        this.damageType = damageType;
    }
}
