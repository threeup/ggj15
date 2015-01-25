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

	public UIOverworldManager overworldMgr = null;
	public UIStoreManager storeMgr = null;

	void Awake()
	{
		DontDestroyOnLoad(this);
		menuCanvas.gameObject.SetActive(false);
	}


	public void SetOverworldVisible(bool val)
	{
		overworldMgr.canvas.enabled = val;
	}


	public void SetStoreVisible(bool val)
	{
		storeMgr.canvas.enabled = val;
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
		coinCount.text = Director.Instance.MainScore+" COINS";
	}
}
