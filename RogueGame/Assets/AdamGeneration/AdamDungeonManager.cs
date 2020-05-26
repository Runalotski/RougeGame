using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdamDungeonManager : MonoBehaviour
{
    public enum TileTypes { Floor, Wall};

    public Transform player;

    public static AdamDungeonData dungeonData;
    public AdamDungeonRenderer dungeonRenderer;

    public float DungeonScale;
    public int DungeonLength;

    // Start is called before the first frame update
    void Start()
    {
        Random.seed = (int)System.DateTime.Now.Ticks;

        List<DungeonNode> test = new List<DungeonNode>();

        dungeonData = new AdamDungeonData(DungeonLength);

        dungeonRenderer.Render(dungeonData.grid, DungeonScale);

        //player.GetComponent<CharacterController>(). enabled = false;
        player.position = new Vector3(dungeonData.SpawnPoint.x * DungeonScale, 0, dungeonData.SpawnPoint.z * DungeonScale);
        //Camera.main.transform.position = player.position + new Vector3(0, 4, 4);
        //player.GetComponent<CharacterController>().enabled = true;
    }

    private void OnDrawGizmos()
    {
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

    }
}
