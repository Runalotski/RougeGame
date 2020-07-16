using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    public int Level { get; private set; }

    public ulong Xp { get; private set; }

    public ulong NextLevelXP
    {
        get
        { 
            return (ulong)((Level + 1) * (Level + 1)) * 100;
        }
    }

    public void AddXP(int xp)
    {
        Xp += (ulong)xp;

        while (Xp >= NextLevelXP)
        {
            Level++;
        }
    }
}
