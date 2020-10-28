using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bacteria { 

public class SpawnFamiliars : MonoBehaviour
{
public GameObject Monocyte;
public GameObject Neutrophil;
public GameObject Bcell;
public GameObject Cytokine;
public GameObject fluBCell;
public GameObject tCell;

public GameObject testVirus;

List<GameObject> MonocyteList;
List<GameObject> PhagocyteList; //this will include both monoctyte and neutrophil, for the flu level. 

public HealthBarManager HBM;

public StaphSpawner SSScript = null;
public VirusAndInfectedCellManager VICMScript = null;

//mana costs
float MonocyteCost = 1;
float NeutrophilCost = 1;
float BcellCost = 1;
float CytokineStormCost = 1;
float fluBCellCost = 1;
float tCellCost = 1;

public ManaBarManager MBM;

System.Random rnd = new System.Random();
int xPosMin, xPosMax, yPosMin, yPosMax;
public Canvas canvas;

public void Start() 
{
xPosMin = (int)(canvas.GetComponent<RectTransform>().rect.x * -1 - canvas.GetComponent<RectTransform>().rect.width/2);
xPosMax = (int)(canvas.GetComponent<RectTransform>().rect.x * -1 + canvas.GetComponent<RectTransform>().rect.width/2);
yPosMin = (int)(canvas.GetComponent<RectTransform>().rect.y * -1 - canvas.GetComponent<RectTransform>().rect.height/2 *.7);
yPosMax = (int)(canvas.GetComponent<RectTransform>().rect.y * -1 + canvas.GetComponent<RectTransform>().rect.height/2 * .9);
     
MonocyteList = new List<GameObject>();
PhagocyteList = new List<GameObject>(); ;
}

public void SpawnMonocyte(){
print("Monocyte Spawned");

if (MBM.getMana() >= MonocyteCost){
    MBM.useMana(MonocyteCost);
    GameObject newMonocyte = (GameObject)Instantiate(Monocyte, new Vector3 (rnd.Next(xPosMin,xPosMax), rnd.Next(yPosMin, (int)(yPosMax)), 474f), Quaternion.identity);
    MonocyteList.Add(newMonocyte);
    PhagocyteList.Add(newMonocyte);
    print("size of phagocyte list: " + PhagocyteList.Count);
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
        print("size of phagocyte list: " + PhagocyteList.Count);
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

public void SpawnTCell()
{

    print("TCell Spawned");

    if (MBM.getMana() >= tCellCost)
    {
        MBM.useMana(tCellCost);

        int TCellWidth = (int)Bcell.transform.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        int TCellHeight = (int)Bcell.transform.GetComponent<SpriteRenderer>().bounds.size.y / 2;

        GameObject newTCell = (GameObject)Instantiate(tCell, new Vector3(rnd.Next(xPosMin + TCellWidth, xPosMax - TCellWidth), rnd.Next(yPosMin + TCellHeight, (int)(yPosMax - TCellHeight)), 474f), Quaternion.identity);
    }

}

        public void spawnFluBCell()
{
    print("fluBCell Spawned");

    if (MBM.getMana() >= fluBCellCost)
    {
        MBM.useMana(fluBCellCost);

        int fluBCellWidth = (int)Bcell.transform.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        int fluBCellHeight = (int)Bcell.transform.GetComponent<SpriteRenderer>().bounds.size.y / 2;

        GameObject newFluBcell = (GameObject)Instantiate(fluBCell, new Vector3(rnd.Next(xPosMin + fluBCellWidth, xPosMax - fluBCellWidth), rnd.Next(yPosMin + fluBCellHeight, (int)(yPosMax - fluBCellHeight)), 474f), Quaternion.identity);
    }
    }


public void SpawnCytokineStorm()
{
    print("Cytokine Storm Spawned");

    if (MBM.getMana() >= CytokineStormCost)
    {
        MBM.useMana(CytokineStormCost);
        if (SSScript != null)
        {
            for (int i = 0; i < SSScript.getStaphList().Count * .75; i++)
            {
                GameObject newCytokine = (GameObject)Instantiate(Cytokine, new Vector3(rnd.Next(xPosMin, xPosMax), rnd.Next(yPosMin, (int)(yPosMax)), 474f), Quaternion.identity);
            }
            HBM.takeCytokineStormDamage();
        }
        else if (VICMScript != null)
        {
            for (int i = 0; i < VICMScript.getInfectedCellsList().Count * .75; i++)
            {
                GameObject newCytokine = (GameObject)Instantiate(Cytokine, new Vector3(rnd.Next(xPosMin, xPosMax), rnd.Next(yPosMin, (int)(yPosMax)), 474f), Quaternion.identity);
            }
            HBM.takeCytokineStormDamage();
            }
    }
}

public void spawnTestVirus()
{
    GameObject newVirus = (GameObject)Instantiate(testVirus, new Vector3(rnd.Next(xPosMin, xPosMax), rnd.Next(yPosMin, (int)(yPosMax)), 474f), Quaternion.identity);
        string outputString = "";
        print("SUMMARY OF PHAGOCYTE LIST: ");
        for(int i = 0; i < PhagocyteList.Count; i++)
        {
            outputString += PhagocyteList[i].name;
        }
        print(outputString);

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
    public float getNeutrophilCost()
    {
        return NeutrophilCost;
    }

    public float getMonocyteCost()
    {
        return MonocyteCost;
    }

    public float getBCellCost()
    {
        return BcellCost;
    }

    public float getCytokineStormCost()
    {
        return CytokineStormCost;
    }
        

    public float getTCellCost()
    {
        return tCellCost;
    }

}

}