using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeScript : MonsterActor
{

    public float damagePerSecond;

    private void Awake()
    {
        init();
    }

    private void Start()
    {
        target = dungeonManager.PlayerTransform();
    }

    void Update()
    {
        //if target is in the same room move to collide with it
        if (agent.enabled && target != null && dungeonManager.PlayerDungeonPos() == room)
        {
            agent.destination = dungeonManager.PlayerTransform().position;
            agent.isStopped = false;
        }
    }


    private void OnTriggerStay(Collider collision)
    {
        //if we collide with the player deal damage to it
        if (collision.transform.root.tag == "Player")
            collision.transform.root.GetComponent<PlayerActor>().TakeDamage(damagePerSecond * Time.deltaTime, DamageTypes.Poison);
    }
}
