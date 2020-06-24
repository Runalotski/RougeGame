using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallCubeScript : MonoBehaviour
{

    private float SmallCubeSplitHealth;

    [SerializeField]
    private Vector3 InitialVelocity;

    [SerializeField]
    private float minVelocity = 10f;

    private Vector3 LastFrameVelocity;
    private Rigidbody rb;

    private bool CollidedThisFrame = false;


    public float MiniBossHealth;

    Vector3 ColNormal;


    // Start is called before the first frame update
    public void Start()
    {

        GameObject Boss = GameObject.Find("BossManager");
        BossManager bHealth = Boss.GetComponent<BossManager>();

        SmallCubeSplitHealth = bHealth.BossHealth;

        StartMoving();
    }

    // Update is called once per frame
    public void Update()
    {
        LastFrameVelocity = rb.velocity;
        CollidedThisFrame = false;

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (!CollidedThisFrame)
        {
            ColNormal = collision.contacts[0].normal;
        }

        //if (collision.gameObject.tag == "Boss")
        //{
        //    Physics.IgnoreCollision(collision.collider, collision.collider);
        //}

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
            rb.velocity = direction * Mathf.Max(speed, minVelocity);
            //print("I have not collided this frame");
        }

        else
        {
            var direction = Vector3.Reflect(LastFrameVelocity.normalized, (collisionNormal + ColNormal).normalized);
            //Debug.Log("Out Direction: " + direction);
            rb.velocity = (direction * Mathf.Max(speed, minVelocity));
            ColNormal = collisionNormal + ColNormal;
            //print("Collided");
        }
    }

    private void StartMoving()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = InitialVelocity;
    }


}
