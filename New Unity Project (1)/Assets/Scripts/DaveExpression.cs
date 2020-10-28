using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace Bacteria
{
    public class DaveExpression : MonoBehaviour
    {
        HealthBarManager HBMscript;
        public Sprite image1;
        public Sprite image2;
        public Sprite image3;
        public Sprite image4;

        public Sprite currentImage;

        float health;
        float maxHealth = 1000;

        //SpriteRenderer SpriteRender;


        // Start is called before the first frame update
        void Start()
        {
            //SpriteRender = GameObject.GetComponent<SpriteRenderer>();

            HBMscript = GameObject.Find("HealthBar").GetComponent<HealthBarManager>();
            GameObject.Find("DaveFace").GetComponent<Image>().sprite = image1;
        }

        // Update is called once per frame
        void Update()
        {
            health = HBMscript.getHealth();
            if (health <= .75*maxHealth && health > .5*maxHealth)
            {
                GameObject.Find("DaveFace").GetComponent<Image>().sprite = image2;
            }

            if (health <= .5 * maxHealth && health > .25 * maxHealth)
            {
                GameObject.Find("DaveFace").GetComponent<Image>().sprite = image3;
            }

            if (health <= .25 * maxHealth && health > 0 * maxHealth)
            {
                GameObject.Find("DaveFace").GetComponent<Image>().sprite = image4;
            }

        }
    }
}
