using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;

namespace Bacteria
{
    public class pauseButton : MonoBehaviour
    {
        // Start is called before the first frame update

        public GameObject pauseUI;
        public GameObject deathUI;

        void Update()
        {
            if (pauseUI.activeSelf)
                Time.timeScale = 0;
            else
                Time.timeScale = 1;

        }
        public void pauseButtonPressed()
        {
            if (deathUI.activeSelf == false){pauseUI.SetActive(true);}
        }

        public void resumeButtonPressed()
        {
            pauseUI.SetActive(false);
        }

       
    }

}