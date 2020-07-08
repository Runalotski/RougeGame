using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//https://www.youtube.com/watch?v=bvRKfLPqQ0Q&list=WL&index=10&t=2016s

public class AbilityCoolDown : MonoBehaviour
{
    public string abilityButtonAxisName = "Fire1";
    public Image darkMask;
    public Text coolDownTextDisplay;

    [SerializeField] private Ability ability;
    [SerializeField] private GameObject weaponHolder; //Gun holds the raycast

    private Image myButtonImage;
    private AudioSource abilitySource;
    private float coolDownDuration;
    private float nextReadyTime;
    private float coolDownTimeLeft;


    // Start is called before the first frame update
    void Start()
    {
        Initialise(ability, weaponHolder);
    }

    public void Initialise(Ability selectedAbility, GameObject weaponHolder)
    {
        ability = selectedAbility;
        myButtonImage = GetComponent<Image>();
        abilitySource = GetComponent<AudioSource>();

        myButtonImage.sprite = ability.aSprite;
        darkMask.sprite = ability.aSprite;
        coolDownDuration = ability.aBaseCoolDown;
        ability.Initialise();
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

        ability.TriggerAbility(weaponHolder);
    }
}
