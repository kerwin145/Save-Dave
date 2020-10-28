using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Bacteria
{

    public class Virus : MonoBehaviour
    {
        //components
        HealthBarManager HBMscript;
        Rigidbody rb;  //the max maginuted of the velocity will be 70
        System.Random rnd = new System.Random();

        public float objectWidth;
        public float objectHeight;

        GameObject phagocyteTarget;
        public SpawnFamiliars SFScript;
        public GameObject infectedCell;
        public VirusAndInfectedCellManager VICMScript;

        //these will be used to get the staph from the allstpah lsit that is the closest to the antibody. 
        GameObject tempPhagocyte;//the phagocyte that is being compared in the for loop
        GameObject closestPhagocyte = null;  //the bacteria with the least distance               
        float currentDistance, closestDistance;
        float xDistance, yDistance, totalDistance;
        float maxVelocity = 140;
        float velX, velY, velRatio;

        Canvas canvas;
        float w, h, x, y, xOrigin, yOrigin;

        bool isTracked = false;
        bool touchAntibody = false;                                
        int randomCollisionUpperbound = 20;

        float trackDieTime = 45; //dies after a certain amount of time tracking a phagocyte
        float time;

        // Start is called before the first frame update
        void Start()
        {
            //getting components
            canvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
            SFScript = GameObject.Find("ActionPanel").GetComponent<SpawnFamiliars>();
            HBMscript = GameObject.Find("HealthBar").GetComponent<HealthBarManager>();
            VICMScript = GameObject.Find("InfectedCellManager").GetComponent<VirusAndInfectedCellManager>();

            rb = GetComponent<Rigidbody>();

            w = canvas.GetComponent<RectTransform>().rect.width ;
            h = canvas.GetComponent<RectTransform>().rect.height;
            x = canvas.GetComponent<RectTransform>().rect.x * -1;
            y = canvas.GetComponent<RectTransform>().rect.y * -1;

            xOrigin = x - w / 2;
            yOrigin = y - h / 2;

            // findTarget();

            objectWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x;
            objectHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y;
            findTarget();

            float angle = Random.Range(0f, 90f);
            float radAngle = angle * Mathf.Deg2Rad;
            velX = Mathf.Pow(Mathf.Cos(radAngle), 2) * maxVelocity;
            velY = Mathf.Pow(Mathf.Sin(radAngle), 2) * maxVelocity;

            rb = GetComponent<Rigidbody>();
            // rb.velocity = new Vector2((PlusMinus(true)*rnd.Next(30,150)), (PlusMinus(false) * rnd.Next(30,150)));
            rb.velocity = new Vector2((PlusMinus() * velX), (PlusMinus() * velY));

            time = 0;
        }

        void FixedUpdate()
        {
            //die after a certain amount of time pursueing the closest phagocyte with fail of contact. 
           

            rb.freezeRotation = true;

            if (closestPhagocyte != null)
            {
                xDistance = transform.position.x - closestPhagocyte.transform.position.x;
                yDistance = transform.position.y - closestPhagocyte.transform.position.y;
                totalDistance = Mathf.Pow(Mathf.Pow(xDistance, 2) + Mathf.Pow(yDistance, 2), 0.5f);
                determineVelocity();

                time += Time.deltaTime;
                if (time > trackDieTime)
                {
                    if (closestPhagocyte.tag == "Monocyte") { closestPhagocyte.GetComponent<monocyte>().setIsTrackedByVirus(false); }
                    else { closestPhagocyte.GetComponent<neutrophil>().setIsTrackedByVirus(false); }
                    VICMScript.removeEnemyFromVICList(this.gameObject);
                    Destroy(this.gameObject);

                }

                if ((totalDistance <= 8))
                {
                    int random = rnd.Next(0, randomCollisionUpperbound);
                    if (random < 10 )
                    {
                        VICMScript.SpawnInfectedCells(transform.position.x, transform.position.y);
                        if (closestPhagocyte.tag == "Monocyte") { SFScript.removeMonocyte(closestPhagocyte); }

                        SFScript.removePhagocyte(closestPhagocyte);   
                        Destroy(closestPhagocyte.gameObject);
                    }
                    else
                    {
                        if (closestPhagocyte.tag == "Monocyte") { closestPhagocyte.GetComponent<monocyte>().setIsTrackedByVirus(false); }
                        else { closestPhagocyte.GetComponent<neutrophil>().setIsTrackedByVirus(false); }
                    }
                    VICMScript.removeEnemyFromVICList(this.gameObject);
                    Destroy(this.gameObject);
                }
            }
            else //float around and look for available phagocytes         
            {
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

                if (transform.position.y < yOrigin + (h * .17))
                { //hit the top border
                    transform.position = new Vector2(transform.position.x, (float)(yOrigin + (h * .17)));
                    rb.velocity = new Vector2(rb.velocity.x, -1 * rb.velocity.y);
                    HBMscript.takeDamage();
                }

                else if (transform.position.y > yOrigin + (h * .88))
                { //hit the bottom border
                    transform.position = new Vector2(transform.position.x, (float)(yOrigin + (h * .88)));
                    rb.velocity = new Vector2(rb.velocity.x, -1 * rb.velocity.y);
                    HBMscript.takeDamage();
                }

                //look for phagocyte
                if (availableTarget())
                    findTarget();

            }

        }

        void OnTriggerEnter(Collider other)
        {

            if (other.gameObject.CompareTag("Familiar"))
            {
            }
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
                rb.velocity = new Vector2(closestPhagocyte.GetComponent<Rigidbody>().velocity.x, closestPhagocyte.GetComponent<Rigidbody>().velocity.y);
            }

        }

//NOOOOOoooOOOOooo 
        public void findTarget()
        {   
            /*
             * the main difference between this and the findTarget method of StaphAntibody is that phagocyte can be either a monocyte and neutrophil.
             * Thus, any time we want to change a property of currentPhagocyte, we will have detemrine what type of phagocyte it is. 
             * This might be able to be solved through inheritance, but I will have to figure that out some time in the future. 
             */
            print("NOW FINDING TARGET-----");
            int firstElementIndex = 0;
            GameObject tempPhagocyte = null;
            bool tempIsTracked = false;
            //determine first element as the first one in the list which is not tracked. This is so we have a reference point for the closest phag, and the closest distance. 

            for (int i = 0; i < SFScript.getPhagocyteList().Count; i++)
            {
                //uhhh i wonder if this could be done through inheritance...
                tempPhagocyte = SFScript.getPhagocyteListElement(i);
          
                if (tempPhagocyte.tag == "Monocyte") { tempIsTracked = tempPhagocyte.GetComponent<monocyte>().getIsTrackedByVirus(); }
                else {tempIsTracked = tempPhagocyte.GetComponent<neutrophil>().getIsTrackedByVirus();}

                if (!tempIsTracked)
                {
                    closestPhagocyte = tempPhagocyte; //establish this as the closest. HOWEVER, it may not be; it is only a starting point. 
                    //establish the closest distance as the distance of this game object. 
                    closestDistance = Mathf.Pow(Mathf.Pow((closestPhagocyte.transform.position.x - transform.position.x), 2) + Mathf.Pow((closestPhagocyte.transform.position.y - transform.position.y), 2), 0.5f);
                    firstElementIndex = i;
                    i = SFScript.getPhagocyteList().Count; //get out of loop
                }
                                                       
            }
            if (closestPhagocyte == null) {
                print(" I Have looked through the list, and can not determine the first index." + closestDistance);
                closestDistance = -1; //no comparison will be made in the next loop that could potentially change this null closest phagocyte
            }
            
            print("First element index is " + firstElementIndex);
            tempIsTracked = false; //reset this, b/c now, you will be going through the whole array. It won't be the same game object anymore. 
          
            //start comparing at one, since we already have currentPhagocyte to be 0th element. 
            //look through the array to find the closest phagocyte that is not tracked. 
            for (int i = firstElementIndex + 1; i < SFScript.getPhagocyteList().Count; i++)
            {
                tempPhagocyte = SFScript.getPhagocyteListElement(i);//the gameobject that is being compared in the for loop
                //get the distance from this game object.
                currentDistance = Mathf.Pow(Mathf.Pow((tempPhagocyte.transform.position.x - transform.position.x), 2) + Mathf.Pow((tempPhagocyte.transform.position.y - transform.position.y), 2), 0.5f);

                if (currentDistance <= closestDistance)
                {
                    if (tempPhagocyte.tag == "Monocyte") { tempIsTracked = tempPhagocyte.GetComponent<monocyte>().getIsTrackedByVirus(); }
                    else{ tempIsTracked = tempPhagocyte.GetComponent<neutrophil>().getIsTrackedByVirus(); }

                    if (!tempIsTracked)//if distance is less, and the current phagocyte is not tracked. 
                    {
                        print("temptracked for the phagocyte that is now closest:"  + tempIsTracked);
                        closestPhagocyte = tempPhagocyte;
                        closestDistance = Mathf.Pow(Mathf.Pow((closestPhagocyte.transform.position.x - transform.position.x), 2) + Mathf.Pow((closestPhagocyte.transform.position.y - transform.position.y), 2), 0.5f);
                    }
                }
            }
            //after looping, mark the closest phagocyte as tracked.   
            //Make a final check. In the zeroth case, or maybe some other case, the closest element could still have isTracked = true.
            //In this case, make closest phagocyte to be null. 
            if (closestPhagocyte != null)
            {
                if (closestPhagocyte.tag == "Monocyte") tempIsTracked = closestPhagocyte.GetComponent<monocyte>().getIsTrackedByVirus();
                else { tempIsTracked = closestPhagocyte.GetComponent<neutrophil>().getIsTrackedByVirus(); }

                if (tempIsTracked) { closestPhagocyte = null; }
                else
                {
                    if (closestPhagocyte.tag == "Monocyte") { closestPhagocyte.GetComponent<monocyte>().setIsTrackedByVirus(true); }
                    else { closestPhagocyte.GetComponent<neutrophil>().setIsTrackedByVirus(true); }
                }
            }

        }

        //virus will be consistenly checking for available targets. Instead of running findTarget nonstop, only run it when there is an available phagocyte. 
        public bool availableTarget()
        {
            GameObject tempPhagocyte;
            bool result = false;
            for(int i = 0; i < SFScript.getPhagocyteList().Count; i++)
            {
                tempPhagocyte = SFScript.getPhagocyteListElement(i); 
                if (tempPhagocyte.tag == "Monocyte"){ result = tempPhagocyte.GetComponent<monocyte>().getIsTrackedByVirus(); }
                else { result = tempPhagocyte.GetComponent<neutrophil>().getIsTrackedByVirus(); }
                //if the current phagocyte is not tracked by virus, return true. 
                if (!result) { return true; }
            }

            return false;
        }

        public GameObject getClosestPhagocyte()
        {
            return  closestPhagocyte;
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
            touchAntibody = true;
            randomCollisionUpperbound = 40;
            maxVelocity /= 2;
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


    }//canvas

}//namespace
