using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    public DungeonManager dungeonManager;

    [Header("Bosses")]

    public Transform slimeBossPrefab;

    [Header("Enemies")]

    public Transform slimePrefab;
    public Transform rangerPrefab;

    public enum SpawnLocation { Center, Random}

    private void CreateMob(Transform prefab, DungeonNode location, float scale, SpawnLocation spawnLocationType)
    {

        Vector3 mobSpawnLocation = GetSpawnLocationInRoom(location, scale, spawnLocationType);

        Transform mob = Instantiate(prefab, mobSpawnLocation, Quaternion.identity) as Transform;
        mob.GetComponent<MonsterActor>().dungeonManager = dungeonManager.GetComponent<DungeonManager>();
        mob.GetComponent<MonsterActor>().room = new Vector3(location.x, 0, location.z);
        mob.name = prefab.name + location.x + "," + location.z + " " + DungeonManager.dungeonData.grid[location.x, location.z].enemies.Count;
        DungeonManager.dungeonData.grid[location.x, location.z].enemies.Add(mob);
        
    }

    /// <summary>
    /// Returns world coords for a mob to spawn in the room
    /// </summary>
    /// <param name="room">The room to spawn Mob</param>
    /// <param name="scale">The scale of dungeon</param>
    /// <param name="spawnLocationType">where should mob spawn in room</param>
    /// <returns></returns>
    private Vector3 GetSpawnLocationInRoom(DungeonNode room, float scale, SpawnLocation spawnLocationType)
    {
        switch (spawnLocationType)
        {
            case SpawnLocation.Center:
                return room.transform.position;
            case SpawnLocation.Random:
                //Pick a random spot in a square room with padding aound the edges for nav mesh agent width
                float navMeshPadding = 5;
                return room.transform.position + new Vector3(Random.Range((-scale / 2) + navMeshPadding, (scale / 2) - navMeshPadding), -5, Random.Range((-scale / 2) + navMeshPadding, (scale / 2) - navMeshPadding));
        }

        Debug.LogError(spawnLocationType.ToString() + " did not match the case, will spawn mob in center!");
        return Vector3.zero;
    }

    public void SpawnBossNExtToSpawnRoom(DungeonNode spawnRoom, float scale)
    {
        CreateMob(slimeBossPrefab, spawnRoom, scale, SpawnLocation.Center);
    }

    public void SpawnAllMobsInDungeon(List<DungeonNode> rooms, DungeonNode bossRoom, float scale)
    {
        foreach(DungeonNode node in rooms)
        {
            for (int i = 0; i < Random.Range(1, 10); i++)
            {
                CreateMob(slimePrefab, node, scale, SpawnLocation.Random);
            }

            for (int i = 0; i < Random.Range(1, 10); i++)
            {
                CreateMob(rangerPrefab, node, scale, SpawnLocation.Random);
            }
        }

        CreateMob(slimeBossPrefab, bossRoom, scale, SpawnLocation.Center);
    }
}