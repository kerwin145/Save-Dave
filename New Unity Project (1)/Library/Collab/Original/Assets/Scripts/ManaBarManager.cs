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

    float regenTime = 0.65f; // seconds.
    float time = 0;
    
    Slider slider;

    System.Random rnd = new System.Random();

    void Awake()
    {
        slider = GameObject.FindGameObjectWithTag("ManaBarSlider").GetComponent<Slider>();

        slider.maxValue = maxMana;
        currentMana = maxMana / 4; 
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
    }


    }

