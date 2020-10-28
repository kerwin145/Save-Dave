using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject StaphInfection;
    public int numEnemy; //we can try and make this random later. 

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numEnemy; i++){
            GameObject newEnemy = Instantiate(StaphInfection);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
