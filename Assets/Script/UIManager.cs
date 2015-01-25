using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour 
{

	public delegate void UIMethod(int index);

	public UIMethod HandleLevelSelect;

	private bool inputEnabled;

	public Canvas menuCanvas;
	public Canvas hudCanvas;
	public Text coinCount;

	void Awake()
	{
		DontDestroyOnLoad(this);
		menuCanvas.gameObject.SetActive(false);
	}

	public void SetMenuVisible(bool val)
	{
		menuCanvas.gameObject.SetActive(val);
	}

	public void SetHudVisible(bool val)
	{
		hudCanvas.gameObject.SetActive(val);
	}
	
	public void LevelSelect(int index)
	{
		HandleLevelSelect(index);
	}

	public void HudUpdate()
	{
		coinCount.text = Director.Instance.mainScore+" COINS";
	}
}
