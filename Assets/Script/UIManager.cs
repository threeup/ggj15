using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour 
{

	public delegate void UIMethod(int index);

	public UIMethod HandleLevelSelect;

	private bool inputEnabled;

	public Canvas menuCanvas;

	void Awake()
	{
		DontDestroyOnLoad(this);
		menuCanvas.gameObject.SetActive(false);
	}

	public void SetMenuVisible(bool val)
	{
		menuCanvas.gameObject.SetActive(val);
	}
	
	public void LevelSelect(int index)
	{
		HandleLevelSelect(index);
	}
}
