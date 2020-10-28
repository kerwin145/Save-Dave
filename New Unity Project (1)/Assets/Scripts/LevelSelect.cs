using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
	public void TitleScreen()
	{
		SceneManager.LoadScene(0);
	}
	public void Level1(){
		  SceneManager.LoadScene(3);
	 }
	public void Level2()
	{
		SceneManager.LoadScene(4);
	}

}
