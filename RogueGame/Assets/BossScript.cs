using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    [SerializeField]
    private Vector3 InitialVelocity;

    [SerializeField]
    private float minVelocity = 10f;

    private Vector3 LastFrameVelocity;
    private Rigidbody rb;

    private bool CollidedThisFrame = false;


    public float MiniBossHealth;

    public GameObject BossSplitOnePrefab;
    public GameObject BossSplitTwoPrefab;

    Vector3 ColNormal;

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
        LastFrameVelocity = rb.velocity;
        CollidedThisFrame = false;

        GameObject Boss = GameObject.Find("BossManager");
        BossManager bHealth = Boss.GetComponent<BossManager>();

        MiniBossHealth = bHealth.BossHealth;


        BossSDeath();
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

    private void BossSDeath()
    {
        if (MiniBossHealth <= 50f)
        {

            GameObject BossSplitOne = Instantiate(BossSplitOnePrefab, transform.position, Quaternion.identity);
            //BossSplitOne.GetComponent<BossScript>().MiniBossHealth = MiniBossHealth * 0.30f;


            GameObject BossSplitTwo = Instantiate(BossSplitTwoPrefab, new Vector3(transform.position.x - 0.2f, transform.position.y, transform.position.z - 0.2f), Quaternion.identity);
            //BossSplitTwo.GetComponent<BossScript>().MiniBossHealth = MiniBossHealth * 0.30f;

            Destroy(this.gameObject);
        }
    }

}
