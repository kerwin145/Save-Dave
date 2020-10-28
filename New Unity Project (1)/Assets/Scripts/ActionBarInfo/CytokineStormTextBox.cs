using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bacteria
{

    public class CytokineStormTextBox : MonoBehaviour
    {
        public SpawnFamiliars SFscript;
        float CytokineStormCost;
        public Text CytokineStormText;

        // Start is called before the first frame update
        void Start()
        {
            CytokineStormCost = SFscript.getCytokineStormCost();
        }

        // Update is called once per frame
        void Update()
        {
            CytokineStormText.text = "Spawn Cytokine Storm\n\nCost: " + CytokineStormCost;
        }
    }
}