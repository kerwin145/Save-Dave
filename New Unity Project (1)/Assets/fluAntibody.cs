using UnityEngine;

namespace Bacteria
{

    public class fluAntibody : MonoBehaviour
    {
        //components
        HealthBarManager HBMscript;
        Rigidbody rb;  //the max maginuted of the velocity will be 70
        System.Random rnd = new System.Random();

        public float objectWidth;
        public float objectHeight;

        VirusAndInfectedCellManager VICMScript;

        //these will be used to get the staph from the allstpah lsit that is the closest to the antibody. 
        GameObject tempEnemy;//the phagocyte that is being compared in the for loop
        GameObject closestEnemy = null;  //the bacteria with the least distance               
        float currentDistance, closestDistance;
        float xDistance, yDistance, totalDistance;
        float maxVelocity = 180;
        float velX, velY, velRatio;

        Canvas canvas;
        float w, h, x, y, xOrigin, yOrigin;

        bool touchEnemy = false;

        // Start is called before the first frame update
        void Start()
        {
            //getting components
            canvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
            HBMscript = GameObject.Find("HealthBar").GetComponent<HealthBarManager>();
            VICMScript = GameObject.Find("InfectedCellManager").GetComponent<VirusAndInfectedCellManager>();

            rb = GetComponent<Rigidbody>();

            w = canvas.GetComponent<RectTransform>().rect.width * .9f;
            h = canvas.GetComponent<RectTransform>().rect.height;
            x = canvas.GetComponent<RectTransform>().rect.x * -1;
            y = canvas.GetComponent<RectTransform>().rect.y * -1;

            xOrigin = x - w / 2;
            yOrigin = y - h / 2;

            // findTarget();

            objectWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x * 1.5f;
            objectHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y;

            float angle = Random.Range(0f, 90f);
            float radAngle = angle * Mathf.Deg2Rad;
            velX = Mathf.Pow(Mathf.Cos(radAngle), 2) * maxVelocity;
            velY = Mathf.Pow(Mathf.Sin(radAngle), 2) * maxVelocity;

            rb = GetComponent<Rigidbody>();
            // rb.velocity = new Vector2((PlusMinus(true)*rnd.Next(30,150)), (PlusMinus(false) * rnd.Next(30,150)));
            rb.velocity = new Vector2((PlusMinus() * velX), (PlusMinus() * velY));
            
            findTarget();

        }

        void FixedUpdate()
        {
            rb.freezeRotation = true;

            if (closestEnemy!= null)
            {
                xDistance = transform.position.x - closestEnemy.transform.position.x;
                yDistance = transform.position.y - closestEnemy.transform.position.y;
                totalDistance = Mathf.Pow(Mathf.Pow(xDistance, 2) + Mathf.Pow(yDistance, 2), 0.5f);
                determineVelocity();

                if ((totalDistance <= 5) && !touchEnemy)
                {
                    if (closestEnemy.tag == "InfectedCell") { closestEnemy.GetComponent<infectedCell>().attatchAntibody(); }
                    else {closestEnemy.GetComponent<Virus>().attatchAntibody(); }
                    touchEnemy = true;
                    maxVelocity = 300;
                }
            }
            else if (closestEnemy == null && touchEnemy) //if it has performed its role, and the enemy it is attatched to has died. 
            {
                Destroy(this.gameObject);
            }
            else
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

                if (transform.position.y < yOrigin + (h * .17))
                { //hit the top border
                    transform.position = new Vector2(transform.position.x, (float)(yOrigin + (h * .17)));
                    rb.velocity = new Vector2(rb.velocity.x, -1 * rb.velocity.y);
                }

                else if (transform.position.y > yOrigin + (h * .88))
                { //hit the bottom border
                    transform.position = new Vector2(transform.position.x, (float)(yOrigin + (h * .88)));
                    rb.velocity = new Vector2(rb.velocity.x, -1 * rb.velocity.y);
                }


                //look for enemy
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
                rb.velocity = new Vector2(closestEnemy.GetComponent<Rigidbody>().velocity.x, closestEnemy.GetComponent<Rigidbody>().velocity.y);
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
            GameObject tempEnemy = null;
            bool tempIsTracked = false;
            //determine first element as the first one in the list which is not tracked. This is so we have a reference point for the closest phag, and the closest distance. 
            print("Size of VICList: " + VICMScript.getVICList().Count);

            for (int i = 0; i < VICMScript.getVICList().Count; i++)
            {
                //uhhh i wonder if this could be done through inheritance...
                tempEnemy = VICMScript.getVICListElement(i);

                if (tempEnemy.tag == "InfectedCell") { tempIsTracked = tempEnemy.GetComponent<infectedCell>().getIsTracked(); }
                else { tempIsTracked = tempEnemy.GetComponent<Virus>().getIsTracked(); }
                print("HALLO temp is tracked is " + tempIsTracked);
                if (!tempIsTracked)
                {
                    closestEnemy = tempEnemy; //establish this as the closest. HOWEVER, it may not be; it is only a starting point. 
                    //establish the closest distance as the distance of this game object. 
                    closestDistance = Mathf.Pow(Mathf.Pow((closestEnemy.transform.position.x - transform.position.x), 2) + Mathf.Pow((closestEnemy.transform.position.y - transform.position.y), 2), 0.5f);
                    firstElementIndex = i;
                    i = VICMScript.getVICList().Count; //get out of loop
                }

            }
            if (closestEnemy == null)
            {
                print(" I Have looked through the list, and can not determine the first index. Time to leave!");      
                closestDistance = -1; //no comparison will be made in the next loop that could potentially change this null closest phagocyte
            }

            print("First element index is " + firstElementIndex);
            tempIsTracked = false; //reset this, b/c now, you will be going through the whole array. It won't be the same game object anymore. 

            //start comparing at one, since we already have currentPhagocyte to be 0th element. 
            //look through the array to find the closest phagocyte that is not tracked. 
            for (int i = firstElementIndex + 1; i < VICMScript.getVICList().Count; i++)
            {
                tempEnemy = VICMScript.getVICListElement(i);//the gameobject that is being compared in the for loop
                //get the distance from this game object.
                currentDistance = Mathf.Pow(Mathf.Pow((tempEnemy.transform.position.x - transform.position.x), 2) + Mathf.Pow((tempEnemy.transform.position.y - transform.position.y), 2), 0.5f);

                if (currentDistance <= closestDistance)
                {
                    if (tempEnemy.tag == "InfectedCell") { tempIsTracked = tempEnemy.GetComponent<infectedCell>().getIsTracked(); }
                    else { tempIsTracked = tempEnemy.GetComponent<Virus>().getIsTracked(); }

                    if (!tempIsTracked)//if distance is less, and the current phagocyte is not tracked. 
                    {
                        print("temptracked for the phagocyte that is now closest:" + tempIsTracked);
                        closestEnemy = tempEnemy;
                        closestDistance = Mathf.Pow(Mathf.Pow((closestEnemy.transform.position.x - transform.position.x), 2) + Mathf.Pow((closestEnemy.transform.position.y - transform.position.y), 2), 0.5f);
                    }
                }
            }
            //after looping, mark the closest phagocyte as tracked.   
            //Make a final check. In the zeroth case, or maybe some other case, the closest element could still have isTracked = true.
            //In this case, make closest phagocyte to be null. 
            if (closestEnemy != null)
            {
                if (closestEnemy.tag == "InfectedCell") { tempIsTracked = closestEnemy.GetComponent<infectedCell>().getIsTracked(); }
                else { tempIsTracked = closestEnemy.GetComponent<Virus>().getIsTracked(); }

                if (tempIsTracked) { closestEnemy = null; }
                else
                {
                    if (closestEnemy.tag == "InfectedCell") {closestEnemy.GetComponent<infectedCell>().setIsTracked(true); }
                    else { closestEnemy.GetComponent<Virus>().setIsTracked(true); }
                }
            }

        }

        public bool availableTarget()
        {
            GameObject tempEnemy;
            bool result = false;
            for (int i = 0; i < VICMScript.getVICList().Count; i++)
            {
                tempEnemy = VICMScript.getVICListElement(i);
                if (tempEnemy.tag == "InfectedCell") { result = tempEnemy.GetComponent<infectedCell>().getIsTracked(); }
                else { result = tempEnemy.GetComponent<Virus>().getIsTracked(); }
                //if the current phagocyte is not tracked by virus, return true. 
                if (!result) { return true; }
            }

            return false;
        }


        public GameObject getClosestEnemy()
        {
            return closestEnemy;
        }
        public int PlusMinus()
        {
            int Sign = rnd.Next(0, 2);

            if (Sign == 0) { return -1; }
            if (Sign == 1) { return 1; }
            else { return 100; }
        }
      


    }//canvas

}//namespace
