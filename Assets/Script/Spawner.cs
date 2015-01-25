using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	public GameObject spawnPrefab;
	public WorldEntity worldEntity;
	private bool canSpawn = true;
	public int spawnHealth = 1;
	public int spawnWalkVelocityMultiplier = 1;

	public void Awake()
	{
		worldEntity = GetComponent<WorldEntity>();
	}

	public void Activate()
	{
		canSpawn = false;
		if( worldEntity.edata.collisionState == CollisionState.ACTIVE )
		{
			canSpawn = true;
		}
		if( worldEntity.edata.collisionState == CollisionState.DISABL && 
			worldEntity.edata.childEntityType == EntityType.COINBRICK )
		{
			canSpawn = true;
		}
		if( worldEntity.edata.childEntityType == EntityType.MAINCHARACTER )
		{
			canSpawn = true;
		}
		if( canSpawn )
		{
			GameObject go = GameObject.Instantiate(spawnPrefab, this.transform.position, Quaternion.identity) as GameObject;	
			GameEntity ge = go.GetComponent<GameEntity>();
			this.worldEntity.AssociateSpawn(ge);
			ActorEntity actor = ge as ActorEntity;
			if( actor != null )
			{
				actor.SetupParameters(spawnHealth, spawnWalkVelocityMultiplier);
			}
			ge.SendActivate();
		}
		else
		{
			Debug.Log("cantspawn"+worldEntity.edata.collisionState +" "+worldEntity.edata.childEntityType);
		}
		this.worldEntity.Deactivate();

		
	}
}
