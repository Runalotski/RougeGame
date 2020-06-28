using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSlimeStage1 : MonsterActor, IBossStageEntity
{
    private Vector3 InitialVelocity;

    private Vector3 LastFrameVelocity;
    private Rigidbody rb;

    private bool CollidedThisFrame = false;

    Vector3 ColNormal;

    public BossActor ParentBoss { get; set; }

    void Awake()
    {
        float rnd = Random.Range(0.2f, 0.8f);
        InitialVelocity = new Vector3(rnd, 0, 1 - rnd) * speed;

        init();
    }

    public void Start()
    {
        StartMoving();
    }

    private void StartMoving()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = InitialVelocity;
    }

    // Update is called once per frame
    public void Update()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        LastFrameVelocity = rb.velocity;
        CollidedThisFrame = false;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (!CollidedThisFrame)
        {
            ColNormal = collision.contacts[0].normal;
        }
        Bounce(collision.contacts[0].normal);
        CollidedThisFrame = true;
    }

    private void Bounce(Vector3 collisionNormal)
    {
        var speed = LastFrameVelocity.magnitude;
        if (!CollidedThisFrame)
        {
            var direction = Vector3.Reflect(LastFrameVelocity.normalized, collisionNormal);
            //Debug.Log("Out Direction: " + direction);
            rb.velocity = direction * Mathf.Max(speed, speed);
            //print("I have not collided this frame");
        }

        else
        {
            var direction = Vector3.Reflect(LastFrameVelocity.normalized, (collisionNormal + ColNormal).normalized);
            //Debug.Log("Out Direction: " + direction);
            rb.velocity = (direction * Mathf.Max(speed, speed));
            ColNormal = collisionNormal + ColNormal;
            //print("Collided");
        }
    }

    public void ApplyDamageToParentBoss(float damage)
    {
        ParentBoss.bossStages.ApplyDamageToCurrentStage(damage, ParentBoss);
    }

    public void EntityDeath(GameObject g)
    {
        ParentBoss.DestroyEntity(g);
    }
}
