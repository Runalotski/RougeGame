using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeScript : MonsterActor
{
    public AdamDungeonManager dungeonManager;

    public Transform Player;


    Vector3 returnPos = Vector3.zero;

    public float damagePerSecond;

    public float MaxHealth;

    private void Awake()
    {
        init();
    }

    private void Start()
    {
        Player = dungeonManager.PlayerTransform();
        returnPos = transform.position;
    }

    void Update()
    {
        if (agent.enabled)
        {
            if (dungeonManager.PlayerDungeonPos() == room)
            {
                agent.destination = dungeonManager.PlayerTransform().position;
                agent.isStopped = false;

            }
            else
            {
                transform.GetComponent<NavMeshAgent>().SetDestination(returnPos);
            }
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.transform.root.tag == "Player")
            collision.transform.root.GetComponent<PlayerActor>().TakeDamage(damagePerSecond * Time.deltaTime, DamageTypes.Poison);
    }

    protected override void init()
    {
        maxHealth = MaxHealth;
        health = MaxHealth;
        agent = transform.GetComponent<NavMeshAgent>();
    }
}
