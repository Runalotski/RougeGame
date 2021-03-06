using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/MegaBlast")]
public class MegaBlast : Ability
{
    Camera cam;

    public float aBaseDamage;
    public DamageTypes aDamageType;

    DamageClass aDamage;
    
    public override void Initialise(Actor actor)
    {
        actor.equippedAbilties.Add(this);
        owner = actor;

        cam = Camera.main;

        aDamage = new DamageClass(aBaseDamage, aDamageType);
    }

    public override void TriggerAbility()
    {
        DungeonNode dNode = DungeonManager.dungeonData.grid[(int)owner.room.x, (int)owner.room.z];

        Debug.Log("Cast in " + dNode.ToString());

        if(cam == null)
            cam = Camera.main;

        Plane[] frus = GeometryUtility.CalculateFrustumPlanes(cam);

        Debug.Log(dNode.enemies.Count + " enemies in the room");

        for(int i = dNode.enemies.Count - 1; i >= 0; i--)
        {
            Transform enemy = dNode.enemies[i];

            if (enemy != null)
            {
                Debug.Log("Eneny found for mega blast");
                Transform enemyBound = enemy.GetComponent<MonsterActor>().boundsCollider;

                if (enemyBound != null && GeometryUtility.TestPlanesAABB(frus, enemy.GetComponent<MonsterActor>().boundsCollider.GetComponent<Collider>().bounds))
                {
                    Debug.Log(enemy.name + " took " + aDamage + " from mega blast");
                    enemy.GetComponent<Actor>().TakeDamage(aDamage);
                }
            }
        }
    }
}
