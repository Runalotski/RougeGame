﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityShopScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            Debug.Log("Ability Shop");
    }
}
