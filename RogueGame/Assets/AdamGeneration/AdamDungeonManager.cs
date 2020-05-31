using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        dungeonScale = DungeonScale;

        Random.InitState((int)System.DateTime.Now.Ticks);

        dungeonData = new AdamDungeonData(dungeonType, RoomCount);

        dungeonRenderer.SpawnRoomAssets(dungeonData.grid, DungeonScale, dungeonData.SpawnPoint, dungeonData.bossRoom);

        player.position = new Vector3(dungeonData.SpawnPoint.x * DungeonScale, 0, dungeonData.SpawnPoint.z * DungeonScale);
    }

    private void Update()
    {
        //dungeonRenderer.DisplayerPlayerRoom(dungeonData.grid, DungeonScale, dungeonData.SpawnPoint, dungeonData.bossRoom, player);
    }

    public static Vector3 GetDungeonPosition(Vector3 position)
    {
        return new Vector3((int)((position.x - (dungeonScale / 2)) / dungeonScale), 0, (int)((position.z - (dungeonScale / 2)) / dungeonScale));
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
