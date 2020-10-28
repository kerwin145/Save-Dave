using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bacteria
{

    public class tCellTextBox : MonoBehaviour
    {
        public SpawnFamiliars SFScript;
        float tCellCost;
        public Text tCellText;

        // Start is called before the first frame update
        void Start()
        {
            tCellCost = SFScript.getTCellCost();

        }

        // Update is called once per frame
        void Update()
        {
            tCellText.text = "Spawn T Cell\n\nCost: " + tCellCost;


        }
    }

}