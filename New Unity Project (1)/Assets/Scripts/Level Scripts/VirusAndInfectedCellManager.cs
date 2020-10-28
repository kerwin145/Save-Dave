using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Bacteria
{

    public class VirusAndInfectedCellManager : MonoBehaviour
    {
        //creating a list of all infected cell objecs to keep track of it later on. 
        public GameObject infectedCell;
        public GameObject virus;
        List<GameObject> AllInfectedCells;
        List<GameObject> AllVirusAndInfectedCells;

        //used for initial postions upon instantiate.
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

            AllInfectedCells = new List<GameObject>();
            AllVirusAndInfectedCells = new List<GameObject>();

            xPosMin = (int)(canvas.GetComponent<RectTransform>().rect.x * -1 - canvas.GetComponent<RectTransform>().rect.width / 2);
            xPosMax = (int)(canvas.GetComponent<RectTransform>().rect.x * -1 + canvas.GetComponent<RectTransform>().rect.width / 2);
            yPosMin = (int)(canvas.GetComponent<RectTransform>().rect.y * -1 - canvas.GetComponent<RectTransform>().rect.height / 2 * .85);
            yPosMax = (int)(canvas.GetComponent<RectTransform>().rect.y * -1 + canvas.GetComponent<RectTransform>().rect.height / 2 * .85);

            SpawnInfectedCells(numEnemyAtStart);
            print("NumEnemy: " + AllInfectedCells.Count);
        }

        // Update is called once per frame
        void Update()
        {
           if(AllInfectedCells.Count == 0)
            {
                winUI.SetActive(true);
                for (int i = 0; i < AllVirusAndInfectedCells.Count; i++)
                {
                    GameObject tempGameObject = AllVirusAndInfectedCells[i];
                    AllVirusAndInfectedCells.RemoveAt(i);
                    Destroy(tempGameObject.gameObject);
                }
            }
        }
         public void SpawnInfectedCells(int num){
            for (int i = 0; i < num; i++)
            {
                GameObject newEnemy = Instantiate(infectedCell, new Vector3(rnd.Next(xPosMin, xPosMax), rnd.Next(yPosMin, (int)(yPosMax)), 474f), Quaternion.identity);
                newEnemy.GetComponent<SphereCollider>().enabled = true;
                AllInfectedCells.Add(newEnemy);
                AllVirusAndInfectedCells.Add(newEnemy);
            }
        }

        public void SpawnInfectedCells(float x, float y)
        {
            GameObject newEnemy = (GameObject)Instantiate(infectedCell, new Vector3(x, y, 474f), Quaternion.identity);
            newEnemy.GetComponent<SphereCollider>().enabled = true;
            AllInfectedCells.Add(newEnemy);
            AllVirusAndInfectedCells.Add(newEnemy);
        }

        public void spawnVirus(float x, float y)
        {
            GameObject newEnemy = (GameObject)Instantiate(virus, new Vector3(x, y, 474f), Quaternion.identity);
            newEnemy.GetComponent<CapsuleCollider>().enabled = true;
            AllVirusAndInfectedCells.Add(newEnemy);
        }

       

        public void addEnemytoICList(GameObject yeetnt) { AllInfectedCells.Add(yeetnt);}
        public void removeEnemyFromICList(GameObject yeet) {AllInfectedCells.Remove(yeet);}
        public List<GameObject> getInfectedCellsList(){return AllInfectedCells;}
        public GameObject getStaphListElement(int i){return AllInfectedCells[i];}

        public void addEnemytoVICList(GameObject yeetnt) { AllVirusAndInfectedCells.Add(yeetnt); }
        public void removeEnemyFromVICList(GameObject yeet){AllVirusAndInfectedCells.Remove(yeet);}
        public List<GameObject> getVICList(){return AllVirusAndInfectedCells;}
        public GameObject getVICListElement(int i){return AllVirusAndInfectedCells[i];}




    }

}//namespace;

