using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityStandardAssets._2D;

public class AdamDungeonData
{
    public enum DungeonTypes { Path, Dungeon };

    public int GridX { get; private set; }
    public int GridZ { get; private set; }

    public DungeonNode[,] grid;

    public DungeonNode SpawnPoint = new DungeonNode(0, 0);

    public List<DungeonNode> spawnedRooms = new List<DungeonNode>();

    public DungeonNode bossRoom;

    public enum Direction { North, East, South, West };

    public AdamDungeonData(DungeonTypes type, int length)
    {
        InitGrid(length, length);

        SpawnPoint = new DungeonNode(GridX / 2, GridX / 2);

        if (type == DungeonTypes.Path)
            CreatePath(SpawnPoint.x, SpawnPoint.z, length);
        else if (type == DungeonTypes.Dungeon)
            CreateDungeon(SpawnPoint.x, SpawnPoint.z, length);

        grid[SpawnPoint.x, SpawnPoint.z].enemiesCleard = true;
    }

    void InitGrid(int x, int z)
    {
        GridX = x;
        GridZ = z;

        grid = new DungeonNode[GridX, GridZ];

        for (int i = 0; i < GridX; i++)
        {
            for (int j = 0; j < GridZ; j++)
            {
                grid[i, j] = new DungeonNode(i,j,0);
            }
        }


    }

    /// <summary>
    /// Create a path of a set length and start position.
    /// </summary>
    /// <param name="length"></param>
    /// <param name="x"></param>
    /// <param name="z"></param>
    void CreatePath(int x, int z, int length)
    {
        List<DungeonNode> path = new List<DungeonNode>();
        path.Add(new DungeonNode(x, z));

        int pathStep = 0;

        path = ExtendPath(path, pathStep, length);
        AddPathToGrid(path);
        AssignBossRoomToPath(path);
    }

    void CreateDungeon(int x, int z, int maxRooms)
    {
        //set start room shape  
        grid[x, z].roomShapeFlag = (byte)Random.Range(1, 16);
        spawnedRooms.Add(grid[x, z]);

        List<DungeonNode> roomsToCreate = new List<DungeonNode>();
        roomsToCreate.AddRange(GetEmptyConnection(new DungeonNode(x, z)));

        ExpandDungeon(roomsToCreate, 1, maxRooms);
        SpawnPoint = grid[SpawnPoint.x, SpawnPoint.z];
        AssignBossRoomInDungeon(SpawnPoint);
    }

    /// <summary>
    /// Return the grid coord of rooms that dont exsist but a room is trying to connect to.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    DungeonNode[] GetEmptyConnection(DungeonNode node, List<DungeonNode> blackList = null)
    {
        List<DungeonNode> nodesToReturn = new List<DungeonNode>();

        DungeonNode north = new DungeonNode(node.x, node.z + 1, node);
        DungeonNode east = new DungeonNode(node.x + 1, node.z, node);
        DungeonNode south = new DungeonNode(node.x, node.z - 1, node);
        DungeonNode west = new DungeonNode(node.x - 1, node.z, node);

        //Check this door is not trying to go out of bound && the space it enters is empty && the new room will connect 
        if (CanExpand(node, Direction.North)) {
            //If black list is null then add the valid new room. If it is already added then we can skip over it
            if (blackList == null || !blackList.Contains(north))
            {
                nodesToReturn.Add(north);
            }
        }

        if (CanExpand(node, Direction.East)) { 
            if (blackList == null || !blackList.Contains(east))
            {
                nodesToReturn.Add(east);
            }
        }

        if (CanExpand(node, Direction.South)){
            if (blackList == null || !blackList.Contains(south))
            {
                nodesToReturn.Add(south);
            }
        }

        if (CanExpand(node, Direction.West)){
            if (blackList == null || !blackList.Contains(west))
            {
                nodesToReturn.Add(west);
            }
        }

        return nodesToReturn.ToArray();
    }

    bool CanExpand(DungeonNode node, Direction dir)
    {
        //Check this door is not trying to go out of bound && the space it enters is empty && the new room will connect 
        if (InGridBounds(node.x, node.z + 1) && grid[node.x, node.z + 1].roomShapeFlag == 0 && ((grid[node.x, node.z].roomShapeFlag & (1 << 0)) == (1 << 0)) && dir == Direction.North)
            return true;

        if (InGridBounds(node.x + 1, node.z) && grid[node.x + 1, node.z].roomShapeFlag == 0 && ((grid[node.x, node.z].roomShapeFlag & (1 << 1)) == (1 << 1)) && dir == Direction.East)
            return true;

        if (InGridBounds(node.x, node.z - 1) && grid[node.x, node.z - 1].roomShapeFlag == 0 && ((grid[node.x, node.z].roomShapeFlag & (1 << 2)) == (1 << 2)) && dir == Direction.South)
            return true;

        if (InGridBounds(node.x - 1, node.z) && grid[node.x - 1, node.z].roomShapeFlag == 0 && ((grid[node.x, node.z].roomShapeFlag & (1 << 3)) == (1 << 3)) && dir == Direction.West)
            return true;

        return false;
    }

    void ExpandDungeon(List<DungeonNode> roomsToCreate, int currentSize, int maxSize)
    {
        List<DungeonNode> nextRoomsToSpawn = new List<DungeonNode>();

        while (currentSize < maxSize)
        {
            nextRoomsToSpawn.Clear();
            nextRoomsToSpawn.AddRange(roomsToCreate);
            roomsToCreate.Clear();

            if(nextRoomsToSpawn.Count == 0)
            {

                DungeonNode[] edgeNodes = GetEdgeRooms();

                int rnd = Random.Range(0, edgeNodes.Length);

                DungeonNode selectedEdge = edgeNodes[rnd];

                DungeonNode[] dirNodes = { new DungeonNode(selectedEdge.x, selectedEdge.z + 1, selectedEdge),
                                    new DungeonNode(selectedEdge.x + 1, selectedEdge.z, selectedEdge),
                                    new DungeonNode(selectedEdge.x, selectedEdge.z - 1, selectedEdge),
                                    new DungeonNode(selectedEdge.x - 1, selectedEdge.z, selectedEdge)};


                List<byte> wallToRemove = new List<byte>();

                for (byte x = 0; x < dirNodes.Length; x++)
                {
                    if (InGridBounds(dirNodes[x]) && grid[dirNodes[x].x, dirNodes[x].z].roomShapeFlag == 0)
                    {
                        wallToRemove.Add(x);
                    }
                }

                rnd = Random.Range(0, wallToRemove.Count);

                grid[selectedEdge.x, selectedEdge.z].roomShapeFlag |= (byte)(1 << wallToRemove[rnd]);
                nextRoomsToSpawn.Add(dirNodes[wallToRemove[rnd]]);

            }

            foreach (DungeonNode newRoom in nextRoomsToSpawn)
            {
                byte newRoomConnections = (byte)Random.Range(1, 16);

                int parentDirX = newRoom.parent.x - newRoom.x;
                int parentDirY = newRoom.parent.z - newRoom.z;

                //if new room has a door that hits a wall or bounds make it a wall
                

                //ensure there is a door conecting back to parent;
                if (parentDirY == 1)
                    newRoomConnections |= 1 << 0;
                else if (parentDirX == 1)
                    newRoomConnections |= 1 << 1;
                else if (parentDirY == -1)
                    newRoomConnections |= 1 << 2;
                else if (parentDirX == -1)
                    newRoomConnections |= 1 << 3;

                grid[newRoom.x, newRoom.z].roomShapeFlag = newRoomConnections;
                spawnedRooms.Add(grid[newRoom.x, newRoom.z]);

                roomsToCreate.AddRange(GetEmptyConnection(newRoom, roomsToCreate));
                currentSize++;
                
                if (currentSize >= maxSize)
                    break;
            }
        }

        CloseUnusedDoors();

    }

    DungeonNode[] GetEdgeRooms()
    {
        List<DungeonNode> rtnList = new List<DungeonNode>();

        for(int i = 0; i < GridX; i ++)
        {
            for(int j = 0; j < GridZ; j++)
            {
                DungeonNode node = new DungeonNode(i, j);

                if (grid[i, j].roomShapeFlag > 0)
                {

                    DungeonNode[] dirNodes = {  new DungeonNode(node.x, node.z + 1),
                                            new DungeonNode(node.x + 1, node.z),
                                            new DungeonNode(node.x, node.z - 1),
                                            new DungeonNode(node.x - 1, node.z)};


                    for (int x = 0; x < dirNodes.Length; x++)
                    {
                        if (InGridBounds(dirNodes[x]) && grid[dirNodes[x].x, dirNodes[x].z].roomShapeFlag == 0)
                        {
                            rtnList.Add(node);
                            break;
                        }
                    }
                }
            }
        }

        return rtnList.ToArray();
    }

    void CloseUnusedDoors()
    {
        for(int i = 0; i < GridX; i++)
        {
            for(int j = 0; j < GridZ; j++)
            {
                DungeonNode node = grid[i, j];

                DungeonNode north = new DungeonNode(node.x, node.z + 1);
                DungeonNode east = new DungeonNode(node.x + 1, node.z);
                DungeonNode south = new DungeonNode(node.x, node.z - 1);
                DungeonNode west = new DungeonNode(node.x - 1, node.z);

                if(!InGridBounds(north) || (node.HasDoor(Direction.North) && !grid[north.x, north.z].HasDoor(Direction.South)))
                    grid[i,j].roomShapeFlag = (byte)(grid[i, j].roomShapeFlag & ~(1 << 0));

                if (!InGridBounds(east) || (node.HasDoor(Direction.East) && !grid[east.x, east.z].HasDoor(Direction.West)))
                    grid[i, j].roomShapeFlag = (byte)(grid[i, j].roomShapeFlag & ~(1 << 1));

                if (!InGridBounds(south) || (node.HasDoor(Direction.South) && !grid[south.x, south.z].HasDoor(Direction.North)))
                    grid[i, j].roomShapeFlag = (byte)(grid[i, j].roomShapeFlag & ~(1 << 2));

                if (!InGridBounds(west) || (node.HasDoor(Direction.West) && !grid[west.x, west.z].HasDoor(Direction.East)))
                    grid[i, j].roomShapeFlag = (byte)(grid[i, j].roomShapeFlag & ~(1 << 3));

            }
        }
    }



    List<DungeonNode> ExtendPath(List<DungeonNode> currentPath, int pathStep, int maxPathLegnth)
    {
        
        DungeonNode currentNode = currentPath[currentPath.Count - 1];

        if (pathStep < maxPathLegnth)
        {
            List<DungeonNode> validNewNodes = new List<DungeonNode>();

            DungeonNode east = new DungeonNode(currentNode.x + 1, currentNode.z);
            DungeonNode west = new DungeonNode(currentNode.x - 1, currentNode.z);
            DungeonNode north = new DungeonNode(currentNode.x, currentNode.z + 1);
            DungeonNode south = new DungeonNode(currentNode.x, currentNode.z - 1);


            if (IsNodeIsValid(currentPath, east) && FloodFillArea(east, currentPath, maxPathLegnth - pathStep) >= maxPathLegnth - pathStep)
            {
                validNewNodes.Add(east);
            }
            if(IsNodeIsValid(currentPath, west) && FloodFillArea(west, currentPath, maxPathLegnth - pathStep) >= maxPathLegnth - pathStep)
            {
                validNewNodes.Add(west);
            }
            if(IsNodeIsValid(currentPath, north) && FloodFillArea(north, currentPath, maxPathLegnth - pathStep) >= maxPathLegnth - pathStep)
            {
                validNewNodes.Add(north);
            }
            if(IsNodeIsValid(currentPath, south) && FloodFillArea(south, currentPath, maxPathLegnth - pathStep) >= maxPathLegnth - pathStep)
            {
                validNewNodes.Add(south);
            }

            if (validNewNodes.Count > 0)
            {

                DungeonNode rndnode = validNewNodes[Random.Range(0, validNewNodes.Count)];

                currentPath.Add(rndnode);
                pathStep++;

                ExtendPath(currentPath, pathStep, maxPathLegnth);
            }
            else
            {
                // The path has finished early for some reason break out of path generation and draw what we have
                Debug.LogError("Path was not able to reaach length " + maxPathLegnth + ", created with length " + pathStep);
                return currentPath;
            }
        }

        return currentPath;
    }

    public void AddPathToGrid(List<DungeonNode> _path)
    {
        for(int i = 0; i < _path.Count; i++)
        {
            if (i == 0)
            {
                grid[_path[i].x, _path[i].z].roomShapeFlag = GetConnectionFlag(_path[i], new DungeonNode[] { _path[i + 1]});
            }
            else if(i == _path.Count - 1)
            {
                grid[_path[i].x, _path[i].z].roomShapeFlag = GetConnectionFlag(_path[i], new DungeonNode[] { _path[i - 1] });
            }
            else
            {
                grid[_path[i].x, _path[i].z].roomShapeFlag = GetConnectionFlag(_path[i], new DungeonNode[] { _path[i + 1], _path[i - 1] });
            }
        }

        spawnedRooms.AddRange(_path);
    }

    byte GetConnectionFlag(DungeonNode node, DungeonNode[] connectedNodes)
    {
        int connecitonFlag = 0;

        DungeonNode east = new DungeonNode(node.x + 1, node.z);
        DungeonNode west = new DungeonNode(node.x - 1, node.z);
        DungeonNode north = new DungeonNode(node.x, node.z + 1);
        DungeonNode south = new DungeonNode(node.x, node.z - 1);

        for(int i = 0; i < connectedNodes.Length; i++)
        {
            if (connectedNodes[i] == north)
                connecitonFlag |= 1 << 0;

            if (connectedNodes[i] == east)
                connecitonFlag |= 1 << 1;

            if (connectedNodes[i] == south)
                connecitonFlag |= 1 << 2;

            if (connectedNodes[i] == west)
                connecitonFlag |= 1 << 3;
        }

        return (byte)connecitonFlag;

    }

    int FloodFillArea(DungeonNode fillNode, List<DungeonNode> path, int fillLimit)
    {
        List<DungeonNode> usedNodes = new List<DungeonNode>();
        List<DungeonNode> nodesToCheck = new List<DungeonNode>();
        int fillCount = 0;

        nodesToCheck.Add(fillNode);

        while (nodesToCheck.Count > 0 && fillCount < fillLimit)
        {
            List<DungeonNode> nodesToAdd = new List<DungeonNode>();

            foreach (DungeonNode node in nodesToCheck)
            {
                usedNodes.Add(node);

                DungeonNode east = new DungeonNode(node.x + 1, node.z);
                DungeonNode west = new DungeonNode(node.x - 1, node.z);
                DungeonNode north = new DungeonNode(node.x, node.z + 1);
                DungeonNode south = new DungeonNode(node.x, node.z - 1);


                if (IsNodeIsValid(path, east) && !nodesToCheck.Contains(east) && !usedNodes.Contains(east) && !nodesToAdd.Contains(east))
                {
                    nodesToAdd.Add(east);
                }
                if (IsNodeIsValid(path, west) && !nodesToCheck.Contains(west) && !usedNodes.Contains(west) && !nodesToAdd.Contains(west))
                {
                    nodesToAdd.Add(west);
                }
                if (IsNodeIsValid(path, north) && !nodesToCheck.Contains(north) && !usedNodes.Contains(north) && !nodesToAdd.Contains(north))
                {
                    nodesToAdd.Add(north);
                }
                if (IsNodeIsValid(path, south) && !nodesToCheck.Contains(south) && !usedNodes.Contains(south) && !nodesToAdd.Contains(south))
                {
                    nodesToAdd.Add(south);
                }

            }

            nodesToCheck = nodesToAdd;

            fillCount = usedNodes.Count;

        }

        return usedNodes.Count;
    }

    void AssignBossRoomToPath(List<DungeonNode> path)
    {
        bossRoom = path[path.Count - 1];
    }

    void AssignBossRoomInDungeon(DungeonNode startRoom)
    {
        List<List<DungeonNode>> nodesByDist = new List<List<DungeonNode>>();

        nodesByDist.Add(new List<DungeonNode>());

        List<DungeonNode> nodesToCheck = new List<DungeonNode>() { startRoom };

        int roomDist = 0;

        while(nodesToCheck.Count > 0 && roomDist < 15)
        {
            List<DungeonNode> nodesToCheckNext = new List<DungeonNode>();

            foreach(DungeonNode currentNode in nodesToCheck)
            {
                bool foundUniqueRoom = true;
                for (int i = 0; i < nodesByDist.Count; i++)
                {
                    if (nodesByDist[i].Contains(currentNode))
                    {
                        foundUniqueRoom = false;
                        break;
                    }
                }

                if (foundUniqueRoom && (currentNode.roomShapeFlag == 1 || currentNode.roomShapeFlag == 2 || currentNode.roomShapeFlag == 4 || currentNode.roomShapeFlag == 8))
                    nodesByDist[roomDist].Add(currentNode);
                else if(!foundUniqueRoom)
                    continue;

                DungeonNode north = new DungeonNode(currentNode.x, currentNode.z + 1);
                DungeonNode east = new DungeonNode(currentNode.x + 1, currentNode.z);
                DungeonNode south = new DungeonNode(currentNode.x, currentNode.z - 1);
                DungeonNode west = new DungeonNode(currentNode.x - 1, currentNode.z);

                if (InGridBounds(north) && (grid[currentNode.x, currentNode.z].HasDoor(Direction.North) && grid[north.x, north.z].HasDoor(Direction.South)))
                    nodesToCheckNext.Add(grid[north.x, north.z]);

                if (InGridBounds(east) && (grid[currentNode.x, currentNode.z].HasDoor(Direction.East) && grid[east.x, east.z].HasDoor(Direction.West)))
                    nodesToCheckNext.Add(grid[east.x, east.z]);

                if (InGridBounds(south) && (grid[currentNode.x, currentNode.z].HasDoor(Direction.South) && grid[south.x, south.z].HasDoor(Direction.North)))
                    nodesToCheckNext.Add(grid[south.x, south.z]);

                if (InGridBounds(west) && (grid[currentNode.x, currentNode.z].HasDoor(Direction.West) && grid[west.x, west.z].HasDoor(Direction.East)))
                    nodesToCheckNext.Add(grid[west.x, west.z]);


            }

            roomDist++;

            if(nodesToCheckNext.Count > 0)
                nodesByDist.Add(new List<DungeonNode>());

            nodesToCheck.Clear();
            nodesToCheck.AddRange(nodesToCheckNext);
            nodesToCheckNext.Clear();

        }

        nodesByDist.RemoveAt(nodesByDist.Count - 1);

        if(roomDist > 2)
        {
            List<DungeonNode> possibleBossRooms = new List<DungeonNode>();

            for(int i = 2; i < nodesByDist.Count; i++)
            {
                for(int j = 0; j < nodesByDist[i].Count; j++)
                {
                    possibleBossRooms.Add(nodesByDist[i][j]);
                }
            }

            if(possibleBossRooms.Count > 0)
                bossRoom = possibleBossRooms[Random.Range(0,possibleBossRooms.Count)];
        }

        if (bossRoom == null)
        {
            nodesByDist.RemoveAt(nodesByDist.Count - 1);

            List<DungeonNode> possibleBossRooms = new List<DungeonNode>();

            for (int i = 0; i < nodesByDist.Count; i++)
            {
                for (int j = 0; j < nodesByDist[i].Count; j++)
                {
                    possibleBossRooms.Add(nodesByDist[i][j]);
                }

                if (possibleBossRooms.Count > 0)
                    bossRoom = possibleBossRooms[Random.Range(0, possibleBossRooms.Count)];

                possibleBossRooms.Clear();
            }
        }
    }

    bool IsNodeIsValid(List<DungeonNode> currentPath, DungeonNode node)
    {
        return InGridBounds(node) && !currentPath.Contains(node);
    }

    bool InGridBounds(DungeonNode node)
    {
        return InGridBounds(node.x, node.z);
    }

    bool InGridBounds(int x, int z)
    {
        if (x >= 0 && x < GridX && z >= 0 && z < GridZ)
            return true;
        else
            return false;
    }
}
