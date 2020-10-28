using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Bacteria { 

public class SpawnFamiliars : MonoBehaviour
{
public GameObject Monocyte;
public GameObject Neutrophil;
public GameObject Bcell;
public GameObject Cytokine;

List<GameObject> MonocyteList;
List<GameObject> PhagocyteList; //this will include both monoctyte and neutrophil, for the flu level. 

public HealthBarManager HBM;

GameObject StaphInfection;
public StaphSpawner SSScript;

    //mana costs
float MonocyteCost = 1;
float NeutrophilCost = 1;
float BcellCost = 1;
float CytokineStormCost = 50;

public ManaBarManager MBM;

System.Random rnd = new System.Random();
int xPosMin, xPosMax, yPosMin, yPosMax;
public Canvas canvas;

public void Start() {
    xPosMin = (int)(canvas.GetComponent<RectTransform>().rect.x * -1 - canvas.GetComponent<RectTransform>().rect.width/2);
    xPosMax = (int)(canvas.GetComponent<RectTransform>().rect.x * -1 + canvas.GetComponent<RectTransform>().rect.width/2);
    yPosMin = (int)(canvas.GetComponent<RectTransform>().rect.y * -1 - canvas.GetComponent<RectTransform>().rect.height/2 *.7);
    yPosMax = (int)(canvas.GetComponent<RectTransform>().rect.y * -1 + canvas.GetComponent<RectTransform>().rect.height/2 * .9);

    StaphInfection = GameObject.Find("StaphInfection");
    SSScript = StaphInfection.GetComponent<StaphSpawner>();

    MonocyteList = new List<GameObject>();
    }

public void SpawnMonocyte(){
    print("Monocyte Spawned");

    if (MBM.getMana() >= MonocyteCost){
        MBM.useMana(MonocyteCost);
        GameObject newMonocyte = (GameObject)Instantiate(Monocyte, new Vector3 (rnd.Next(xPosMin,xPosMax), rnd.Next(yPosMin, (int)(yPosMax)), 474f), Quaternion.identity);
        MonocyteList.Add(newMonocyte);
        PhagocyteList.Add(newMonocyte);
    }

}
    public void SpawnNeutrophil()
    {
        print("Neutrophil Spawned");

        if (MBM.getMana() >= NeutrophilCost)
        {
            MBM.useMana(NeutrophilCost);
            GameObject newNeutrophil = (GameObject)Instantiate(Neutrophil, new Vector3(rnd.Next(xPosMin, xPosMax), rnd.Next(yPosMin, (int)(yPosMax)), 474f), Quaternion.identity);
            PhagocyteList.Add(newNeutrophil);
            }

        }

    public void SpawnBcell(){

        print("Bcell Spawned");

        if (MBM.getMana() >= BcellCost){
            MBM.useMana(BcellCost);

            int BCellWidth = (int) Bcell.transform.GetComponent<SpriteRenderer>().bounds.size.x / 2;
            int BCellHeight = (int) Bcell.transform.GetComponent<SpriteRenderer>().bounds.size.y / 2;

            GameObject newBcell = (GameObject )Instantiate(Bcell, new Vector3 (rnd.Next(xPosMin + BCellWidth, xPosMax - BCellWidth), rnd.Next(yPosMin + BCellHeight, (int)(yPosMax - BCellHeight)), 474f), Quaternion.identity);
        }

    }

    public void SpawnCytokineStorm()
    {
        print("Cytokine Storm Spawned");

        if (MBM.getMana() >= CytokineStormCost)
        {
            MBM.useMana(CytokineStormCost);
            for (int i = 0; i < SSScript.getStaphList().Count * .75; i++)
            {
                GameObject newCytokine = (GameObject)Instantiate(Cytokine, new Vector3(rnd.Next(xPosMin, xPosMax), rnd.Next(yPosMin, (int)(yPosMax)), 474f), Quaternion.identity);
            }
            HBM.takeCytokineStormDamage();
        }
    }

        public int PlusMinus(){
        int Sign = rnd.Next(0,2);
        if (Sign== 0){ 
        
            return -1;
        }
        if (Sign== 1){ 
            
            return 1;
         }
        else {return 100;}
    }

 
    public void removeMonocyte(GameObject yeet){MonocyteList.Remove(yeet);}
    public List<GameObject> getMonocyteList(){return MonocyteList;}
    public GameObject getMonocyteListElement(int i){return MonocyteList[i];}

    public void removePhagocyte(GameObject yeet) {PhagocyteList.Remove(yeet);}
    public List<GameObject> getPhagocyteList() { return PhagocyteList; }
    public GameObject getPhagocyteListElement(int i) {return PhagocyteList[i];}

        public void setMonocyteTarget(GameObject staph)
        {
        //set that bacteria to be the one that is going to be tracked. 
        for (int i = 0; i < MonocyteList.Count; i++)
        {
            if (!MonocyteList[i].GetComponent<monocyte>().getHasTarget())
            {
                MonocyteList[i].GetComponent<monocyte>().setTargetedStaph(staph);
                i = MonocyteList.Count; //get out of loop
            }
        }
        }

    }
}