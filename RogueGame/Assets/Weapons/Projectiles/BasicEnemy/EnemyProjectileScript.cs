using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileScript : MonoBehaviour
{

    public float speed;
    public float baseDamage;
    public DamageTypes damageType;
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
        if(col.transform.root.tag == "Player")
        {
            col.transform.root.GetComponent<PlayerActor>().TakeDamage(baseDamage, damageType);
            Destroy(this.gameObject);
        }

    }
}
