using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Damage Resistences", menuName = "Damage Resistences")]
public class DamageResistances : ScriptableObject
{
    [System.Serializable]
    public struct Resistance
    {
        public DamageTypes damageType;
        public float percentageToTake;
    }

    public List<Resistance> resistances = new List<Resistance>();

    public float CalculateDamagetoTake(float damage, DamageTypes damageType)
    {
        foreach(Resistance r in resistances)
        {
            if(r.damageType == damageType)
            {
                return ((damage * r.percentageToTake));
            }
        }

        return 0;
    }
}
