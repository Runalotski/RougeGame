using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    public Transform playerSpawn;

    // Start is called before the first frame update
    void Start()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        player.transform.position = playerSpawn.position;
        player.GetComponent<MyCharacterController>().enabled = true;
        player.GetComponent<Actor>().health = player.GetComponent<Actor>().maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
