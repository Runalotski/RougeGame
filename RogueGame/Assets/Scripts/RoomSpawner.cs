using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{

    public int openingDirection;
    // 1 = Needs a Top door
    // 2 = Needs a Right door
    // 3 = Needs a Bottom door
    // 4 = Needs a Left door

    private RoomTemplates templates;
    private int rand;
    public bool spawned = false;

    private void Start() 
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("Spawn", 0.1f);
    }

    private void Spawn()
    {
        if(spawned == false)
        {
            if (openingDirection == 3) //Needs a room with a Top door.
            {
                rand = Random.Range(0, templates.topRooms.Length); // Random range of top rooms.
                Instantiate(templates.topRooms[rand], transform.position, templates.topRooms[rand].transform.rotation); // Random amount of top rooms, using random top template at spawn positon and rotation.
            }

            else if (openingDirection == 4) //Needs a room with Right door.
            {
                rand = Random.Range(0, templates.rightRoom.Length); // Random range of right rooms.
                Instantiate(templates.rightRoom[rand], transform.position, templates.rightRoom[rand].transform.rotation); // Random amount of right rooms, using random right template at spawn positon and rotation.
            }

            else if (openingDirection == 1) //Needs a room with Bottom door.
            {
                rand = Random.Range(0, templates.bottomRooms.Length); // Random range of left rooms.
                Instantiate(templates.bottomRooms[rand], transform.position, templates.bottomRooms[rand].transform.rotation); // Random amount of bottom rooms, using random bottom template at spawn positon and rotation.
            }

            else if (openingDirection == 2) //Needs a room with Left door.
            {
                rand = Random.Range(0, templates.leftRooms.Length); // Random range of bottom rooms.
                Instantiate(templates.leftRooms[rand], transform.position, templates.leftRooms[rand].transform.rotation); // Random amount of left rooms, using random left template at spawn positon and rotation.
            }

            spawned = true;
        }

    }

    public void Update()
    {
        Object[] t = GameObject.FindObjectsOfTypeAll(typeof(RoomSpawner));
        foreach (Object obj in t)
        {
            RoomSpawner G = obj as RoomSpawner;
            if (G.transform.tag != "SpawnPoint") { Debug.LogError("I am not tagged correctly", G.transform); }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpawnPoint"))
        {
            if(other.GetComponent<RoomSpawner>().spawned == false && spawned == false)
            {
                Instantiate(templates.closedRooms, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            spawned = true;
        }
    }
}
