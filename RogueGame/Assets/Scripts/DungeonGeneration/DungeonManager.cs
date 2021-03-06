using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DungeonManager : MonoBehaviour
{
    public enum TileTypes { Floor, Wall };

    public Transform player;

    public static DungeonData dungeonData;
    public DungeonRenderer dungeonRenderer;

    public DungeonData.DungeonTypes dungeonType;

    public float DungeonScale;
    public int RoomCount;

    public static float dungeonScale;

    public Transform DungeonParent;
    public static Transform dungeonParent;

    public Transform Mobspawner;

    public Transform doors;

    public Transform gameManager;

    private void Awake()
    {
        dungeonParent = DungeonParent;
        dungeonScale = DungeonScale;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Player keeps falling through the world on load scense sometimes....
        //Wil try disabling CC incase that is problem in longer dungeon creation times
        player = GameObject.FindGameObjectWithTag("Player").transform;
        DisablePlayer();

        doors.GetChild(0).localScale = new Vector3(DungeonScale, DungeonScale, DungeonScale);

        Random.InitState((int)System.DateTime.Now.Ticks);

        dungeonData = new DungeonData(dungeonType, RoomCount);

        dungeonRenderer.SpawnRoomAssets(dungeonData.grid, DungeonScale, dungeonData.SpawnPoint, dungeonData.bossRoom);

        dungeonParent.GetComponent<NavigationBaker>().AddSurfacesOfChildren();

        dungeonParent.GetComponent<NavigationBaker>().Build();

        List<DungeonNode> Mobrooms = new List<DungeonNode>();
        
        foreach(DungeonNode node in dungeonData.spawnedRooms)
        {
            if(node != dungeonData.SpawnPoint && node != dungeonData.bossRoom)
            {
                Mobrooms.Add(node);
            }
        }

        Mobspawner.GetComponent<MobSpawner>().SpawnAllMobsInDungeon(Mobrooms, dungeonData.bossRoom, dungeonScale);

        Vector3 playerSpawnPos = new Vector3(dungeonData.SpawnPoint.x * DungeonScale, 0.1f, dungeonData.SpawnPoint.z * DungeonScale);

        player.position = playerSpawnPos;
        Debug.Log("Spawning the Player at " + playerSpawnPos);

        EnablePlayer();

    }

    private void DisablePlayer()
    {
        player.GetComponent<CharacterController>().enabled = false;
    }

    private void EnablePlayer()
    {
        player.GetComponent<CharacterController>().enabled = true;
    }

    private void Update()
    {
        if (player != null)
        {
            DoorsFollowPlayer();

            DungeonNode playerNode = GetPlayerDungeonNode();

            if (!playerNode.enemiesCleard && !playerNode.enemiesAreActive)
            {
                playerNode.enemiesAreActive = true;
                InitiateEnemies(playerNode);
            }

            if(IsRoomClear(playerNode) && playerNode == dungeonData.bossRoom)
            {
                gameManager.GetComponent<GameManager>().BossDefeated();
            }
        }
    }

    public bool IsRoomClear(DungeonNode node)
    {
        foreach (Transform t in node.enemies)
        {
            if (t != null)
            {
                node.enemiesCleard = false;
                return false;
            }
        }

        node.enemiesCleard = true;

        return true;
    }

    public void InitiateEnemies(DungeonNode node)
    {
        foreach(Transform t in node.enemies)
        {
            //move the moster above the ground
            t.position = new Vector3(t.position.x, 0, t.position.z);

            //enable the moster script to take damage and take actions
            MonsterActor actor = t.GetComponent<MonsterActor>();
            actor.enabled = true;

            //enable the navmesh if the moster uses it
            t.GetComponent<NavMeshAgent>().enabled = actor.useNavMeshOnSpawn;
        }
    }

    public static Vector3 GetDungeonPosition(Vector3 position)
    {
        return new Vector3((int)((position.x - (dungeonScale / 2)) / dungeonScale) + 1, 0, (int)((position.z - (dungeonScale / 2)) / dungeonScale) + 1);
    }

    public Vector3 PlayerDungeonPos()
    {
        return GetDungeonPosition(player.position);
    }

    public DungeonNode GetPlayerDungeonNode()
    {
        
        Vector3 playerPos = PlayerDungeonPos();
        return dungeonData.grid[(int)playerPos.x, (int)playerPos.z];
    }

    public Transform PlayerTransform()
    {
        return player;
    }

    public void DoorsFollowPlayer()
    {
        if (player != null && doors != null)
        {
            DungeonNode playerRoom = GetPlayerDungeonNode();

            if (playerRoom != null && playerRoom.transform != null)
            {
                doors.transform.position = playerRoom.transform.position;
                DoorsOpen(playerRoom.enemiesCleard);
            }
        }
    }

    public void DoorsOpen(bool open)
    {
        if(open)
            doors.position = new Vector3(doors.position.x, -5, doors.position.z);
        else
            doors.position = new Vector3(doors.position.x, 0, doors.position.z);
    }

    private void OnDrawGizmos()
    {
        /*
        if (dungeonData != null && dungeonData.HasPath())
        {
            for (int i = 0; i < dungeonData.path.Count - 1; i++)
            {
                if (i == 0)
                    Gizmos.color = Color.green;
                else
                    Gizmos.color = Color.red;

                Gizmos.DrawLine(new Vector3((dungeonData.path[i].x) * DungeonScale, 0, (dungeonData.path[i].z) * DungeonScale),
                                new Vector3((dungeonData.path[i + 1].x) * DungeonScale, 0, (dungeonData.path[i + 1].z) * DungeonScale));

                Gizmos.DrawSphere(new Vector3((dungeonData.path[i].x) * DungeonScale, 0, (dungeonData.path[i].z) * DungeonScale), 0.1f * DungeonScale);
            }

            Gizmos.DrawSphere(new Vector3((dungeonData.path[dungeonData.path.Count - 1].x) * DungeonScale, 0, (dungeonData.path[dungeonData.path.Count - 1].z) * DungeonScale), 0.1f * DungeonScale);
        }
        */
    }
}
