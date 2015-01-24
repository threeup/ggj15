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
			currentLevelData.entityDataMap = ScanLevelElements();
			storedLevelData.Add(level.LevelID, currentLevelData);
		}
		level.ApplyLevelData(currentLevelData);
	}

	
	void Update() 
	{
		
	}

	public void FinishLevel()
	{
		Debug.Log("Finished "+currentLevel);

	}

	public Dictionary<string, EntityData> ScanLevelElements()
	{
		Dictionary<string, EntityData>  entityDataMap = new Dictionary<string, EntityData>();
		int worldmask = 1 << LayerMask.NameToLayer ("World");
		int actormask = 1 << LayerMask.NameToLayer ("Actor");
		int mask = worldmask | actormask;

		/*Collider2D[] colliders = Physics2D.OverlapCircleAll(Vector3.zero, 10000f, mask);
		foreach( Collider2D collider2D in colliders )
		{
			WorldEntity worldEntity = collider2D.GetComponent<WorldEntity>();
			if( worldEntity != null )
			{
				worldEntity.SaveToData();
				entityDataMap.Add( worldEntity.entityID, worldEntity.edata);
			}
			collider2D.enabled = false;
		}*/
		WorldEntity[] entities = FindObjectsOfType<WorldEntity>();
		foreach(WorldEntity worldEntity in entities)
		{
			if( (worldEntity.gameObject.layer | mask) == 0 )
			{
				//what is this?
				continue;
			}
			if( worldEntity != null )
			{
				worldEntity.SaveToData();
				entityDataMap.Add( worldEntity.entityID, worldEntity.edata);
				worldEntity.Deactivate();
			}
		}
		return entityDataMap;
	}

}
