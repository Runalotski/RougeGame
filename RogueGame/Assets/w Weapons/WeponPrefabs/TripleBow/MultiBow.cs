using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiBow : MonoBehaviour, IWeapon
{
    public Transform WeaponOwner { get; set; }
    public Transform WeaponTransform { get; set; }

    public Transform projectile;

    public int numberOfShots;
    public float spreadAngle;

    /// <summary>
    /// How many times a second will the wepon attack
    /// </summary>
    public float _attackSpeed;
    public float WeaponAttackSpeed { get { return _attackSpeed; } set { _attackSpeed = value; } }

    public bool WeaponWaitingForCooldown { get; set; }
    public string targetTag { get; set; }

    private IEnumerator coroutine;

    public float damage;
    public DamageTypes damageType;

    // Start is called before the first frame update
    void Start()
    {
        coroutine = AttackTimer();
        WeaponWaitingForCooldown = false;
    }

    public void WeaponAttack(Vector3 firePostion, Vector3 TargetPostion, string targetTag)
    {
        if (!WeaponWaitingForCooldown)
        {
            Vector3 shootDir;
            float angle = 0;

            //Mouse has hit a valid location to shoot at
            //TODO: We dont need a ray cast for this, get mouse position when we click!!
            shootDir = TargetPostion - firePostion;
            shootDir.y = 0;
            shootDir.Normalize();
            angle = Mathf.Acos(Vector3.Dot(Vector3.forward, shootDir)) * Mathf.Rad2Deg;

            if (TargetPostion.x < transform.position.x)
                angle = 360 - angle;


            //We need, Prefab (Local), Position, Rotation / Direction, and then set the damage;
            //We Need, Who is shooting and where in the world we are shooting

            for (int i = 0; i < numberOfShots; i++)
            {
                int offset = (numberOfShots / 2) - i;

                Transform clone = Instantiate(projectile, firePostion + (shootDir) + new Vector3(offset * shootDir.z, 0.5f, offset * -shootDir.x), Quaternion.Euler(0, angle + (offset * spreadAngle), 0)) as Transform;
                clone.GetComponent<BowProjectileScript>().Init(targetTag, new DamageClass(damage, damageType));

                if (WeaponOwner.GetComponent<Actor>().equippedDamageModifiers.Count > 0)
                {
                    foreach (DamageModifier abi in WeaponOwner.GetComponent<Actor>().equippedDamageModifiers)
                    {
                        if (abi.isActive)
                        {

                            DamageClass cloneDam = clone.GetComponent<BowProjectileScript>().damage;

                            cloneDam = abi.ApplyDamageMod(cloneDam);

                            clone.GetComponent<BowProjectileScript>().damage = cloneDam;
                        }
                    }
                }
            }

            WeaponWaitingForCooldown = true;
            StartCoroutine(AttackTimer());
        }
    }

    IEnumerator AttackTimer()
    {
        yield return new WaitForSeconds(1 / _attackSpeed);
        WeaponWaitingForCooldown = false;
        yield return null;

    }
}
