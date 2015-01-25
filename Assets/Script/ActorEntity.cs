using UnityEngine;
using System.Collections;

public class ActorEntity : GameEntity 
{

	
	public override void Awake()
	{

		base.Awake();
		entityName = "ACTOR-"+this.gameObject.name+"_"+this.GetHashCode();
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
