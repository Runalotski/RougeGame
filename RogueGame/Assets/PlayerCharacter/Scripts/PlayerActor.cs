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

    [HideInInspector]
    public int Level { get; private set; }

    [HideInInspector]
    public int CurrentXP { get; private set; }

    void Start()
    {
        Level = 1;

        if (StartWeapon != null)
        {
            Transform weapon = Instantiate(StartWeapon);
            GetComponent<IWeaponUser>().PickUpWeapon(weapon);
        }
    }

    public int NextLevelXP()
    {
        return Level * 100;
    }

    public void AddXP(int xpGain)
    {
        CurrentXP += xpGain;

        //reduce XP to 0 at level up but keep XP spill over
        if(CurrentXP >= NextLevelXP())
        {
            CurrentXP = CurrentXP - NextLevelXP();
            Level++;
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
