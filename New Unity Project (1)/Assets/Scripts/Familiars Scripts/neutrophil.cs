using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.UI;


namespace Bacteria
{
    public class neutrophil : MonoBehaviour
    {
        //components
        Rigidbody rb;
        System.Random rnd = new System.Random();

        double maxHealth = 8;
        double health;

        //camera
        public float objectWidth;
        public float objectHeight;

        Canvas canvas;
        float w, h, x, y, xOrigin, yOrigin;

        double posX, posY;

        float xDistance, yDistance, xVel1, xVel2;
        float velX, velY, velRatio;

        SpawnFamiliars SFScript;
        HealthBarManager HBMscript;
        Color tempColor;

        bool isTrackedByVirus = false;

        // Start is called before the first frame update
        void Start()
        {
            //getting components
            canvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
            SFScript = GameObject.Find("ActionPanel").GetComponent<SpawnFamiliars>();
            HBMscript = GameObject.Find("HealthBar").GetComponent<HealthBarManager>();
            health = maxHealth;

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

            objectWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x;
            objectHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y;

            print("Monocyte: (" + transform.position.x + ", " + transform.position.y);
        }

        void FixedUpdate()
        {
            rb.freezeRotation = true;

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

            //make the monocyte become more transparent when it loses health. 
            tempColor = GetComponent<SpriteRenderer>().color;
            tempColor.a = (float)(0.5 * (1 + health / maxHealth));//set the minimum alpha value to be 1/2. The monocyte will get transparent as it loses health
            GetComponent<SpriteRenderer>().color = tempColor;

        }

        void OnCollisionEnter(Collision collision)
        {
            //neutrophil has certain health. Loses health upon collision. If health 0, then it dies.

            if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("InfectedCell"))
            {
                health--;
                if (health == 0)
                {
                    SFScript.removePhagocyte(this.gameObject);
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


        }//canvas

        public bool getIsTrackedByVirus()
        {
            return isTrackedByVirus;
        }

        public void setIsTrackedByVirus(bool hi)
        {
            isTrackedByVirus = hi;
        }

    }//class
}//namespace
