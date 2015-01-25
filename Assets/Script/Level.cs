using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;

public class Level : MonoBehaviour 
{
	public string LevelID;
	public int levelUID;

	private int nextUID;

	public List<GameEntity> entities;
	public GameEntity mainCharacterEntity;

	public Vector3 startCamera;

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
			yield return new WaitForSeconds(0.1f);
			Director.Instance.SetAction(true, mainCharacterEntity);
		}
		else
		{
			Application.LoadLevel( "startscene" );
		}
	}

	public void ResetUID()
	{
		nextUID = 0;
	}

	public void AddEntity(GameEntity entity)
	{
		if( entity.edata.entityType == EntityType.DONTSAVE )
		{
			return;
		}
		entities.Add(entity);
		if( entity.edata.entityType == EntityType.MAINCHARACTER )
		{
			mainCharacterEntity = entity;	
		}
	}
	public void SaveAndPurge(LevelData levelData)
	{
		StartCoroutine(SaveAndPurgeRoutine(levelData));
	}

	IEnumerator SaveAndPurgeRoutine(LevelData levelData)
	{
		for( int i=entities.Count-1; i>=0; --i )
		{
			GameEntity entity = entities[i];
			entity.SaveToData();
			if( levelData.entityDataMap.ContainsKey(entity.entityID) )
			{
				levelData.entityDataMap[entity.entityID] = entity.edata;
			}
			else
			{
				Debug.Log("didnt save"+entity);
			}
			entity.gameObject.SetActive(false);
		}
		Debug.LogWarning("Finish Save");
		yield return new WaitForSeconds(0.5f);
		for( int i=entities.Count-1; i>=0; --i )
		{
			entities[i].FinishDestroy();
		}
		entities.Clear();
		Debug.LogWarning("Finish Purge");
		Destroy(gameObject);

	}

	public void ApplyLevelData(LevelData levelData)
	{
		for( int i=entities.Count-1; i>=0; --i )
		{
			GameEntity entity = entities[i];
			if( !levelData.entityDataMap.ContainsKey(entity.entityID) )
			{
				Debug.LogError("What is "+entity);
				continue;
			}
			entity.edata = levelData.entityDataMap[entity.entityID];
			entity.LoadFromData();
			

		}
		for( int i=0; i<entities.Count; ++i )
		{
			GameEntity entity = entities[i];
			entity.SendActivate();			
		}
	}

	public static Dictionary<int, EntityData> ScanGatherData()
	{
		Dictionary<int, EntityData>  entityDataMap = new Dictionary<int, EntityData>();
		int worldmask = 1 << LayerMask.NameToLayer ("World");
		int actormask = 1 << LayerMask.NameToLayer ("Actor");
		int mask = worldmask | actormask;

		//Collider2D[] colliders = Physics2D.OverlapCircleAll(Vector3.zero, 10000f, mask);

		GameEntity[] scannedEntities = FindObjectsOfType<GameEntity>();
		foreach(GameEntity gameEntity in scannedEntities)
		{
			if( (gameEntity.gameObject.layer | mask) == 0 )
			{
				//what is this?
				Debug.Log("what is "+gameEntity);
				continue;
			}
			gameEntity.SaveToData();
			gameEntity.edata.collisionState = CollisionState.ACTIVE;
			entityDataMap.Add( gameEntity.entityID, gameEntity.edata);
			gameEntity.Deactivate();
		}
		return entityDataMap;
	}

	public void ScanAssignUID()
	{
		int worldmask = 1 << LayerMask.NameToLayer ("World");
		int actormask = 1 << LayerMask.NameToLayer ("Actor");
		int mask = worldmask | actormask;

		GameEntity[] scannedEntities = FindObjectsOfType<GameEntity>();
		foreach(GameEntity gameEntity in scannedEntities)
		{
			if( (gameEntity.gameObject.layer | mask) == 0 )
			{
				//what is this?
				continue;
			}
#if UNITY_EDITOR
			PrefabUtility.DisconnectPrefabInstance(gameEntity.gameObject);
			gameEntity.gameObject.name = gameEntity.gameObject.name.Replace("Prefab", ""); 
#endif			
			gameEntity.entityID = this.levelUID*1000 + this.nextUID;
			this.nextUID++;
		}
	}	

	public void ScanAlign()
	{
		int worldmask = 1 << LayerMask.NameToLayer ("World");
		int actormask = 1 << LayerMask.NameToLayer ("Actor");
		int mask = worldmask | actormask;

		GameEntity[] scannedEntities = FindObjectsOfType<GameEntity>();
		foreach(GameEntity gameEntity in scannedEntities)
		{
			if( (gameEntity.gameObject.layer | mask) == 0 )
			{
				//what is this?
				continue;
			}
			if( !gameEntity.snapToBounds )
			{
				continue;
			}
			Vector3 pos = gameEntity.transform.position;
			float snapTo = 1f;
			pos.x = Mathf.Round(pos.x*snapTo)/snapTo;
			pos.y = Mathf.Round(pos.y*snapTo)/snapTo;
			pos.z = Mathf.Round(pos.z*snapTo)/snapTo;
			gameEntity.transform.position = pos;
		}
	}
}
