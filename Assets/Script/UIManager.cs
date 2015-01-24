using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour 
{

	public delegate void UIMethod(int index);

	public UIMethod HandleStartButton;

	private bool inputEnabled;

	void Awake()
	{
		DontDestroyOnLoad(this);
	}

	public void PlayFirstScreen()
	{
		inputEnabled = true;
	}
	
	void Update() 
	{
		if( inputEnabled )
		{
			if( Input.GetButtonDown ("Fire1")) 
			{
				inputEnabled = false;
				HandleStartButton(0);
			}
		} 
	}
}
