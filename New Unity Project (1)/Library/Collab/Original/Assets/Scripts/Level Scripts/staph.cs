using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

namespace Bacteria
{

    public class staph : MonoBehaviour
    {
        public float speed = 10.0f;

        HealthBarManager HBMscript;
        GameObject StaphInfection;
        public StaphSpawner SSScript;

        Slider slider;
        Rigidbody rb;
        System.Random rnd = new System.Random();

        int randomDie; //number will be randmozied. If number is 0, then monocyte will kill itself.
        int dieUpperRange = 2; //randomized from 0 to upperrange. (uninclusive)

        public float objectWidth;
        public float objectHeight;

        Canvas canvas;
        float w, h, x, y, xOrigin, yOrigin;

        float maxVelX, maxVelY, maxVelMag;

        bool isTracked = false;
        bool antibodyAttatched = false;
        bool monocyteTracked = false;

        void Start()
        {
            canvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
            slider = GameObject.FindGameObjectWithTag("Slider").GetComponent<Slider>();
            HBMscript = slider.GetComponent<HealthBarManager>();

            StaphInfection = GameObject.Find("StaphInfection");
            SSScript = StaphInfection.GetComponent<StaphSpawner>();

            w = canvas.GetComponent<RectTransform>().rect.width;
            h = canvas.GetComponent<RectTransform>().rect.height;
            x = canvas.GetComponent<RectTransform>().rect.x * -1;
            y = canvas.GetComponent<RectTransform>().rect.y * -1;

            xOrigin = x - w / 2;
            yOrigin = y - h / 2;

            float angle = Random.Range(0f, 90f);
            float radAngle = angle * Mathf.Deg2Rad;
            maxVelX  =Mathf.Cos(radAngle) * 60;
            maxVelY = Mathf.Sin(radAngle) * 60;

            rb = GetComponent<Rigidbody>();
            // rb.velocity = new Vector2((PlusMinus(true)*rnd.Next(30,150)), (PlusMinus(false) * rnd.Next(30,150)));
            rb.velocity = new Vector2((PlusMinus() * maxVelX), (PlusMinus() * maxVelY));

            objectWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x / 2;
            objectHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2;

            print("WIDTH: " + w + " HEIGHT: " + h + " xPos: " + x + " yPos: " + y + " xVel: " + rb.velocity.x + " yVel: " + rb.velocity.y);
          
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            GetComponent<CapsuleCollider>().enabled = true;
            rb.freezeRotation = true;    

            if ((transform.position.x < xOrigin))
            { //hit the left border
                transform.position = new Vector2(xOrigin, transform.position.y);
                rb.velocity = new Vector2(-1 * rb.velocity.x, rb.velocity.y);
                HBMscript.takeDamage();

            }

            else if ((transform.position.x > xOrigin + w))
            { //hit the right border
                transform.position = new Vector2(xOrigin + w, transform.position.y);
                rb.velocity = new Vector2(-1 * rb.velocity.x, rb.velocity.y);
                HBMscript.takeDamage();

            }

            if (transform.position.y < yOrigin + (h * .12))
            { //hit the top border
                transform.position = new Vector2(transform.position.x, (float)(yOrigin + (h * .12)));
                rb.velocity = new Vector2(rb.velocity.x, -1 * rb.velocity.y);
                HBMscript.takeDamage();
            }

            else if (transform.position.y > yOrigin + (h * .9))
            { //hit the bottom border
                transform.position = new Vector2(transform.position.x, (float)(yOrigin + (h * .9)));
                rb.velocity = new Vector2(rb.velocity.x, -1 * rb.velocity.y);
                HBMscript.takeDamage();
            }


        }

        void OnCollisionEnter(Collision collision)
        {

            if (collision.gameObject.CompareTag("Enemy"))
            {
                Rigidbody otherRB = collision.gameObject.GetComponent<Rigidbody>();
                //rb.velocity = new Vector2(otherRB.velocity.x * -1, otherRB.velocity.y * -1);
            }

            randomDie = rnd.Next(0, dieUpperRange);
            if(randomDie == 0) {
                if (collision.gameObject.CompareTag("Monocyte")){
                    collision.gameObject.GetComponent<monocyte>().setHasTarget(false);
                    monocyteTracked = false;
                    SSScript.removeEnemy(this.gameObject);
                    Destroy(this.gameObject);
                }
                else if (collision.gameObject.CompareTag("Neutrophil") || collision.gameObject.CompareTag("Cytokine"))
                {
                    SSScript.removeEnemy(this.gameObject);
                    Destroy(this.gameObject);
                }   

            }

        }

        public int PlusMinus()
        {
            int Sign = rnd.Next(0, 2);
            if (Sign == 0) { return -1; }
            if (Sign == 1) { return 1; }
            else { return 100; }
        }

      
        public bool getIsTracked()
        {
            return isTracked;
        }

        public void setIsTracked(bool hi)
        {
            isTracked = hi;
        }
        
        public void attatchAntibody()
        {
           antibodyAttatched = true;
           rb.velocity = new Vector2(rb.velocity.x/3, rb.velocity.y/3);
           dieUpperRange = 1;
        }

        public bool  getAntibodyAttached()
        {
            return antibodyAttatched;
        }

        public bool getMonocyteTracked()
        {
            return monocyteTracked;
        }

        public void setMonocyteTracked(bool hi)
        {
            monocyteTracked = hi;
        }
    }//class
}  //namespace
