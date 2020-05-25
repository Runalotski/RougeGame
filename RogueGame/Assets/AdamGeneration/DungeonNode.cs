using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonNode
{
    public List<DungeonNode> childNodes;

    public int x;
    public int z;

    public DungeonNode(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public DungeonNode(Vector2 pos)
    {
        this.x = (int)pos.x;
        this.z = (int)pos.y;
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
    public override int GetHashCode()
    {
        return this.x ^ this.z;
    }public override string ToString()
    {
        return "DungeionNode(" + this.x + "," + this.z + ")";
    }
}
