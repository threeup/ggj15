using UnityEngine;
using System.Collections;

public class UICutsceneManager : MonoBehaviour {

	public bool quitOnAnyPress = true;
	
	void Update () 
	{
		bool gameOver = Input.GetButtonDown("Cancel");
		if( quitOnAnyPress )
		{
			gameOver |= Input.GetButtonDown("Jump") || Input.GetButtonDown("Fire1");
		}
		if( gameOver )
		{
			Director.Instance.UnloadScene();
			return;
		}
	}

}
