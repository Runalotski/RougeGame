using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBoss : BossActor
{
    public override void SpawnCurrentBossStage(Vector3 spawnPosition)
    {
        switch (bossStages.currentStageCount)
        {
            case 0:
                CreateBossEnetity(bossStages.stages[0].bossEntitiesPrefabs[0], spawnPosition + new Vector3(0,0.5f,0));
                break;
            case 1:
                CreateBossEnetity(bossStages.stages[1].bossEntitiesPrefabs[0], spawnPosition);
                CreateBossEnetity(bossStages.stages[1].bossEntitiesPrefabs[1], spawnPosition + new Vector3(2f, 0, -2f));
                break;
        }
    }
}
