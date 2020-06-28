using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DungeonRenderer : MonoBehaviour
{
    string pathToRoomPrefabs = "TileSets/DebugTiles/Room";

    public void SpawnRoomAssets(DungeonNode[,] gridData, float scale , DungeonNode startRoom ,DungeonNode bossRoom)
    {
        

        for (int i = 0; i < gridData.GetLength(0); i++)
        {
            for(int j = 0; j < gridData.GetLength(1); j++)
            {
                if (gridData[i, j].roomShapeFlag > 0)
                {
                    gridData[i, j].transform = Instantiate(Resources.Load<Transform>(pathToRoomPrefabs + gridData[i, j].roomShapeFlag), new Vector3(i * scale, 0, j * scale), Quaternion.identity, DungeonManager.dungeonParent) as Transform;
                    gridData[i, j].transform.Find("RoomGraphics").localScale = new Vector3(scale,scale,scale);
                    gridData[i, j].transform.name = ("Room(" + i + "," + j + ")");

                    if (gridData[i, j].x == startRoom.x && gridData[i, j].z == startRoom.z)
                        MakeTestRoomChangeColour(gridData[startRoom.x, startRoom.z].transform, new Color(0, 1, 0));

                    if (gridData[i, j].x == bossRoom.x && gridData[i, j].z == bossRoom.z)
                        MakeTestRoomChangeColour(gridData[bossRoom.x, bossRoom.z].transform, new Color(1, 0, 0));
                }
            }
        }

        
    }

    public void DisplayerPlayerRoom(DungeonNode[,] gridData, float scale, DungeonNode startRoom, DungeonNode bossRoom, Transform player)
    {
        Vector3 playerPos = DungeonManager.GetDungeonPosition(player.position);

        DungeonNode node = gridData[(int)playerPos.x + 1, (int)playerPos.z + 1];

        if (node.transform == null)
        {
            node.transform = Instantiate(Resources.Load<Transform>(pathToRoomPrefabs + node.roomShapeFlag), new Vector3(node.x * scale, 0, node.z * scale), Quaternion.identity) as Transform;
            node.transform.Find("RoomGraphics").localScale = new Vector3(scale, scale, scale);

            if(node.x == startRoom.x && node.z == startRoom.z)
                MakeTestRoomChangeColour(gridData[startRoom.x, startRoom.z].transform, new Color(0, 1, 0));

            if(node.x == bossRoom.x && node.z == bossRoom.z)
                MakeTestRoomChangeColour(gridData[bossRoom.x, bossRoom.z].transform, new Color(1, 0, 0));
        }
    }

    void MakeTestRoomChangeColour(Transform t, Color c)
    {
        List<Transform> objectsToCheck = new List<Transform>();
        List<Transform> nextToCheck = new List<Transform>();

        objectsToCheck.Add(t);

        while (objectsToCheck.Count > 0)
        {
            for(int i = 0; i < objectsToCheck.Count; i++)
            {

                if (objectsToCheck[i].childCount > 0)
                {
                    for (int j = 0; j < objectsToCheck[i].childCount; j++)
                    {
                        nextToCheck.Add(objectsToCheck[i].GetChild(j));
                    }
                }
                else
                {
                    objectsToCheck[i].GetComponent<Renderer>().material.color = c;
                }
            }

            objectsToCheck.Clear();
            objectsToCheck.AddRange(nextToCheck);
            nextToCheck.Clear();
        }
    }
}
