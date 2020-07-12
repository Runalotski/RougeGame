using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangerScript : MonsterActor, IWeaponUser
{
    public Transform SpawnWeaponPrefab;

    public List<Transform> CarriedWeapons { get; set; }
    public int MaxCarriedWeapons { get; set; }
    public IWeapon ActiveWeapon { get; set; }

    enum TargetType { TargetLocation, PredictTarget}

    private TargetType TargetingStratergy;

    private Vector3 TargetPositionLastFrame;

    Vector3 tragetNextPosEst;

    private void Awake()
    {
        int rnd = Random.Range(0, 2);

        TargetingStratergy = rnd == 0 ? TargetType.PredictTarget : TargetType.TargetLocation;

        init();
    }

    // Start is called before the first frame update
    void Start()
    {
        target = dungeonManager.PlayerTransform();
        TargetPositionLastFrame = target.position;

        WeaponUserInit();

        Transform spawnWep = Instantiate(SpawnWeaponPrefab) as Transform;
        PickUpWeapon(spawnWep);
    }

    // Update is called once per frame
    void Update()
    {
        //If player target is in the same 2room attack it.
        if (AttackedShouldTrigger())
            Attack("Player");

        Vector3 targetMoveVec = target.position - TargetPositionLastFrame;

        float targetDist = (target.position - transform.position).magnitude;

        tragetNextPosEst = target.position + (targetMoveVec * (targetDist));

    }

    void LateUpdate()
    {
        TargetPositionLastFrame = target.position;
    }

    public bool AttackedShouldTrigger()
    {
        return agent.enabled && target != null && dungeonManager.PlayerDungeonPos() == room && !ActiveWeapon.WeaponWaitingForCooldown;
    }

    public void Attack(string targetTag)
    {
        agent.destination = dungeonManager.PlayerTransform().position;
        agent.isStopped = false;

        switch (TargetingStratergy)
        {
            case TargetType.TargetLocation:
                ActiveWeapon.WeaponAttack(transform.position, target.position, targetTag);
                break;

            case TargetType.PredictTarget:

                Vector3 targetMoveVec = target.position - TargetPositionLastFrame;

                float targetDist = (target.position - transform.position).magnitude;

                tragetNextPosEst = target.position + (targetMoveVec * (targetDist * 1.6f)) ;


                ActiveWeapon.WeaponAttack(transform.position, tragetNextPosEst, targetTag);
                break;
        }
    }

    void OnDrawGizmos()
    {

        Gizmos.color = new Color(1, 0, 0);
        Gizmos.DrawCube(tragetNextPosEst, new Vector3(0.3f, 0.3f, 0.3f));
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

            ActiveWeapon.WeaponOwner = this.transform;
            ActiveWeapon.WeaponTransform = weaponT;

            weaponT.parent = this.transform;
            weaponT.position = transform.position + new Vector3(0, 2, 0);
            weaponT.rotation = Quaternion.identity;
        }
    }

    public void WeaponUserInit()
    {
        MaxCarriedWeapons = 1;
        CarriedWeapons = new List<Transform>();
    }
}
