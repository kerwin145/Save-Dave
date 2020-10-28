using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Bacteria { 

public class TryAgain : MonoBehaviour
{
    HealthBarManager HBM;
    ManaBarManager MBM;
    StaphSpawner staph;
    public GameObject deathUI;
    string sceneName;


        // Start is called before the first frame update
        void Start()
    {
        HBM = GameObject.Find("HealthBar").GetComponent<HealthBarManager>();
        MBM = GameObject.Find("Mana").GetComponent<ManaBarManager>();
        try
        {
            staph = GameObject.Find("StaphInfection").GetComponent<StaphSpawner>();
        }   catch { print("PPPOopoo"); }

    }

    public void Restart()
    {
        sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }
    
}
}
