using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowProjectileScript : MonoBehaviour
{
    
    public float speed;

    public DamageClass damage;
    

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 10);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * speed, Space.World);
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.transform.root.tag == "Enemy")
        {
            col.transform.root.GetComponent<MonsterActor>().TakeDamage(damage);
            Destroy(this.gameObject);
        }
    }
}
