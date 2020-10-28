using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bacteria
{

    public class BCellTextBox : MonoBehaviour
    {
        public SpawnFamiliars SFscript;
        float BCellCost;
        public Text BCellText;

        // Start is called before the first frame update
        void Start()
        {
            BCellCost = SFscript.getBCellCost();
        }

        // Update is called once per frame
        void Update()
        {
            BCellText.text = "Spawn B Cell\n\nCost: " + BCellCost;
        }
    }
}