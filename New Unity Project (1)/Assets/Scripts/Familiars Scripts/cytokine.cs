using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bacteria
{

    public class cytokine : MonoBehaviour
    {
        //components
        Rigidbody rb;
        System.Random rnd = new System.Random();

        int health = 1;

        //camera
        public float objectWidth;
        public float objectHeight;

        HealthBarManager HBMscript;

        Canvas canvas;
        float w, h, x, y, xOrigin, yOrigin;

        double posX, posY;
        float maxVelX, maxVelY, maxVelMag;

        // Start is called before the first frame update
        void Start()
        {
            //getting components
            canvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
            HBMscript = GameObject.Find("HealthBar").GetComponent<HealthBarManager>();

            w = canvas.GetComponent<RectTransform>().rect.width;
            h = canvas.GetComponent<RectTransform>().rect.height;
            x = canvas.GetComponent<RectTransform>().rect.x * -1;
            y = canvas.GetComponent<RectTransform>().rect.y * -1;

            xOrigin = x - w / 2;
            yOrigin = y - h / 2;

            print("WIDTH: " + w + "HEIGHT: " + h + "xPos: " + x + "yPos: " + y);

            float angle = Random.Range(0f, 90f);
            float radAngle = angle * Mathf.Deg2Rad;
            maxVelX = Mathf.Pow(Mathf.Cos(radAngle), 2) * 200;
            maxVelY = Mathf.Pow(Mathf.Sin(radAngle), 2) * 200;

            rb = GetComponent<Rigidbody>();
            // rb.velocity = new Vector2((PlusMinus(true)*rnd.Next(30,150)), (PlusMinus(false) * rnd.Next(30,150)));
            rb.velocity = new Vector2((PlusMinus() * maxVelX), (PlusMinus() * maxVelY));

            objectWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x;
            objectHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y;

            print("Cytokine: (" + transform.position.x + ", " + transform.position.y);
        }

        void FixedUpdate()
        {
            maxVel();
            if ((transform.position.x - objectWidth < xOrigin))
            { //hit the left border
                transform.position = new Vector2(xOrigin + objectWidth, transform.position.y);
                rb.velocity = new Vector2(-1 * rb.velocity.x, rb.velocity.y);
                //HBMscript.takeDamage();

            }

            else if ((transform.position.x + objectWidth > xOrigin + w))
            { //hit the right border
                transform.position = new Vector2(xOrigin + w - objectWidth, transform.position.y);
                rb.velocity = new Vector2(-1 * rb.velocity.x, rb.velocity.y);
                //HBMscript.takeDamage();

            }

            if (transform.position.y < yOrigin + (h * .15))
            { //hit the top border
                transform.position = new Vector2(transform.position.x, (float)(yOrigin + (h * .15)));
                rb.velocity = new Vector2(rb.velocity.x, -1 * rb.velocity.y);
                //HBMscript.takeDamage();
            }

            else if (transform.position.y > yOrigin + (h * .9))
            { //hit the bottom border
                transform.position = new Vector2(transform.position.x, (float)(yOrigin + (h * .9)));
                rb.velocity = new Vector2(rb.velocity.x, -1 * rb.velocity.y);
                //HBMscript.takeDamage();
            }


        }

        void OnCollisionEnter(Collision collision)
        {

            if (collision.gameObject.CompareTag("Familiar"))
            {
                rb.velocity = new Vector2(-1 * rb.velocity.x, -1 * rb.velocity.y);
            }
            //cytokine has certain health. Loses health upon collision. If health 0, then it dies.

            if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("InfectedCell"))
            {
                health--;
                if (health == 0)
                    Destroy(this.gameObject);

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


        public void maxVel()
        {
            //this is to make the magnitude of the velocity the same if the bacteria ever loses velocity during a collision
            //take the current and maxVel velocity magnitudes, but keep them squared.
            //divide them to make the ratio. Multiply both the current x and y vel by the ratio. This will make the magnitude that is squared now the same.
            //NOTE: take the absolute value of the ratio so as to not change direction. 
            //square root the resulting x and y velocities. 

            //Vector2 vel = new Vector2(rb.velocity.x, rb.velocity.y);
            Vector2 maxVel = new Vector2(maxVelX, maxVelY);

            float maxVeltoVelRatio = Mathf.Abs((Mathf.Pow(maxVelX, 2) + Mathf.Pow(maxVelY, 2)) / (Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.y, 2)));

            if (rb.velocity.magnitude < maxVel.magnitude)
            {
                float x = (Mathf.Pow((Mathf.Pow(rb.velocity.x, 2) * maxVeltoVelRatio), .5f));
                float y = (Mathf.Pow((Mathf.Pow(rb.velocity.y, 2) * maxVeltoVelRatio), .5f));

                rb.velocity = new Vector2(x, y);
            }
        }


    }//canvas

}//namespace
