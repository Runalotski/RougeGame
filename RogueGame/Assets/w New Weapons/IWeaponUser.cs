using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface to allow an entity to use weapons
/// </summary>
public interface IWeaponUser
{
    void WeaponUserInit();

    IWeapon ActiveWeapon { get; set; }

    List<Transform> CarriedWeapons { get; set; }

    int MaxCarriedWeapons { get; set; }

    bool AttackedShouldTrigger();

    void Attack(string targetTag);

    void SwitchWeapon();

    void DropWeapon();

    void PickUpWeapon(Transform weaponT);
}
