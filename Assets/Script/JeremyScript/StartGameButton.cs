using UnityEngine;
using System.Collections;

public class StartGameButton : MonoBehaviour {


	public void StartGameClick()
	{
		Application.LoadLevel("LevelSelect");
	}
}
