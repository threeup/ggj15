using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum EntityType
{
	BRICK,
	ENEMY,
	COIN,
}

public enum EntityState
{
	MOVING,
	ACTIVE,
	DISABLED,
}

public struct LevelElement
{
	public EntityType entityType;
	public EntityState entityState;
	public Vector2 position;
	
}
