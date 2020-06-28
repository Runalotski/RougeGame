using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBossStage0 : MonsterActor, IBossStageEntity
{
    private Vector3 InitialVelocity;

    private Vector3 LastFrameVelocity;
    private Rigidbody rb;

    private bool CollidedThisFrame = false;

    Vector3 ColNormal;

    public BossActor ParentBoss { get; set; }

    public float damagePerHit;
    bool hitPlayerThisFrame = false;

    void Awake()
    {
        float rnd = Random.Range(0.2f, 0.8f);
        InitialVelocity = new Vector3(rnd, 0, 1 - rnd) * speed;

        init();
    }

    private void Start()
    {
        StartMoving();
    }

    private void StartMoving()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = InitialVelocity;
    }

    // Update is called once per frame
    private void Update()
    {

        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        LastFrameVelocity = rb.velocity;

        if (rb.velocity.magnitude == 0 && LastFrameVelocity.magnitude == 0)
        {
            Debug.LogError("I got stuck so I called start moving again");
            StartMoving();
        }

        if(Mathf.Abs(rb.velocity.x) < 0.1f || Mathf.Abs(rb.velocity.z) < 0.1f)
        {
            Debug.LogError("I am not moving in an intresting way so randomized my direction");
            StartMoving();
        }

        CollidedThisFrame = false;
        hitPlayerThisFrame = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!CollidedThisFrame)
        {
            ColNormal = collision.contacts[0].normal;
        }
        Bounce(collision.contacts[0].normal);
        CollidedThisFrame = true;

        //if we collide with the player deal damage to it
        if (collision.transform.root.tag == "Player" && !hitPlayerThisFrame)
        {
            collision.transform.root.GetComponent<PlayerActor>().TakeDamage(damagePerHit, DamageTypes.Poison);
            hitPlayerThisFrame = transform;
        }
    }

    private void Bounce(Vector3 collisionNormal)
    {
        float velocity = LastFrameVelocity.magnitude;
        if (!CollidedThisFrame)
        {
            Vector3 direction = Vector3.Reflect(LastFrameVelocity.normalized, collisionNormal);
            rb.velocity = direction * Mathf.Max(velocity, speed);
        }

        else
        {
            Vector3 direction = Vector3.Reflect(LastFrameVelocity.normalized, (collisionNormal + ColNormal).normalized);
            rb.velocity = (direction * Mathf.Max(velocity, speed));
            ColNormal = collisionNormal + ColNormal;
        }
    }

    public override void Die()
    {
        rb.detectCollisions = false;
        EntityDeath(gameObject);
    }

    public override void TakeDamage(float damage, DamageTypes damageType)
    {
        ApplyDamageToParentBoss(damageResistance.CalculateDamagetoTake(damage, damageType));
        base.TakeDamage(damage, damageType);
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
