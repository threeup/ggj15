using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	public GameObject spawnPrefab;
	public WorldEntity worldEntity;

	public void Awake()
	{
		worldEntity = GetComponent<WorldEntity>();
	}

	public void Activate()
	{
		GameObject go = GameObject.Instantiate(spawnPrefab, this.transform.position, Quaternion.identity) as GameObject;	
		GameEntity ge = go.GetComponent<GameEntity>();
		this.worldEntity.AssociateSpawn(ge);
		this.worldEntity.Deactivate();

		ge.SendActivate();
	}
}
