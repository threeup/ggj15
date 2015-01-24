using UnityEngine;
using System.Collections;

public class WorldEntity : MonoBehaviour {

	public string entityID = string.Empty;
	private Transform thisTransform;
	public EntityData edata;

	public bool IsDrawable { get { return edata.entityState != EntityState.NONE; } }

	void Awake()
	{
		thisTransform = this.transform;
		if( thisTransform.parent != null )
		{
			entityID = gameObject.name+"_"+this.GetHashCode();
		}
		else
		{
			entityID = "WORLD-"+this.gameObject.name+"_"+this.GetHashCode();
		}
		this.name = entityID;
		if( LevelManager.Instance != null && IsDrawable )
		{
			renderer.enabled = false;
		}
	}

	// Use this for initialization
	void Start () 
	{
		if( LevelManager.Instance != null )
		{
			LevelManager.Instance.RegisterToLevel(this);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void Deactivate()
	{
		if( IsDrawable )
		{
			renderer.enabled = false;
			collider2D.enabled = false;
		}
	}

	public void SaveToData()
	{
		edata.position = new Vector2(thisTransform.position.x, thisTransform.position.y);
		if( IsDrawable )
		{
			if( collider2D.enabled )
			{
				edata.entityState = EntityState.ACTIVE;
			}
			else
			{
				edata.entityState = EntityState.DISABL;
			}
		}
	}

	public void LoadFromData()
	{
		Vector3 position = new Vector3(edata.position.x, edata.position.y, 0f);
		thisTransform.position = position;

		switch(edata.entityState)
		{
			case EntityState.NONE: break;
			case EntityState.MOVING: renderer.enabled = true;  collider2D.enabled = true; break;
			case EntityState.ACTIVE: renderer.enabled = true;  collider2D.enabled = true; break;
			case EntityState.DISABL: renderer.enabled = false; collider2D.enabled = false; break;
		}
	}
}
