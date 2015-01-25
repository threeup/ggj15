using UnityEngine;
using System.Collections;

public class UICutsceneManager : MonoBehaviour {


	
	void Update () 
	{
		bool gameOver = Input.GetButtonDown("Cancel");
		if( gameOver )
		{
			Director.Instance.UnloadScene();
			return;
		}
	}

}
