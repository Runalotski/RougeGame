using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//https://www.youtube.com/watch?v=bvRKfLPqQ0Q

public class AbilityCoolDown : MonoBehaviour
{
    public string abilityButtonAxisName = "Ability1";
    public Image darkMask;
    public Text coolDownTextDisplay;

    [SerializeField] private Ability ability;

    private Image myButtonImage;
    private AudioSource abilitySource;
    private float coolDownDuration;
    private float nextReadyTime;
    private float coolDownTimeLeft;


    // Start is called before the first frame update
    void Start()
    {
        Initialise(ability);
    }

    public void Initialise(Ability selectedAbility)
    {
        ability = selectedAbility;
        myButtonImage = GetComponent<Image>();
        abilitySource = GetComponent<AudioSource>();

        //myButtonImage.sprite = ability.aSprite;
        //darkMask.sprite = ability.aSprite;
        coolDownDuration = ability.aBaseCoolDown;
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
    }

    void CoolDown()
    {
        coolDownTimeLeft -= Time.deltaTime;
        float roundedCD = Mathf.Round(coolDownTimeLeft);
        coolDownTextDisplay.text = roundedCD.ToString();

        darkMask.fillAmount = (coolDownTimeLeft / coolDownDuration);
    }

    void ButtonTriggerd()
    {
        nextReadyTime = coolDownDuration + Time.time;
        coolDownTimeLeft = coolDownDuration;

        darkMask.enabled = true;
        coolDownTextDisplay.enabled = true;
        ability.isActive = true;

        ability.TriggerAbility();
    }
}
