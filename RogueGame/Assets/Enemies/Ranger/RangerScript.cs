using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangerScript : MonsterActor
{
    public Transform projectile;

    bool canAttack = false;
    private IEnumerator coroutine;

    /// <summary>
    /// How many times a second will the wepon attack
    /// </summary>
    public float _attackSpeed;
    public float attackSpeed { get { return _attackSpeed; } set { _attackSpeed = value; } }

    private void Awake()
    {
        init();
    }

    // Start is called before the first frame update
    void Start()
    {
        target = dungeonManager.PlayerTransform();
        StartCoroutine(AttackTimer());
    }

    // Update is called once per frame
    void Update()
    {
        //If player target is in the same 2room attack it.
        if (agent.enabled && dungeonManager.PlayerDungeonPos() == room && canAttack)
        {
                Vector3 shootDir = Vector3.zero;
                float angle = 0;

                shootDir = target.position - transform.position;
                shootDir.y = 0;
                shootDir.Normalize();
                angle = Mathf.Acos(Vector3.Dot(Vector3.forward, shootDir)) * Mathf.Rad2Deg;

                if (target.position.x < transform.position.x)
                    angle = 360 - angle;


                Instantiate(projectile, transform.position + (shootDir) + new Vector3(0, 0.5f, 0), Quaternion.Euler(0, angle, 0));
                canAttack = false;
                StartCoroutine(AttackTimer());

                agent.destination = dungeonManager.PlayerTransform().position;
                agent.isStopped = false;
        }
    }

    /// <summary>
    /// Delay until next attack
    /// </summary>
    /// <returns></returns>
    IEnumerator AttackTimer()
    {
        yield return new WaitForSeconds(1 / _attackSpeed);
        canAttack = true;
        yield return null;

    }
}
