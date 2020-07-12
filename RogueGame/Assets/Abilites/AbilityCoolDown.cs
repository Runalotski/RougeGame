using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityCoolDown : MonoBehaviour
{
    public string abilityButtonAxisName;
    public Image darkMask;
    public Text coolDownTextDisplay;

    public Ability ability;

    private Image myButtonImage;
    private AudioSource abilitySource;
    private float coolDownDuration;
    private float nextReadyTime;
    private float coolDownTimeLeft;
    private float activeTime;


    // Start is called before the first frame update
    void Start()
    {
        Initialise(ability);
    }

    public void Initialise(Ability selectedAbility)
    {
        ability = selectedAbility;
        myButtonImage = transform.GetComponent<Image>();
        abilitySource = GetComponent<AudioSource>();

        //myButtonImage.sprite = ability.aSprite;
        //darkMask.sprite = ability.aSprite;
        coolDownDuration = ability.aBaseCoolDown;
        activeTime = ability.aBaseActiveTime;
        ability.Initialise(GameObject.FindGameObjectWithTag("Player").GetComponent<Actor>());
        AbilityReady();
    }

    // Update is called once per frame
    void Update()
    {
        bool coolDownComplete = (Time.time > nextReadyTime);

        if(coolDownComplete)
        {
            AbilityReady();

            if(Input.GetButtonDown(abilityButtonAxisName))
            {
                ButtonTriggerd();
            }
        }
        else if(DungeonManager.dungeonData.grid[(int)ability.owner.room.x, (int)ability.owner.room.z].enemiesCleard)
        {
            nextReadyTime += Time.deltaTime;
        }
        else
        {
            CoolDown();
        }
    }

    void AbilityReady()
    {
        coolDownTextDisplay.enabled = false;
        darkMask.enabled = false;
        ability.isActive = false;
        myButtonImage.color = new Color(1, 1, 1);
    }

    void CoolDown()
    {
        coolDownTimeLeft -= Time.deltaTime;
        float roundedCD = Mathf.Round(coolDownTimeLeft);
        coolDownTextDisplay.text = roundedCD.ToString();

        darkMask.fillAmount = (coolDownTimeLeft / coolDownDuration);

        //ability time has rune out
        if ((coolDownDuration - coolDownTimeLeft) >= activeTime)
        {
            ability.isActive = false;
            myButtonImage.color = new Color(1, 1, 1);
        }
    }

    void ButtonTriggerd()
    {
        nextReadyTime = coolDownDuration + Time.time;
        coolDownTimeLeft = coolDownDuration;

        darkMask.enabled = true;
        coolDownTextDisplay.enabled = true;

        ability.TriggerAbility();

        myButtonImage.color = new Color(0, 1, 0);
    }
}
