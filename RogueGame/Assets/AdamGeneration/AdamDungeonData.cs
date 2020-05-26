using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityStandardAssets._2D;

public class AdamDungeonData
{

    public const int GridX = 50;
    public const int GridZ = 50;

    public int[,] grid = new int[GridX, GridZ];

    public DungeonNode SpawnPoint = new DungeonNode(0,0);

    public List<DungeonNode> path;

    public AdamDungeonData(int length)
    {
        InitGrid();
        SpawnPoint = new DungeonNode(GridX / 2, GridX / 2);
        CreatePath(length, SpawnPoint.x, SpawnPoint.z);
    }

    void InitGrid()
    {
        for (int i = 0; i < GridX; i++)
        {
            for (int j = 0; j < GridZ; j++)
            {
                grid[i, j] = 0;
            }
        }

        
    }

    /// <summary>
    /// Create a path of a set length and start position.
    /// </summary>
    /// <param name="length"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    void CreatePath(int length, int x, int y)
    {
        path = new List<DungeonNode>();
        path.Add(new DungeonNode(x,y));

        int pathStep = 0;

        AddToPath(path, pathStep, length);
        AddPathToGrid(path);

        Debug.Log("Path Added to Grid");
    }

    void AddToPath(List<DungeonNode> currentPath, int pathStep, int maxPathLegnth)
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

                AddToPath(currentPath, pathStep, maxPathLegnth);
            }
            else
            {
                // The path has finished early for some reason break out of path generation and draw what we have
                Debug.LogError("Path was not able to reaach length " + maxPathLegnth + ", created with length " + pathStep);
                return;
            }
        }    
    }

    public void AddPathToGrid(List<DungeonNode> _path)
    {
        for(int i = 0; i < _path.Count; i++)
        {
            if (i == 0)
            {
                grid[_path[i].x, _path[i].z] = GetConnectionFlag(path[i], new DungeonNode[] {path[i + 1]});
            }
            else if(i == _path.Count - 1)
            {
                grid[_path[i].x, _path[i].z] = GetConnectionFlag(path[i], new DungeonNode[] { path[i - 1] });
            }
            else
            {
                grid[_path[i].x, _path[i].z] = GetConnectionFlag(path[i], new DungeonNode[] { path[i + 1], path[i - 1] });
            }
        }
    }

    int GetConnectionFlag(DungeonNode node, DungeonNode[] connectedNodes)
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

        return connecitonFlag;

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
        if (x > 0 && x < GridX && z > 0 && z < GridZ)
            return true;
        else
            return false;
    }

    public bool HasPath()
    {
        return (path != null && path.Count > 0);
    }
}
