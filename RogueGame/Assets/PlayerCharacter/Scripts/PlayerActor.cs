using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActor : Actor
{
    //public Transform playerManager;
    //PlayerManager manager;
    public GameManager gameManager;
    public DungeonManager dungeonManager;

    public Transform StartWeapon;

    [HideInInspector]
    public Transform ActiveWeapon { get; set; }

    [HideInInspector]
    public List<Transform> CarriedWeapons { get; set; }

    private void Start()
    {

        if (StartWeapon != null)
        {
            Transform weapon = Instantiate(StartWeapon);
            GetComponent<IWeaponUser>().PickUpWeapon(weapon);
        }
    }

    public override void TakeDamage(DamageClass damage)
    {
        health -= damageResistance.CalculateDamagetoTake(damage.baseDamage, damage.damageType);

        if (health <= 0)
            Die();
    }

    public override void Die()
    {
        //We may not have game manager in the Hub world
        if (gameManager != null)
        {
            gameManager.PlayerDied();

            GetComponent<MyCharacterController>().enabled = false;

        }
    }

    protected override void init()
    {
        throw new System.NotImplementedException();
    }

    public void UpdateCurrentRoom()
    {
        //We may not have dungeon manager in the Hub world
        if (dungeonManager != null)
        {
            room = dungeonManager.PlayerDungeonPos();
        }
    }   

}
