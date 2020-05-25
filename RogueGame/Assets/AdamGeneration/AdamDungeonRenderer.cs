using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdamDungeonRenderer
{

    public AdamDungeonRenderer()
    {

    }

    public void Render(int[,] gridData)
    {


        for(int i = 0; i < gridData.GetLength(0); i++)
        {
            for(int j = 0; j < gridData.GetLength(1); j++)
            {
                Debug.Log("(" + i + "," + j + ")");

                switch(gridData[i,j])
                {
                    case (int)AdamDungeonManager.TileTypes.Floor:
                        RenderFloor(i, j);
                        break;
                    case (int)AdamDungeonManager.TileTypes.Wall:
                        RenderWall(i, j);
                        break;

                }

            }
        }
    }


    void RenderFloor(int x, int z)
    {
        Debug.Log("Rendering a Floor");
        Transform t = Resources.Load<Transform>("DebugTiles/Floor") as Transform;
        AdamDungeonManager.Instantiate(t, new Vector3(x, 0, z), Quaternion.identity);
    }

    void RenderWall(int x, int z)
    {
        Debug.Log("Rendering a Wall");
        Transform t = Resources.Load<Transform>("DebugTiles/Wall") as Transform;
        AdamDungeonManager.Instantiate(t, new Vector3(x, 0, z), Quaternion.identity);
    }
}
