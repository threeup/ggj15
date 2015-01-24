using UnityEngine;
using System.Collections;

public class Director : MonoBehaviour 
{


	public UIManager uiMgr;
	public LevelManager levelMgr;

	void Awake()
	{
		DontDestroyOnLoad(this);
	}

	void Start() 
	{
		uiMgr.PlayFirstScreen();
		uiMgr.HandleStartButton = LoadLevel;
	}

	public void LoadLevel(int index)
	{
		levelMgr.LoadLevel( index );
	}
	
	void Update() 
	{
		
	}
}
