using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{

    public float BossMaxHealth = 100f;
    public float BossHealth;


    // Start is called before the first frame update
    void Awake()
    {
        BossHealth = BossMaxHealth;
    }

    // Update is called once per frame
    void Update()
    {

        BossHealth -= 5f * Time.deltaTime;

        print(BossHealth);
       
    }

    
}
