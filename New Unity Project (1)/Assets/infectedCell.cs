using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Bacteria
{
    public class infectedCell : MonoBehaviour
    {
        HealthBarManager HBMscript;
        GameObject InfectedCellManager;
        VirusAndInfectedCellManager VICMScript;
        GameObject Virus;

        Rigidbody rb;
        System.Random rnd = new System.Random();

        public float objectWidth;
        public float objectHeight;

        Canvas canvas;
        float w, h, x, y, xOrigin, yOrigin;

        float maxVelX, maxVelY, maxVelMag;

        //tracking things...
        bool isTracked = false;
        bool antibodyAttatched = false;

        //spawning virus
        float time = 0;
        double InitialSpawnTime = 14; //amount of time that has to elapse before it explodes. 
        double spawnTime;
        double spawnTimeMultiplier = 1; //will be 1.5 when antibody is attatched. 
        int spawnCount = 0;



        // Start is called before the first frame update
        void Start()
        {
            canvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
            HBMscript = GameObject.Find("HealthBar").GetComponent<HealthBarManager>();
            InfectedCellManager = GameObject.Find("StaphInfection");
            VICMScript = GameObject.Find("InfectedCellManager").GetComponent<VirusAndInfectedCellManager>();

            spawnTime = InitialSpawnTime;

            w = canvas.GetComponent<RectTransform>().rect.width ;
            h = canvas.GetComponent<RectTransform>().rect.height;
            x = canvas.GetComponent<RectTransform>().rect.x * -1;
            y = canvas.GetComponent<RectTransform>().rect.y * -1;

            xOrigin = x - w / 2;
            yOrigin = y - h / 2;

            //get direction of unit vector
            float angle = Random.Range(0f, 90f);

            float radAngle = angle * Mathf.Deg2Rad;
            maxVelX = Mathf.Cos(radAngle) * 80;
            maxVelY = Mathf.Sin(radAngle) * 80;

            rb = GetComponent<Rigidbody>();
            rb.velocity = new Vector2((PlusMinus() * maxVelX), (PlusMinus() * maxVelY));

            objectWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x ;
            objectHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y;

            print("WIDTH: " + w + " HEIGHT: " + h + " xPos: " + x + " yPos: " + y + " xVel: " + rb.velocity.x + " yVel: " + rb.velocity.y);

        }

        // Update is called once per frame
        void Update()
        {
            GetComponent<SphereCollider>().enabled = true;
            rb.freezeRotation = true;

            time += Time.deltaTime;
            if (time >= spawnTime)
            {
                VICMScript.spawnVirus(transform.position.x, transform.position.y);
                spawnCount++;
                time = 0;
            }

            //this equation was determined by plotting a series of points and getting the curve from https://mycurvefit.com/. 
            spawnTime = (InitialSpawnTime - (-5.710064 / 0.2221271) * (1 - Mathf.Pow((float)2.718281828, (float)(-0.2221271 * spawnCount)))) * (float)spawnTimeMultiplier;

            //border movement
            if ((transform.position.x - objectWidth < xOrigin))
            { //hit the left border
                transform.position = new Vector2(xOrigin + objectWidth, transform.position.y);
                rb.velocity = new Vector2(-1 * rb.velocity.x, rb.velocity.y);
                HBMscript.takeDamage();

            }

            else if ((transform.position.x + objectWidth > xOrigin + w))
            { //hit the right border
                transform.position = new Vector2(xOrigin + w - objectWidth, transform.position.y);
                rb.velocity = new Vector2(-1 * rb.velocity.x, rb.velocity.y);
                HBMscript.takeDamage();

            }

            if (transform.position.y < yOrigin + (h * .15))
            { //hit the top border
                transform.position = new Vector2(transform.position.x, (float)(yOrigin + (h * .15)));
                rb.velocity = new Vector2(rb.velocity.x, -1 * rb.velocity.y);
                HBMscript.takeDamage();
            }

            else if (transform.position.y > yOrigin + (h * .85))
            { //hit the bottom border
                transform.position = new Vector2(transform.position.x, (float)(yOrigin + (h * .85)));
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

            else if (collision.gameObject.CompareTag("Neutrophil") || collision.gameObject.CompareTag("Cytokine") || collision.gameObject.CompareTag("Monocyte") || collision.gameObject.CompareTag("tCell"))
            {
                VICMScript.removeEnemyFromICList(this.gameObject);
                VICMScript.removeEnemyFromVICList(this.gameObject);
                Destroy(this.gameObject);
            }

         
        }
        public int PlusMinus()
        {
            int Sign = rnd.Next(0, 2);
            if (Sign == 0) { return -1; }
            if (Sign == 1) { return 1; }
            else { return 100; }
        }
        public void attatchAntibody()
        {
            spawnTimeMultiplier = 1.5f;
            rb.velocity = new Vector2(rb.velocity.x / 2, rb.velocity.y / 2);
        }
        public bool getIsTracked()
        {
            return isTracked;
        }
        public void setIsTracked(bool hi)
        {
            isTracked = hi;
        }
    }



}
