using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour 
{
	public static LevelManager Instance;

	public Dictionary<string, LevelData> storedLevelData = new Dictionary<string, LevelData>();
	private Level currentLevel;
	private LevelData currentLevelData;
	private BasicTimer gameOverTimer = new BasicTimer(300f);

	void Awake()
	{
		DontDestroyOnLoad(this);
		Instance = this;
	}

	public void LoadLevel(int index)
	{
		Application.LoadLevelAdditive( "Level"+index );
		
	}

	public void RegisterLevel(Level level)
	{
		currentLevel = level;
	}

	public void RegisterToLevel(WorldEntity entity)
	{
		currentLevel.AddWorldEntity(entity);
	}

	public void CheckLevelData(Level level)
	{
		if( storedLevelData.ContainsKey(level.LevelID) )
		{
			currentLevelData = storedLevelData[level.LevelID];
		}
		else
		{
			GameObject go = new GameObject(level.LevelID+"Data");
			go.transform.parent = this.transform;
			currentLevelData = go.AddComponent<LevelData>();
			currentLevelData.entityDataMap = Level.ScanGatherData();
			storedLevelData.Add(level.LevelID, currentLevelData);
		}
		level.ApplyLevelData(currentLevelData);
	}

	
	void Update() 
	{
		float deltaTime = Time.deltaTime;
		if(gameOverTimer.Tick(deltaTime))
		{
			Director.Instance.UnloadLevel();
		}
	}

	public void Purge()
	{
		currentLevel.Purge();
		currentLevel = null;

	}

	

}
