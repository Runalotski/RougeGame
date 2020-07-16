using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The base class for all abilities, This my need to branch to be come, Movement Abilities,
/// Damage Abilities, Resistance Abilites ect
/// </summary>
public abstract class Ability : ScriptableObject
{
    [HideInInspector]
    public Actor owner;

    public string aName = "new ability";
    public Sprite aSprite;

    //The audio clip will be used at some point when we want a sound to trigger
    //public AudioClip aSound;
    public float aBaseCoolDown = 1f;
    public float aBaseActiveTime = 1f;
    public bool isActive = false;

    public abstract void Initialise(Actor actor);

    //General Case for abilities
    public abstract void TriggerAbility();
}
