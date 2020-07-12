using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActor : Actor
{
    public Transform playerManager;
    PlayerManager manager;
    GameManager gameManager;
    public DungeonManager dungeonManager;

    private void Awake()
    {
        manager = playerManager.GetComponent<PlayerManager>();
        gameManager = playerManager.GetComponent<PlayerManager>().gameManager.GetComponent<GameManager>();
    }

    public override void TakeDamage(DamageClass damage)
    {
        health -= damageResistance.CalculateDamagetoTake(damage.baseDamage, damage.damageType);

        if (health <= 0)
            Die();
    }

    public override void Die()
    {
        gameManager.PlayerDied();

        GetComponent<MyCharacterController>().enabled = false;
        

        //Destroy(gameObject);
    }

    protected override void init()
    {
        throw new System.NotImplementedException();
    }

    public void UpdateCurrentRoom()
    {
        room = dungeonManager.PlayerDungeonPos();
    }   

}
