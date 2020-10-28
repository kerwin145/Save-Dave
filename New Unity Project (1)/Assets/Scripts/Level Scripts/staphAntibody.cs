using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Bacteria
{

    public class staphAntibody : MonoBehaviour
    {
        //components
        Rigidbody rb;  //the max maginuted of the velocity will be 70
        System.Random rnd = new System.Random();

        public float objectWidth;
        public float objectHeight;

        GameObject StaphInfection;
        public StaphSpawner SSScript;
        GameObject SpawnFamiliarsGO;
        public SpawnFamiliars SFScript;

        //these will be used to get the staph from the allstpah lsit that is the closest to the antibody. 
        GameObject currentBacteria;//the bacteria that is being compared in the for loop
        GameObject closestBacteria = null;  //the bacteria with the least distance               
        float currentDistance, closestDistance;
        float xDistance, yDistance, totalDistance;
        //these will be used to determine the velocity so that it will catch up to the staph
        float maxVelocity = 120;
        float velX, velY, velRatio;

        Canvas canvas;
        float w, h, x, y, xOrigin, yOrigin;
        bool touchAntibody = false;

        // Start is called before the first frame update
        void Start()
        {
            //getting components
            canvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
            StaphInfection = GameObject.Find("StaphInfection");
            SSScript = StaphInfection.GetComponent<StaphSpawner>();

            SpawnFamiliarsGO = GameObject.Find("ActionPanel");
            SFScript = SpawnFamiliarsGO.GetComponent<SpawnFamiliars>();

            rb = GetComponent<Rigidbody>();

            w = canvas.GetComponent<RectTransform>().rect.width;
            h = canvas.GetComponent<RectTransform>().rect.height;
            x = canvas.GetComponent<RectTransform>().rect.x * -1;
            y = canvas.GetComponent<RectTransform>().rect.y * -1;

            xOrigin = x - w / 2;
            yOrigin = y - h / 2;

            findTarget();                                     

            objectWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x / 2;// * transform.localScale.x;
            objectHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2;// * transform.localScale.y;

        }
 
        void FixedUpdate()
        {
            if (!(closestBacteria == null))
            {
                xDistance = transform.position.x - closestBacteria.transform.position.x;
                yDistance = transform.position.y - closestBacteria.transform.position.y;
                totalDistance = Mathf.Pow(Mathf.Pow(xDistance, 2) + Mathf.Pow(yDistance, 2), 0.5f);
                determineVelocity();

                if ((totalDistance <= 5) && (!touchAntibody))
                {
                    print("HI I, staphAntibody, HAVE MADE CONTACT");
                    closestBacteria.GetComponent<staph>().attatchAntibody();
                    SFScript.setMonocyteTarget(closestBacteria); //look for an available monocyte to set its target to closest bacteria
                    touchAntibody = true;
                }
            }
            else
            {
                print("My job has been done");
                Destroy(this.gameObject);
            }
   
         
        }

        void OnCollisionEnter(Collision other)
        {
        }

        public void determineVelocity()
        {
            //determine velocity so that antibody will be on a collision course with the bacteria.
            //first find the distances, and then find the ratio such that when each x and y distance is multiplied, it will create velocities which have the correct direction
            //and the coorect velocity (which is 70)

            //next, because the bacteria is moving, add the x and y velocity of the bacteria to the antibody, to anticipate the collision, and set that back to the max velocity through ratio.
            

            if (totalDistance >= 5) //do the velocity thing. Number is determined through trial and error
            {
                velRatio = maxVelocity / (Mathf.Pow((Mathf.Pow(xDistance, 2)) + (Mathf.Pow(yDistance, 2)), 0.5f));

                velX = xDistance * velRatio;
                velY = yDistance * velRatio;

                rb.velocity = new Vector2(-velX, -velY);   //negative for some reason...
            }
            else       //set velocity equal to the bacteria when it is close. 
            {
                rb.velocity = new Vector2(closestBacteria.GetComponent<Rigidbody>().velocity.x, closestBacteria.GetComponent<Rigidbody>().velocity.y);   
            }

        }

        public void findTarget()
        {
            int firstElementIndex = 0;
            //determine first element as the first one in the list which is not tracked. 
                                              
            for (int i = 0; i < SSScript.getStaphList().Count; i++)
            {
                if (!SSScript.getStaphListElement(i).GetComponent<staph>().getIsTracked())
                {
                    closestBacteria = SSScript.getStaphListElement(i);
                    closestDistance = Mathf.Pow(Mathf.Pow((closestBacteria.transform.position.x - transform.position.x), 2) + Mathf.Pow((closestBacteria.transform.position.y - transform.position.y), 2), 0.5f);
                    firstElementIndex = i;
                    i = SSScript.getStaphList().Count; //get out of loop
                }
                
            }

            if (closestBacteria == null)
            {
                print("I HAVE NO ONE TO GO TO :(");
                Destroy(this.gameObject);
            }

            //start at one, since we already have currentBacteria to be 0th element. 
            for (int i = firstElementIndex + 1; i < SSScript.getStaphList().Count; i++)
            {
                currentBacteria = SSScript.getStaphListElement(i);//the gameobject that is being compared in the for loop
                currentDistance = Mathf.Pow(Mathf.Pow((currentBacteria.transform.position.x - transform.position.x), 2) + Mathf.Pow((currentBacteria.transform.position.y - transform.position.y), 2), 0.5f);

                if (currentDistance <= closestDistance)
                {
                    if (!currentBacteria.GetComponent<staph>().getIsTracked())
                    {
                        closestBacteria = currentBacteria;
                        closestDistance = Mathf.Pow(Mathf.Pow((closestBacteria.transform.position.x - transform.position.x), 2) + Mathf.Pow((closestBacteria.transform.position.y - transform.position.y), 2), 0.5f);
                    }

                }

            }

            closestBacteria.GetComponent<staph>().setIsTracked(true);
        }



    }//canvas

}//namespace
