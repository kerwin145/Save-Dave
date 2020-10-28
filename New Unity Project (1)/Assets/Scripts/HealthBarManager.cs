using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Image fillImage;
    public Text Health;
    public GameObject deathUI;
    Slider slider;

    System.Random rnd = new System.Random();

    void Awake()
    {

        slider = GameObject.FindGameObjectWithTag("Slider").GetComponent<Slider>();
        slider.value = slider.maxValue;
        //slider.value = 25;
    }

    // Update is called once per frame
    void Update()
    {

        /* if (slider.value <= slider.minValue)
             fillImage.enabled = false;
         if (slider.value > slider.minValue && !(fillImage.enabled))
             fillImage.enabled = true;

         //fillImage color = color.white;*/
        try
        {
            Health.text = "Health: " + slider.value;
        }
        catch
        { }

        if(slider.value <= 0)
        {
            deathUI.SetActive(true);
        }

    }

    public void takeDamage(){
        int takeDamageChance = rnd.Next(0, 6); //Generated random number from 0 - 10.
        if (takeDamageChance == 0)
            slider.value-= 5;
           // print(slider.value);

    }
    public void takeCytokineStormDamage()
    {
            slider.value -= 400;
        // print(slider.value);

    }
    public void takeCytokineDamage()
    {
        int takeDamageChance = rnd.Next(0, 2); //Generated random number from 0 - 10.
        if (takeDamageChance == 0) {
            slider.value -= 1;
        }
        // print(slider.value);

    }
    public void resetSlider()
    {
        slider.value = slider.maxValue;
        //slider.value = 25;
    }

    public float getHealth()
    {
        return slider.value;
    }
}
