using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Bacteria
{
    public class bcell : MonoBehaviour
    {
        //components
        Rigidbody rb;  //the max maginuted of the velocity will be 70
        System.Random rnd = new System.Random();

        public float objectWidth;
        public float objectHeight;

        float xDistance, yDistance, xVel1, xVel2;
        double posX, posY;

        public GameObject antibody;   
        int numAntibodySpawned = 5;

        Canvas canvas;
        float w, h, x, y, xOrigin, yOrigin;
        int xPosMin, xPosMax, yPosMin, yPosMax;


        float spawnDelay = 1, time = 0;

        // Start is called before the first frame update
        void Start()
        {
            //getting components
            canvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();

            rb = GetComponent<Rigidbody>();

            w = canvas.GetComponent<RectTransform>().rect.width;
            h = canvas.GetComponent<RectTransform>().rect.height;
            x = canvas.GetComponent<RectTransform>().rect.x * -1;
            y = canvas.GetComponent<RectTransform>().rect.y * -1;

            xPosMin = (int)(canvas.GetComponent<RectTransform>().rect.x * -1 - canvas.GetComponent<RectTransform>().rect.width / 2);
            xPosMax = (int)(canvas.GetComponent<RectTransform>().rect.x * -1 + canvas.GetComponent<RectTransform>().rect.width / 2);
            yPosMin = (int)(canvas.GetComponent<RectTransform>().rect.y * -1 - canvas.GetComponent<RectTransform>().rect.height / 2 * .7);
            yPosMax = (int)(canvas.GetComponent<RectTransform>().rect.y * -1 + canvas.GetComponent<RectTransform>().rect.height / 2 * .9);

            xOrigin = x - w / 2;
            yOrigin = y - h / 2;

            objectWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x / 2;// * transform.localScale.x;
            objectHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2;// * transform.localScale.y;

        }

        // Update is called once per frame
        void Update()
        {
            rb.velocity = new Vector2(0, 0);

            time += Time.deltaTime;

            if ((time >= spawnDelay) && (numAntibodySpawned > 0))
            {
                GameObject newAntibody = (GameObject)Instantiate(antibody, new Vector3(transform.position.x, transform.position.y, 474f), Quaternion.identity);

                // GameObject newAntibody = (GameObject)Instantiate(Antibody, new Vector3(rnd.Next(xPosMin, xPosMax), rnd.Next(yPosMin, (int)(yPosMax)), 474f), Quaternion.identity);
                time = 0;
                numAntibodySpawned--;
            }
            if (numAntibodySpawned == 0)
            {
                Destroy(this.gameObject);
            }

        }
    }

}//namespace