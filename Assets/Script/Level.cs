using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level : MonoBehaviour 
{
	public string LevelID;

	public List<WorldEntity> entities;

	void Awake()
	{
		if( LevelManager.Instance != null )
		{
			LevelManager.Instance.RegisterLevel(this);
		}
	}

	IEnumerator Start()
	{
		//wait for whole thing ot finish?
		yield return new WaitForSeconds(0.1f);
		if( LevelManager.Instance != null )
		{
			LevelManager.Instance.CheckLevelData(this);
		}
	}

	public void AddWorldEntity(WorldEntity entity)
	{
		entities.Add(entity);
	}

	public void EnableLevel()
	{

	}

	public void ApplyLevelData(LevelData levelData)
	{
		for( int i=entities.Count-1; i>=0; --i )
		{
			WorldEntity entity = entities[i];
			if( !levelData.entityDataMap.ContainsKey(entity.entityID) )
			{
				Debug.LogError("What is "+entity);
				continue;
			}
			entity.edata = levelData.entityDataMap[entity.entityID];
			entity.LoadFromData();

		}
	}
}
