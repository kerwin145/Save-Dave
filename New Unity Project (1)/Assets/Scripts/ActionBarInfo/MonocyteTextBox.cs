using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bacteria
{

    public class MonocyteTextBox : MonoBehaviour
    {
        public SpawnFamiliars SFscript;
        float MonocyteCost;
        public Text MonocyteText;

        // Start is called before the first frame update
        void Start()
        {
            MonocyteCost = SFscript.getMonocyteCost();
        }

        // Update is called once per frame
        void Update()
        {
            MonocyteText.text = "Spawn Monocyte\n\nCost: " + MonocyteCost;
        }
    }
}