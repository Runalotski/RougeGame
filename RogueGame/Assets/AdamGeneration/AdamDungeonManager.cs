using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AdamDungeonManager : MonoBehaviour
{
    public enum TileTypes { Floor, Wall };

    public Transform player;

    public static AdamDungeonData dungeonData;
    public AdamDungeonRenderer dungeonRenderer;

    public AdamDungeonData.DungeonTypes dungeonType;

    public float DungeonScale;
    public int RoomCount;

    public static float dungeonScale;

    public Transform DungeonParent;
    public static Transform dungeonParent;

    public Transform Mobspawner;

    public Transform doors;

    private void Awake()
    {
        dungeonParent = DungeonParent;
        dungeonScale = DungeonScale;
    }

    // Start is called before the first frame update
    void Start()
    {
        doors.GetChild(0).localScale = new Vector3(DungeonScale, DungeonScale, DungeonScale);

        Random.InitState((int)System.DateTime.Now.Ticks);

        dungeonData = new AdamDungeonData(dungeonType, RoomCount);

        dungeonRenderer.SpawnRoomAssets(dungeonData.grid, DungeonScale, dungeonData.SpawnPoint, dungeonData.bossRoom);

        player.position = new Vector3(dungeonData.SpawnPoint.x * DungeonScale, 0, dungeonData.SpawnPoint.z * DungeonScale);

        dungeonParent.GetComponent<NavigationBaker>().AddSurfacesOfChildren();

        dungeonParent.GetComponent<NavigationBaker>().Build();

        List<DungeonNode> slimeRooms = new List<DungeonNode>();
        
        foreach(DungeonNode node in dungeonData.spawnedRooms)
        {
            if(node != dungeonData.SpawnPoint && node != dungeonData.bossRoom)
            {
                slimeRooms.Add(node);
            }
        }

        Mobspawner.GetComponent<MobSpawner>().SpawnSlimes(slimeRooms, dungeonScale);
        Mobspawner.GetComponent<MobSpawner>().SpawnRangers(slimeRooms, dungeonScale);
    }

    private void Update()
    {
        DoorsFollowPlayer();

        DungeonNode playerNode = GetPlayerDungeonNode();

        if (!playerNode.enemiesCleard && !playerNode.enemiesAreActive)
        {
            playerNode.enemiesAreActive = true;
            InitiateEnemies(playerNode);
        }

        IsRoomClear(playerNode);
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
            t.position = new Vector3(t.position.x, 0, t.position.z);
            t.GetComponent<NavMeshAgent>().enabled = true;
            t.GetComponent<MonsterActor>().enabled = true;
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
        DungeonNode playerRoom = GetPlayerDungeonNode();

        doors.transform.position = playerRoom.transform.position;
        DoorsOpen(playerRoom.enemiesCleard);
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
