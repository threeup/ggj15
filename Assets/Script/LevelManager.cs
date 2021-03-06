using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour 
{
	public static LevelManager Instance;

	public Dictionary<string, LevelData> storedLevelData = new Dictionary<string, LevelData>();
	private Level currentLevel;
	private LevelData currentLevelData;

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

	public void RegisterToLevel(GameEntity entity)
	{
		currentLevel.AddEntity(entity);
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

	public void SaveAndPurge()
	{
		currentLevel.SaveAndPurge(currentLevelData);
		currentLevel = null;

	}

	public void Trash(GameObject gameObject, bool hitByHuman)
	{
		gameObject.SetActive(false);
		GameEntity ge = gameObject.GetComponent<GameEntity>();
		if ( ge != null )
		{
			ge.edata.hitByHuman = hitByHuman;
			ge.edata.collisionState = CollisionState.DEAD;
		}
		if( currentLevel != null )
		{
			gameObject.name = "TRASH-"+gameObject.name;
			gameObject.transform.parent = currentLevel.gameObject.transform;
			gameObject.transform.position = gameObject.transform.position+999f*Vector3.up;
		}
		
	}

	

}
