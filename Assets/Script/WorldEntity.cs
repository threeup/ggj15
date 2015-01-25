using UnityEngine;
using System.Collections;

public class WorldEntity : GameEntity {

	
	public override void Awake()
	{

		base.Awake();
		if( thisTransform.parent != null )
		{
			entityName = gameObject.name+"_"+this.GetHashCode();
		}
		else
		{
			entityName = "WORLD-"+this.gameObject.name+"_"+this.GetHashCode();
		}
		this.name = entityName;
	}



	public void OnCollisionEnter(Collision collision)
	{
		GameEntity other = collision.gameObject.GetComponent<GameEntity>();
		switch( edata.entityType )
		{
			default: break;
		}
		Debug.Log(this+"Hit"+other);
	}

}
