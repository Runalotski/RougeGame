using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleWeapons : MonoBehaviour
{
    Transform heldWeapon;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Fire1") && heldWeapon != null)
        {
            heldWeapon.GetComponent<IWeaponClass>().Attack();
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.root.GetComponent<MonoBehaviour>() is IWeaponClass)
        {
            if(heldWeapon == null)
            {
                PickupWeapon(hit.transform.root);
            }
        }
    }

    void PickupWeapon(Transform weaponT)
    {
        heldWeapon = weaponT;
        weaponT.parent = this.transform;
        weaponT.position = new Vector3(0, 2, 0);
        weaponT.rotation = Quaternion.identity;
        weaponT.GetComponent<IWeaponClass>().owner = this.transform;
    }
}
