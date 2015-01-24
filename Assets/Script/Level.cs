using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level : MonoBehaviour 
{
	public string LevelID;
	public int levelUID;

	private int nextUID;

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

	public void ResetUID()
	{
		nextUID = 0;
	}

	public void AddWorldEntity(WorldEntity entity)
	{
		entities.Add(entity);
	}

	public void Purge()
	{
		for( int i=entities.Count-1; i>=0; --i )
		{
			entities[i].Destroy();
		}
		entities.Clear();

		Destroy(gameObject);

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

	public static Dictionary<int, EntityData> ScanGatherData()
	{
		Dictionary<int, EntityData>  entityDataMap = new Dictionary<int, EntityData>();
		int worldmask = 1 << LayerMask.NameToLayer ("World");
		int actormask = 1 << LayerMask.NameToLayer ("Actor");
		int mask = worldmask | actormask;

		//Collider2D[] colliders = Physics2D.OverlapCircleAll(Vector3.zero, 10000f, mask);

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

	public void ScanAssignUID()
	{
		int worldmask = 1 << LayerMask.NameToLayer ("World");
		int actormask = 1 << LayerMask.NameToLayer ("Actor");
		int mask = worldmask | actormask;

		WorldEntity[] entities = FindObjectsOfType<WorldEntity>();
		foreach(WorldEntity worldEntity in entities)
		{
			if( (worldEntity.gameObject.layer | mask) == 0 )
			{
				//what is this?
				continue;
			}
			worldEntity.entityID = this.levelUID*1000 + this.nextUID;
			this.nextUID++;
		}
	}
}
