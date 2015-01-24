using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum EntityType
{
	CONTAINER = 0,
	BRICK = 1,
	COINBRICK = 2,
	ENEMY = 3,
	COIN = 4,
}

public enum EntityState
{
	NONE = 0,
	MOVING = 1,
	ACTIVE = 2,
	DISABL = 3,
}

[System.Serializable]
public struct EntityData
{
	public EntityType entityType;
	public EntityState entityState;
	public Vector2 position;
	
}
