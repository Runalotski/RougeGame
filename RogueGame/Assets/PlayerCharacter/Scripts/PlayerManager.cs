using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Transform player;

    float maxHealth = 100;

    public Transform StartWeapon;

    public Transform gameManager;

    public bool displayMenu = false;

    private void Start()
    {

        if (StartWeapon != null)
        {
            Transform weapon = Instantiate(StartWeapon);
            player.GetComponent<HandleWeapons>().PickupWeapon(weapon);
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Settings"))
            displayMenu = !displayMenu;
    }

    private void OnGUI()
    {
        if (player != null)
        {
            GUI.TextArea(new Rect(10, 10, 100, 20), DungeonManager.GetDungeonPosition(player.position).ToString());

            GUI.TextArea(new Rect(10, 40, 120, 20), "Health: " + System.Math.Round(player.GetComponent<PlayerActor>().health, 2));

            GUI.TextArea(new Rect(10, 70, 150, 20), "Kill The Orange Cube");
        }

        float endScreenX = 120;
        float endScreenY = 25;

        float endScreenPosX = (Screen.width / 2) - (endScreenX / 2);
        float endScreenPosY = (Screen.height / 2) - (endScreenY / 2);

        if (GameManager.gameOver)
        {
            string msg = GameManager.playerDead ? "Game Over :(" : "You Win :D";

            GUI.TextArea(new Rect(endScreenPosX, endScreenPosY, endScreenX, endScreenY), msg + " " + System.Math.Round(GameManager.respawnCounter,1));
        }

        if(displayMenu)
        {
            if (GUI.Button(new Rect(130, 10, 150, 20), "Quit"))
            {
                Application.Quit();
            }
        }
    }
}
