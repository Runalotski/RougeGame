using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBossStageEntity
{
    BossActor ParentBoss { get; set; }

    void ApplyDamageToParentBoss(float damage);

    void EntityDeath(GameObject g);
}
