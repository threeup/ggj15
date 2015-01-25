using UnityEngine;
using System.Collections;

public class GameEntity : MonoBehaviour {

	public int entityID = -1;
	public string entityName = string.Empty;
	protected Transform thisTransform;
	public EntityData edata;


	public virtual void Awake()
	{
		thisTransform = this.transform;
		edata.drawable = this.renderer != null;
		edata.collidable = this.collider != null;
		if( LevelManager.Instance != null && edata.drawable )
		{
			//default everything to off when we start, level data decides if it is visible or not
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

	public void Destroy()
	{
		Destroy(gameObject);
	}

	public void SendActivate()
	{
		gameObject.SendMessage("Activate");
	}

	public void SendDeactivate()
	{
		gameObject.SendMessage("Deactivate");
	}

	public void Activate()
	{
		
	}

	public void Deactivate()
	{
		if( edata.drawable )
		{
			renderer.enabled = false;
		}
		if( edata.collidable )
		{
			collider2D.enabled = false;
		}
	}

	public void SaveToData()
	{
		edata.position = thisTransform.position;
	}

	public void LoadFromData()
	{
		thisTransform.position = edata.position;

		if( edata.drawable )
		{
			renderer.enabled = true;
		}
		if( edata.collidable )
		{  
			switch(edata.collisionState)
			{
				case CollisionState.MOVING: collider2D.enabled = true; break;
				case CollisionState.ACTIVE: collider2D.enabled = true; break;
				case CollisionState.DISABL: collider2D.enabled = false; break;
			}
		}
	}

	
}
