using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{ 
    Transform WeaponOwner { get; set; }

    Transform WeaponTransform { get; set; }

    void WeaponAttack(Vector3 firePostion, Vector3 TargetPostion, string targetTag);

    float WeaponAttackSpeed { get; set; }

    bool WeaponWaitingForCooldown { get; set; }
}
