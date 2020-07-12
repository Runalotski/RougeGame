using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Ability/DamageModifier")]
public class DamageModifier : Ability
{
    public float damageMultiplyer = 2f;
    public float damageShift = 1f;

    //Add the multiplied damage to the base.
    public DamageClass ApplyDamageMod(DamageClass damage)
    {
        damage.damageMultiplyer += damageMultiplyer;
        damageShift += damageShift;

        return damage;
    }
    
    public override void Initialise(Actor actor)
    {
        actor.equippedDamageModifiers.Add(this);
        owner = actor;
    }

    public override void TriggerAbility()
    {
        isActive = true;
    }
}
