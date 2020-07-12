using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponHandler : MonoBehaviour, IWeaponUser
{
    public IWeapon ActiveWeapon { get; set; }
    public List<Transform> CarriedWeapons { get; set; }

    public int MaxCarriedWeapons { get; set; }

    private void Awake()
    {
        WeaponUserInit();
    }

    // Update is called once per frame
    void Update()
    {
        if(AttackedShouldTrigger())
            Attack("Enemy");
    }

    // If we touch a weapn on the ground pick it up if we can
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.root.GetComponent<MonoBehaviour>() is IWeapon)
        {
            if(ActiveWeapon == null)
            {
                PickUpWeapon(hit.transform.root);
            }
        }
    }

    public bool AttackedShouldTrigger()
    {
        return Input.GetButton("Fire1") && ActiveWeapon != null && !ActiveWeapon.WeaponWaitingForCooldown;
    }

    public void Attack(string targetTag)
    {
        //Set up for wherewhich direction we want to shoot in
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Vector3 targetPosition = Vector3.zero;

        //Mouse has hit a valid location to shoot at
        //TODO: We dont need a ray cast for this, get mouse position when we click!!
        if (Physics.Raycast(mouseRay, out hit, 200))
        {
            targetPosition = hit.point;
        }

        ActiveWeapon.WeaponAttack(transform.position, targetPosition, targetTag);
    }

    public void SwitchWeapon()
    {
        throw new System.NotImplementedException();
    }

    public void DropWeapon()
    {
        throw new System.NotImplementedException();
    }

    public void PickUpWeapon(Transform weaponT)
    {
        if (ActiveWeapon == null)
        {
            CarriedWeapons.Add(weaponT);

            ActiveWeapon = weaponT.root.GetComponent<IWeapon>();
            weaponT.parent = this.transform;
            weaponT.position = transform.position + new Vector3(0, 2, 0);
            weaponT.rotation = Quaternion.identity;
            weaponT.GetComponent<IWeapon>().WeaponOwner = this.transform;
        }
        else
        {
            if(CarriedWeapons.Count < MaxCarriedWeapons)
                CarriedWeapons.Add(weaponT);
        }
    }

    public void WeaponUserInit()
    {
        MaxCarriedWeapons = 4;
        CarriedWeapons = new List<Transform>();
    }
}
