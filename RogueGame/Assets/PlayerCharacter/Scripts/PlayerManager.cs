using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Transform player;

    float maxHealth = 100;

    public Transform StartWeapon;

    private void Start()
    {

        if (StartWeapon != null)
        {
            Transform weapon = Instantiate(StartWeapon);
            player.GetComponent<HandleWeapons>().PickupWeapon(weapon);
        }
    }

    private void OnGUI()
    {
        GUI.TextArea(new Rect(10, 10, 100, 20), AdamDungeonManager.GetDungeonPosition(player.position).ToString());

        GUI.TextArea(new Rect(10, 40, 120, 30), "Health: " + System.Math.Round(player.GetComponent<PlayerActor>().health, 2));
    }
}
