using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShopScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            Debug.Log("Weapon Shop");
    }
}
