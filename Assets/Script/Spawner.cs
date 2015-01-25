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
		GameObject.Instantiate(spawnPrefab);
		this.worldEntity.SendDeactivate();
	}
}
