using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdamDungeonRenderer : MonoBehaviour
{
    public void Render(int[,] gridData, float scale)
    {
        string pathToRoomPrefabs = "TileSets/DebugTiles/Room";

        for (int i = 0; i < gridData.GetLength(0); i++)
        {
            for(int j = 0; j < gridData.GetLength(1); j++)
            {
                if (gridData[i, j] > 0)
                {
                    Transform room = Instantiate(Resources.Load<Transform>(pathToRoomPrefabs + gridData[i, j]), new Vector3(i * scale, 0, j * scale), Quaternion.identity) as Transform;
                    room.Find("RoomGraphics").localScale = new Vector3(scale,scale,scale);
                }
            }
        }
    }
}
