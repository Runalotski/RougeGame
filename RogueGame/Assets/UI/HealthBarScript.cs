using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    public Text healthText;

    public Slider healthSlider;

    public Actor player;

    // Update is called once per frame
    void Update()
    {
        healthText.text = player.health + " / " + player.maxHealth;

        healthSlider.value = player.health / player.maxHealth;
    }
}
