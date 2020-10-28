using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.UI;


namespace Bacteria
{

    public class TCell : MonoBehaviour
    {
        //components
        Rigidbody rb;
        System.Random rnd = new System.Random();
        //camera
        public float objectWidth;
        public float objectHeight;

        Canvas canvas;
        float w, h, x, y, xOrigin, yOrigin;
        float velX, velY;

        double posX, posY;

        SpawnFamiliars SFScript;

        // Start is called before the first frame update
        void Start()
        {
            //getting components
            canvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
            SFScript = GameObject.Find("ActionPanel").GetComponent<SpawnFamiliars>();

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

            //findAvailableTarget();
            //SFScript.setMonocyteTarget(targetedStaph);

            print("Monocyte: (" + transform.position.x + ", " + transform.position.y);
        }

        void FixedUpdate()
        {

            rb.freezeRotation = true;

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

         
        }

        void OnCollisionEnter(Collision collision)
        {

            if (collision.gameObject.CompareTag("Familiar"))
            {
            }
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
