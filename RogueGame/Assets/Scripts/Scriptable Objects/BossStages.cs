using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Boss Stages", menuName = "Boss Stages")]
public class BossStages : ScriptableObject
{
    [System.Serializable]
    public class Stage
    {
        public bool sharedHealth;
        public List<Transform> bossEntitiesPrefabs;

        [System.NonSerialized]
        public float damageTaken;

        [System.NonSerialized]
        public List<Transform> activeEntities = new List<Transform>();

        [System.NonSerialized]
        public List<Vector3> deathLocations = new List<Vector3>();
    }

    public List<Stage> stages = new List<Stage>();

    [System.NonSerialized]
    public int currentStageCount = 0;

    public float GetTotalMaxHealth()
    {
        float rtn = 0;

        for(int i = 0; i < stages.Count; i++)
        {
            rtn += GetStageHealth(i);
        }

        return rtn;
    }

    public float GetStageHealth(int stageCount)
    {
        float rtn = 0;

        foreach (Transform t in stages[stageCount].bossEntitiesPrefabs)
        {
            rtn += t.GetComponent<MonsterActor>().maxHealth;
        }

        return rtn;
    }

    public Stage GetCurrentStage()
    {
        return stages[currentStageCount];
    }

    /// <summary>
    /// Tally up the amount of damage the stage has taken. Only affects shared health stages
    /// </summary>
    /// <param name="stageIndex">The stage to take damage</param>
    /// <param name="damage">The amount of damage taken</param>
    public void ApplyDamageToCurrentStage(float damage, BossActor parentBoss)
    {
        GetCurrentStage().damageTaken += damage;

        //Check if the shared damage is enugh to complte the stage
        if(IsCurrentStageComplete())
        {
            parentBoss.StageCompleted();
        }
    }

    /// <summary>
    /// Check if the current stage is completed
    /// </summary>
    /// <returns></returns>
    public bool IsCurrentStageComplete()
    {
        if (currentStageCount < stages.Count)
        {
            Stage currentStage = GetCurrentStage();

            bool stageComplete = false;

            //If the Stage has taken its shared damage clear the stage
            if (currentStage.sharedHealth && currentStage.damageTaken >= GetStageHealth(currentStageCount))
            {
                Debug.Log("SharedHealth is completed");
                stageComplete = true;
            }
            //Stage health is not link wait for each entity to die
            else
            {
                //I dont like this.... Default will need to assume that the stage is complete which is the tested as any active entitiy 
                // not equal to null would mean th estage is not complete.
                stageComplete = true;

                //Check if all the active entities have been destroyed.
                for (int i = 0; i < currentStage.activeEntities.Count; i++)
                {
                    if (currentStage.activeEntities[i] != null && currentStage.activeEntities[i].GetComponent<MonsterActor>().health > 0)
                    {
                        stageComplete = false;
                        break;
                    }
                }
            }

            return stageComplete;
        }

        //all stages are completed so this one must be
        return true;
    }

    /// <summary>
    /// Kill all the active enetites for this stage
    /// </summary>
    /// <param name="s">THe stage to kill active entities</param>
    public void KillActiveEntitites(Stage s)
    {
        foreach (Transform t in s.activeEntities)
        {
            if (t != null)
                Destroy(t.gameObject);
        }
    }

    public void AddActiveEntityToCurrentStage(Transform t)
    {
        stages[currentStageCount].activeEntities.Add(t);
    }

    public bool AreallStagescompleted()
    {
        return currentStageCount >= stages.Count;
    }
}
