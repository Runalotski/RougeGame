using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponClass
{
    byte handedness { get; set; }

    Transform owner { get; set; }

    void Attack();

    float attackSpeed { get; set; }
}
