using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.UI;


namespace Bacteria
{

    public class phagocyte : MonoBehaviour
    {
        //components
        Rigidbody rb;
        System.Random rnd = new System.Random();

        int health = 10;

        //camera
        public float objectWidth;
        public float objectHeight;

        Canvas canvas;
        float w, h, x, y, xOrigin, yOrigin;

        double posX, posY;

        float currentDistance, closestDistance;
        float xDistance, yDistance, xVel1, xVel2;
        float velX, velY, velRatio;

        SpawnFamiliars SFScript;
        StaphSpawner SSScript;
        bool hasTarget = false;
        GameObject targetedStaph = null;

        bool isTrackedByVirus = false;

        // Start is called before the first frame update
        void Start()
        {
            //getting components
            canvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
            SFScript = GameObject.Find("ActionPanel").GetComponent<SpawnFamiliars>();

            if (GameObject.Find("StaphInfection") != null)
                SSScript = GameObject.Find("StaphInfection").GetComponent<StaphSpawner>();

            w = canvas.GetComponent<RectTransform>().rect.width;
            h = canvas.GetComponent<RectTransform>().rect.height;
            x = canvas.GetComponent<RectTransform>().rect.x * -1;
            y = canvas.GetComponent<RectTransform>().rect.y * -1;

            xOrigin = x - w / 2;
            yOrigin = y - h / 2;

            print("WIDTH: " + w + "HEIGHT: " + h + "xPos: " + x + "yPos: " + y);

            float angle = Random.Range(0f, 90f);
            float radAngle = angle * Mathf.Deg2Rad;
            velX = Mathf.Pow(Mathf.Cos(radAngle), 2) * 200;
            velY = Mathf.Pow(Mathf.Sin(radAngle), 2) * 200;

            rb = GetComponent<Rigidbody>();
            // rb.velocity = new Vector2((PlusMinus(true)*rnd.Next(30,150)), (PlusMinus(false) * rnd.Next(30,150)));
            rb.velocity = new Vector2((PlusMinus() * velX), (PlusMinus() * velY));

            objectWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x * 1.5f;
            objectHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y;

            //findAvailableTarget();
            //SFScript.setMonocyteTarget(targetedStaph);

            print("Monocyte: (" + transform.position.x + ", " + transform.position.y);
        }

        void FixedUpdate()
        {

            if ((transform.position.x - objectWidth < xOrigin))
            { //hit the left border
                transform.position = new Vector2(xOrigin + objectWidth, transform.position.y);
                rb.velocity = new Vector2(-1 * rb.velocity.x, rb.velocity.y);

            }

            else if ((transform.position.x + objectWidth > xOrigin + w))
            { //hit the right border
                transform.position = new Vector2(xOrigin + w - objectWidth, transform.position.y);
                rb.velocity = new Vector2(-1 * rb.velocity.x, rb.velocity.y);

            }

            if (transform.position.y < yOrigin + (h * .15))
            { //hit the top border
                transform.position = new Vector2(transform.position.x, (float)(yOrigin + (h * .15)));
                rb.velocity = new Vector2(rb.velocity.x, -1 * rb.velocity.y);
            }

            else if (transform.position.y > yOrigin + (h * .9))
            { //hit the bottom border
                transform.position = new Vector2(transform.position.x, (float)(yOrigin + (h * .9)));
                rb.velocity = new Vector2(rb.velocity.x, -1 * rb.velocity.y);
            }

            if (targetedStaph != null)
            {
                // determineVelocity();

            }

        }

        void OnCollisionEnter(Collision collision)
        {

            if (collision.gameObject.CompareTag("Familiar"))
            {
                rb.velocity = new Vector2(-1 * rb.velocity.x, -1 * rb.velocity.y);
            }
            //monocyte has certain health. Loses health upon collision. If health 0, then it dies.

            if (collision.gameObject.CompareTag("Enemy"))
            {
                health--;
                findAvailableTarget();         //find the next bacteria
                SFScript.setMonocyteTarget(targetedStaph);

                if (health == 0)
                {
                    if (targetedStaph != null) { targetedStaph.GetComponent<staph>().setIsTracked(false); }
                    SFScript.removeMonocyte(this.gameObject);
                    SFScript.removePhagocyte(this.gameObject);
                    Destroy(this.gameObject);
                }

            }
            print("HI I COLLIDED");
        }

        public int PlusMinus()
        {
            int Sign = rnd.Next(0, 2);

            if (Sign == 0) { return -1; }
            if (Sign == 1) { return 1; }
            else { return 100; }
        }


        public void determineVelocity()   //almost copied from StaphAntibody
        {
            //determine velocity so that antibody will be on a collision course with the bacteria.
            //first find the distances, and then find the ratio such that when each x and y distance is multiplied, it will create velocities which have the correct direction
            //and the coorect velocity (which is 70)

            //next, because the bacteria is moving, add the x and y velocity of the bacteria to the antibody, to anticipate the collision, and set that back to the max velocity through ratio.
            if (getDistanceFromTargetedStaph(targetedStaph) > 10)
            {
                xDistance = transform.position.x - targetedStaph.transform.position.x;
                yDistance = transform.position.y - targetedStaph.transform.position.y;
                velRatio = rb.velocity.magnitude / (Mathf.Pow((Mathf.Pow(xDistance, 2)) + (Mathf.Pow(yDistance, 2)), 0.5f));

                velX = xDistance * velRatio;
                velY = yDistance * velRatio;

                rb.velocity = new Vector2(-velX, -velY);   //negative for some reason...

            }
            else //to prevent when the monocyte is close to the bacteria
            {
                //rb.velocity = new Vector2(targetedStaph.GetComponent<Rigidbody>().velocity.x, targetedStaph.GetComponent<Rigidbody>().velocity.y);
                //slow down bacteria until v = 0. For some reason, creating new vector2 with(0,0) does not make it stop. 
                targetedStaph.GetComponent<Rigidbody>().velocity = new Vector2(targetedStaph.GetComponent<Rigidbody>().velocity.x / 2, targetedStaph.GetComponent<Rigidbody>().velocity.y / 2);
            }

        }

        public void findAvailableTarget()
        {
            //this will look for the first instance of a staph that has an antibody attached and set is as the target to track. 
            for (int i = 0; i < SSScript.getStaphList().Count; i++)
            {
                if (SSScript.getStaphListElement(i).GetComponent<staph>().getAntibodyAttached())
                {
                    targetedStaph = SSScript.getStaphListElement(i);
                    i = SSScript.getStaphList().Count; //get out of loop
                    return;
                }

            }
            targetedStaph = null;
        }

        public float getDistanceFromTargetedStaph(GameObject staph)
        {
            return Mathf.Pow(Mathf.Pow(staph.GetComponent<Transform>().position.x, 2) + Mathf.Pow(staph.GetComponent<Transform>().position.y, 2), 2);
        }
        public void setTargetedStaph(GameObject staph)
        {
            targetedStaph = staph;
            hasTarget = true;
        }

        public bool getHasTarget()
        {
            return hasTarget;
        }

        public void setHasTarget(bool Hastarget)
        {
            hasTarget = Hastarget;
        }
        public bool getIsTrackedByVirus()
        {
            return isTrackedByVirus;
        }

        public void setIsTrackedByVirus(bool hi)
        {
            isTrackedByVirus = hi;
        }

    }//canvas

}//namespace
