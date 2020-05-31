using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Transform player;

    private void OnGUI()
    {
        GUI.TextArea(new Rect(10, 10, 100, 20), AdamDungeonManager.GetDungeonPosition(player.position).ToString());
    }
}
