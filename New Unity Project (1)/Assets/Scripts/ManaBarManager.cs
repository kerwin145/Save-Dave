using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ManaBarManager : MonoBehaviour
{
    // Start is called before the first frame update
    float maxMana = 100;
    float currentMana;
    public Image fillImage;
    public Text Mana;

    float regenTime = 0.8f; // seconds.
    float time = 0;
    
    Slider slider;

    System.Random rnd = new System.Random();

    void Awake()
    {
        slider = GameObject.FindGameObjectWithTag("ManaBarSlider").GetComponent<Slider>();

        slider.maxValue = maxMana;

        Mana.text = "Mana: " + slider.value;

        currentMana = maxMana / 5;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (time >= regenTime){
            time = 0;
            if (currentMana < maxMana)
                currentMana++;
        }
        
        slider.value = currentMana;

        Mana.text = "Mana: " + slider.value;


        /* if (slider.value <= slider.minValue)
            fillImage.enabled = false;
        if (slider.value > slider.minValue && !(fillImage.enabled))
            fillImage.enabled = true;

        //fillImage color = color.white;*/

        //  print("Mana: " + slider.value);
    }


    public float getMana(){
        return currentMana;
    }

   
    public void useMana(float manaUsed){
        currentMana -= manaUsed;
        Mana.text = "Mana: " + currentMana;
    }
    public void resetSlider()
    {
        currentMana = maxMana / 5;
        slider.value = currentMana;
    }
}

