using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowProjectileScript : MonoBehaviour
{

    public bool playerProjectile;
    
    public float speed;

    public DamageClass damage;

    public string targetTag;

    public void Init(string targetTag, DamageClass damage)
    {
        Destroy(this.gameObject, 10);
        this.targetTag = targetTag;
        this.damage = damage;

        playerProjectile = (targetTag == "Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.transform.root.tag == targetTag)
        {
            col.transform.root.GetComponent<Actor>().TakeDamage(damage);
            Destroy(this.gameObject);
        }
    }
}
