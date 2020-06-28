using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossActor : MonsterActor
{
    /// <summary>
    /// The Stages of the boss these will be the prefabs of the boss.
    /// </summary>
    [SerializeField]
    public BossStages bossStages;

    public bool stageCompleted = false;

    // Start is called before the first frame update
    void Awake()
    {
        init();
    }

    private void Start()
    {
        SpawnCurrentBossStage(transform.position);
    }

    protected override void init()
    {
        maxHealth = bossStages.GetTotalMaxHealth();
        base.init();
    }

    public BossStages.Stage ActiveStage()
    {
        return bossStages.GetCurrentStage();
    }

    public abstract void SpawnCurrentBossStage(Vector3 spawnPosition);

    public void DestroyEntity(GameObject g)
    {
        Destroy(g);
        if (bossStages.IsCurrentStageComplete())
            StageCompleted();
    }

    public void StageCompleted()
    {
        //Kill the active enetities for this stage
        bossStages.KillActiveEntitites(bossStages.GetCurrentStage());
        bossStages.currentStageCount++;

        if (bossStages.AreallStagescompleted())
            Die();
        else
            SpawnCurrentBossStage(transform.position);
    }

    public override void Die()
    {
        foreach(BossStages.Stage s in bossStages.stages)
        {
            foreach(Transform t in s.activeEntities)
            {
                if (t != null && t.GetComponent<MonsterActor>().health > 0)
                    t.GetComponent<MonsterActor>().Die();
            }
        }

        base.Die();
    }

    protected void CreateBossEnetity(Transform prefab, Vector3 spawnPosition)
    {
        Transform stageEntity = Instantiate(prefab, spawnPosition, Quaternion.identity);
        stageEntity.GetComponent<IBossStageEntity>().ParentBoss = this.GetComponent<BossActor>();
        bossStages.AddActiveEntityToCurrentStage(stageEntity);
    }
}
