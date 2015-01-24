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
		if( storedLevelData.ContainsKey(level.LevelID) )
		{
			currentLevelData = storedLevelData[level.LevelID];
			level.ApplyLevelData(currentLevelData);
		}
		else
		{
			GameObject go = new GameObject(level.LevelID+"Data");
			go.transform.parent = this.transform;
			currentLevelData = go.AddComponent<LevelData>();
			currentLevelData.levelElements = ScanLevelElements();
			storedLevelData.Add(level.LevelID, currentLevelData);
		}
	}

	
	void Update() 
	{
		
	}

	public void FinishLevel()
	{
		Debug.Log("Finished "+currentLevel);
	}

	public List<LevelElement> ScanLevelElements()
	{
		List<LevelElement> levelElements = new List<LevelElement>();
		int worldmask = 1 << LayerMask.NameToLayer ("World");
		int actormask = 1 << LayerMask.NameToLayer ("Actor");
		int mask = worldmask | actormask;

		Collider2D[] colliders = Physics2D.OverlapCircleAll(Vector3.zero, 10000f, mask);
		foreach( Collider2D collider in colliders )
		{
			WorldEntity worldEntity = collider.GetComponent<WorldEntity>();
			if( worldEntity != null )
			{
				levelElements.Add( ConvertToLevelElement(worldEntity) );
			}
		}
		return levelElements;
	}

	public static LevelElement ConvertToLevelElement(WorldEntity e)
	{
		LevelElement element = new LevelElement();
		if( e.isMoving )
		{
			element.entityState = EntityState.MOVING;
		} 
		else if( e.isActive )
		{
			element.entityState = EntityState.ACTIVE;
		}
		else
		{
			element.entityState = EntityState.DISABLED;
		}
		element.entityType = e.entityType;
		element.position = new Vector2(e.transform.position.x, e.transform.position.z);
		return element;
	}
}
