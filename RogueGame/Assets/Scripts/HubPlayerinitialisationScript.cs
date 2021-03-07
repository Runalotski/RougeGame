using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubPlayerinitialisationScript : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform startWeapon;
    public Transform abilitiesIconRegion;
    public Transform healthBar;
    public Transform xpBar;
    public Transform mcamera;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            player.transform.GetComponent<PlayerActor>().StartWeapon = startWeapon;
            player.transform.GetComponent<CharacterAbilities>().AbilityIconRegion = abilitiesIconRegion;
            DontDestroyOnLoad(player);

            healthBar.GetComponent<HealthBarScript>().player = player.GetComponent<Actor>();
            xpBar.GetComponent<XPBarScript>().player = player.GetComponent<PlayerActor>();

            mcamera.GetComponent<FollowTarget>().target = player.transform;
        }
        else
        {
            //
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = Vector3.zero;
            player.GetComponent<CharacterController>().enabled = true;
        }
    }
}
