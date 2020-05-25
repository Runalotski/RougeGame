using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdamDungeonManager : MonoBehaviour
{
    public enum TileTypes { Floor, Wall};

    public static AdamDungeonData dungeonData;
    public static AdamDungeonRenderer dungeonRenderer;

    public int DungeonLength;

    // Start is called before the first frame update
    void Start()
    {
        

        Random.seed = (int)System.DateTime.Now.Ticks;

        List<DungeonNode> test = new List<DungeonNode>();

        dungeonData = new AdamDungeonData(DungeonLength);
        dungeonRenderer = new AdamDungeonRenderer();

        //dungeonRenderer.Render(dungeonData.grid);
    }

    private void OnDrawGizmos()
    {
        if (dungeonData != null && dungeonData.HasPath())
        {
            for (int i = 0; i < /*(Time.timeSinceLevelLoad * 8) % */ (dungeonData.path.Count - 1); i++)
            {
                if (i == 0)
                    Gizmos.color = Color.green;
                else
                    Gizmos.color = Color.red;

                Gizmos.DrawLine(new Vector3(dungeonData.path[i].x + 0.5f, 0, dungeonData.path[i].z + 0.5f),
                                new Vector3(dungeonData.path[i + 1].x + 0.5f, 0, dungeonData.path[i + 1].z + 0.5f));

                Gizmos.DrawSphere(new Vector3(dungeonData.path[i].x + 0.5f, 0, dungeonData.path[i].z + 0.5f), 0.2f);
            }

            Gizmos.DrawSphere(new Vector3(dungeonData.path[dungeonData.path.Count - 1].x + 0.5f, 0, dungeonData.path[dungeonData.path.Count - 1].z + 0.5f), 0.2f);
        }

    }
}
