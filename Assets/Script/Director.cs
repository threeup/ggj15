using UnityEngine;
using System.Collections;

public class Director : MonoBehaviour 
{
	public static Director Instance;

	public UIManager uiMgr;
	public LevelManager levelMgr;
	public CameraFollow camFollow;

	void Awake()
	{
		Instance = this;
		DontDestroyOnLoad(this);
		uiMgr.HandleLevelSelect = LoadLevel;
	}

	void Start() 
	{
		uiMgr.SetMenuVisible(true);
	}

	public void UnloadLevel()
	{	
		levelMgr.Purge();
		uiMgr.SetMenuVisible(true);
	}

	public void LoadLevel(int index)
	{
		levelMgr.LoadLevel( index );
		uiMgr.SetMenuVisible(false);
	}

	public void SetAction(bool val, GameEntity primaryEntity)
	{
		if( val )
		{
			camFollow.followTarget = primaryEntity;
		}
	}
	
	void Update() 
	{
		
	}
}
