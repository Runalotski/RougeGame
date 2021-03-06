﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// when enable the player can die and win the game
    /// </summary>
    public bool enableGameMode;

    [System.NonSerialized]
    public static bool gameOver = false;

    [System.NonSerialized]
    public static bool playerDead = false;

    [System.NonSerialized]
    public static bool bossDefeated = false;

    public static float respawnCounter
    {
        get
        {
            return Mathf.Max(respawnTime - (Time.timeSinceLevelLoad - deathTime),0);
        }
        private set { }
    }

    private static float deathTime = 0;

    private static float respawnTime = 3;

    public void Awake()
    {
        gameOver = false;
        playerDead = false;
        bossDefeated = false;
        
    }

    public void Start()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerActor>().gameManager = this;
    }

    public void PlayerDied()
    {
        if (enableGameMode && !playerDead && !bossDefeated)
        {
            deathTime = Time.timeSinceLevelLoad;
            gameOver = true;
            playerDead = true;
            SceneManager.LoadScene("HubWorld", LoadSceneMode.Single);
        }
    }

    public void BossDefeated()
    {
        if (enableGameMode && !bossDefeated)
        {
            deathTime = Time.timeSinceLevelLoad;
            gameOver = true;
            bossDefeated = true;
            StartCoroutine(DisplayEndMessage());
        }
    }

    IEnumerator DisplayEndMessage()
    {
        //This is used to add a delay before spawning the player for displaying a message
        yield return new WaitForSeconds(respawnTime);
        SceneManager.LoadScene("HubWorld", LoadSceneMode.Single);
        yield return null;

    }
}
