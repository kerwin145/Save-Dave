using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
 public void AlmanacScreen(){
	  SceneManager.LoadScene(1);
 }

 public void LevelSelectScreen(){
	  SceneManager.LoadScene(2);
 }


 public void QuitGame(){
	  Debug.Log("QUIT!");
	  Application.Quit();
 }

public void MainMenuScreen()
	{
		Time.timeScale = 1;
		SceneManager.LoadScene(0);
	}

public void CovidReleifButton()
    {
		Application.OpenURL("https://www.newsdaycharities.org/");
    }

}
