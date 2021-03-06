using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class initialisePlayerOnLoadScript : MonoBehaviour
{

    public DungeonManager dungeonManager;

    // Start is called before the first frame update
    void Start()
    {
        PlayerActor actor = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerActor>();
        actor.dungeonManager = dungeonManager;
    }
}
