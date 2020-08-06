using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XPBarScript : MonoBehaviour
{
    public Text healthText;

    public Slider healthSlider;

    public PlayerActor player;

    // Update is called once per frame
    void Update()
    {
        healthText.text = player.Level.ToString();

        if (player.CurrentXP > 0)
            healthSlider.value = (float)player.CurrentXP / (float)player.NextLevelXP();
        else
            healthSlider.value = 0;
    }
}
