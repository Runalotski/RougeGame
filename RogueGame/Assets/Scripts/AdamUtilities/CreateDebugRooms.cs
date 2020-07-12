using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateDebugRooms : MonoBehaviour
{
    public Transform room0;

    List<Transform> toSave = new List<Transform>();

    public bool generatePrefabs = false;
    public bool SavePrefabs = false;

    public bool GetBit(byte b, int bitNumber)
    {
        return (b & (1 << bitNumber)) != 0;
    }

    void GeneratePrefabs()
    {
        for (byte i = 0; i <= 255; i++)
        {
            Transform room = null;

            bool keep = true;

            for (int j = 0; j < 4; j++)
            {
                if (GetBit(i, j) && GetBit(i, j + 4))
                {
                    keep = false;
                    break;
                }
                else if (j == 0)
                {
                    room = Instantiate(room0, Vector3.zero, Quaternion.identity);
                    room.name = "Room" + i;
                }

                if (GetBit(i, j) || GetBit(i, j + 4))
                {
                    Destroy(room.Find("RoomGraphics/Door" + j).gameObject);
                }

                if (GetBit(i, j + 4))
                {
                    Destroy(room.Find("RoomGraphics/Wall" + (j)).gameObject);
                }
            }

            if (keep)
                toSave.Add(room);

            //PrefabUtility.SaveAsPrefabAsset(room.gameObject, localPath);
            //Destroy(room.gameObject);

            if (i == 255)
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(SavePrefabs)
        {
            foreach(Transform t in toSave)
            {
                string localPath = "Assets/Resources/TileSets/DebugTiles/" + t.name + ".prefab";
                //PrefabUtility.SaveAsPrefabAsset(t.gameObject, localPath);
            }

            SavePrefabs = false;
        }
        else if(generatePrefabs)
        {
            GeneratePrefabs();
            generatePrefabs = false;
        }
    }
}
