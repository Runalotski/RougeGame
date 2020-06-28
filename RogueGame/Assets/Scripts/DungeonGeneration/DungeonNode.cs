using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonNode
{
    /// <summary>
    /// Referece to connected Rooms
    /// </summary>
    public List<DungeonNode> connections;

    /// <summary>
    /// Flag for position of room 1 = North, 2 = East, 4 = South, 8 = West
    /// </summary>
    public byte roomShapeFlag;

    public Transform transform;

    /// <summary>
    /// Parent just the first connection betwen room establisehd == connectios[0]
    /// </summary>
    public DungeonNode parent {
        get { return connections[0]; }
        set { SetParent(value); }
    }

    /// <summary>
    /// True when all the enemies have been killed in a room
    /// </summary>
    public bool enemiesCleard = false;
    public bool enemiesAreActive = false;

    public List<Transform> enemies = new List<Transform>();

    public int x;
    public int z;

    public DungeonNode(int x, int z, DungeonNode parent, byte roomShapeFlag = 0)
    {
        this.connections = new List<DungeonNode>();
        this.connections.Add(parent);

        this.roomShapeFlag = roomShapeFlag;

        this.x = x;
        this.z = z;
    }

    public DungeonNode(int x, int z, byte roomShapeFlag = 0)
    {
        connections = new List<DungeonNode>();

        this.roomShapeFlag = roomShapeFlag;

        this.x = x;
        this.z = z;
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;

        DungeonNode other = obj as DungeonNode;

        // Would still want to check for null etc. first.
        return other != null &&
               this.x == other.x &&
               this.z == other.z;
    }

    public static bool operator ==(DungeonNode lhs, DungeonNode rhs)
    {
        // Check for null on left side.
        if (Object.ReferenceEquals(lhs, null))
        {
            if (Object.ReferenceEquals(rhs, null))
            {
                // null == null = true.
                return true;
            }

            // Only the left side is null.
            return false;
        }
        // Equals handles case of null on right side.
        return lhs.Equals(rhs);
    }

    public static bool operator !=(DungeonNode lhs, DungeonNode rhs)
    {
        return !(lhs == rhs);
    }

    public override int GetHashCode()
    {
        return this.x ^ this.z;
    }

    public override string ToString()
    {
        return "DungeionNode(" + this.x + "," + this.z + ")";
    }

    private void SetParent(DungeonNode node)
    {
        if (connections == null)
            connections = new List<DungeonNode>();

        if (connections.Count == 0)
            connections.Add(node);
        else
            connections[0] = node;
    }

    public bool HasDoor(DungeonData.Direction dir)
    {
        return (roomShapeFlag & (1 << (int)dir)) == (1 << (int)dir);
    }
}
