using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    public AdamDungeonManager dungeonManager;

    public Transform slimePrefab;
    public Transform rangerPrefab;

    public void SpawnSlimes(List<DungeonNode> rooms, float scale)
    {
        foreach(DungeonNode node in rooms)
        {
            for (int i = 0; i < Random.Range(1, 10); i++)
            {
                Transform slime = Instantiate(slimePrefab, node.transform.position + new Vector3(Random.Range((-scale / 2) + 2, (scale / 2) - 2), -5, Random.Range((-scale / 2) + 2, (scale / 2) - 2)), Quaternion.identity) as Transform;
                slime.GetComponent<SlimeScript>().dungeonManager = dungeonManager.GetComponent<AdamDungeonManager>();
                slime.GetComponent<SlimeScript>().room = new Vector3(node.x, 0, node.z);
                slime.name = "Slime " + node.x + "," + node.z + " " + i;
                AdamDungeonManager.dungeonData.grid[node.x, node.z].enemies.Add(slime);
            }
        }
    }

    public void SpawnRangers(List<DungeonNode> rooms, float scale)
    {
        foreach (DungeonNode node in rooms)
        {
            for (int i = 0; i < Random.Range(1, 10); i++)
            {
                Transform ranger = Instantiate(rangerPrefab, node.transform.position + new Vector3(Random.Range((-scale / 2) + 2, (scale / 2) - 2), -5, Random.Range((-scale / 2) + 2, (scale / 2) - 2)), Quaternion.identity) as Transform;
                ranger.GetComponent<RangerScript>().dungeonManager = dungeonManager.GetComponent<AdamDungeonManager>();
                ranger.GetComponent<RangerScript>().room = new Vector3(node.x, 0, node.z);
                ranger.name = "Ranger " + node.x + "," + node.z + " " + i;
                AdamDungeonManager.dungeonData.grid[node.x, node.z].enemies.Add(ranger);
            }
        }
    }
}