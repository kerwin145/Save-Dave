using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Bacteria
{

    public class StaphSpawner : MonoBehaviour
    {
        public GameObject staph;
        List<GameObject> Allstaph;

        int xPosMin, xPosMax, yPosMin, yPosMax;

        public int numEnemyAtStart; //we can try and make this random later.
        int wave = 0;
        int duplicateInstances;

        float time = 0;
        float baseCoolDown = 10;
        float cooldown;

        public Canvas canvas;
        public GameObject winUI;

        System.Random rnd = new System.Random();

        // Start is called before the first frame update
        void Start()
        {
            cooldown = baseCoolDown;

            Allstaph = new List<GameObject>();

            xPosMin = (int)(canvas.GetComponent<RectTransform>().rect.x * -1 - canvas.GetComponent<RectTransform>().rect.width / 2);
            xPosMax = (int)(canvas.GetComponent<RectTransform>().rect.x * -1 + canvas.GetComponent<RectTransform>().rect.width / 2);
            yPosMin = (int)(canvas.GetComponent<RectTransform>().rect.y * -1 - canvas.GetComponent<RectTransform>().rect.height / 2 * .7);
            yPosMax = (int)(canvas.GetComponent<RectTransform>().rect.y * -1 + canvas.GetComponent<RectTransform>().rect.height / 2 * .9);

            SpawnStaph();
            print("NumEnemy: " + Allstaph.Count);
        }
        // /*
        // Update is called once per frame
        void Update()
        {
            time += Time.deltaTime;
            if (time >= cooldown)
            {
                duplicateInstances = Allstaph.Count;
                for (int i = 0; i < duplicateInstances; i++)
                {
                    GameObject newEnemy = (GameObject)Instantiate(staph, new Vector3(rnd.Next(xPosMin, xPosMax), rnd.Next(yPosMin, (int)(yPosMax)), 474f), Quaternion.identity);
                    newEnemy.GetComponent<CapsuleCollider>().enabled = true;
                    Allstaph.Add(newEnemy);
                }
                wave++;
                time = 0;
                print("COOLDOWN: " + cooldown);
            }

            cooldown = baseCoolDown * (float)((Mathf.Log(Allstaph.Count, 5) + 0.5) * (1 + wave * .1));

            if( Allstaph.Count == 0)
            {
                winUI.SetActive(true);
            }

        }
        //*/
        public void SpawnStaph()
        {
            for (int i = 0; i < numEnemyAtStart; i++)
            {
                GameObject newEnemy = (GameObject)Instantiate(staph, new Vector3(rnd.Next(xPosMin, xPosMax), rnd.Next(yPosMin, (int)(yPosMax)), 474f), Quaternion.identity);
                newEnemy.GetComponent<CapsuleCollider>().enabled = true;
                Allstaph.Add(newEnemy);
            }
        }

        public void removeEnemy(GameObject yeet)
        {
            Allstaph.Remove(yeet);
        }

        public List<GameObject> getStaphList()
        {
            return Allstaph;
        }

        public GameObject getStaphListElement(int i)
        {
            return Allstaph[i];
        }

        public void resetWave()
        {
            wave = 0;
        }
    }

}//namespace;

