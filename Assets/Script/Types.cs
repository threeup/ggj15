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

public enum CollisionState
{
	DISABL = 0,
	ACTIVE = 1,
	MOVING = 2,
}

[System.Serializable]
public struct EntityData
{
	public EntityType entityType;
	public CollisionState collisionState;
	public bool collidable;
	public bool drawable;
	public Vector2 position;
	
}
