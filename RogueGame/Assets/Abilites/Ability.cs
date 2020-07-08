using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://www.youtube.com/watch?v=bvRKfLPqQ0Q

public abstract class Ability : ScriptableObject
{
    public string aName = "new ability";
    public Sprite aSprite;
    //public AudioClip aSound;
    public float aBaseCoolDown = 1f;
    public float aBaseActiveTime = 1f;
    public bool isActive = false;

    public abstract void Initialise();
    public abstract void TriggerAbility(GameObject obj);
}
