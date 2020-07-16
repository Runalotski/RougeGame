using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterAbilities : MonoBehaviour
{
    public Transform AbilityIconRegion;
    public Transform AbilityIconPrefab;

    public List<Ability> abilities;

    // Start is called before the first frame update
    void Start()
    {
        int i = 1;
        foreach(Ability a in abilities)
        {
            Transform iconClone = Instantiate(AbilityIconPrefab, AbilityIconRegion);
            iconClone.GetComponent<Image>().sprite = a.aSprite;

            iconClone.Find("DarkMask").GetComponent<Image>().sprite = a.aSprite;

            AbilityCoolDown coolDownS = iconClone.GetComponent<AbilityCoolDown>();

            coolDownS.ability = a;
            coolDownS.abilityButtonAxisName = "Ability" + i;

            i++;
        }
    }
}
