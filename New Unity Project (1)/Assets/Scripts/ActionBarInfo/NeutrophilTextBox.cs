using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bacteria
{

    public class NeutrophilTextBox : MonoBehaviour
    {
        public SpawnFamiliars SFscript;
        float  NeutrophilCost;
        public Text NeutrophilText;

        // Start is called before the first frame update
        void Start()
        {
            NeutrophilCost = SFscript.getNeutrophilCost();
        }

        // Update is called once per frame
        void Update()
        {
            NeutrophilText.text = "Spawn Neutrophil\n\nCost: " + NeutrophilCost;
        }
    }
}