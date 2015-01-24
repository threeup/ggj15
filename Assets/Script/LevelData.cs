using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelData : MonoBehaviour 
{


	public List<LevelElement> levelElements = new List<LevelElement>();

	public void Scan()
	{
		int mask = 1 << LayerMask.NameToLayer ("World");
		Collider[] colliders = Physics.OverlapSphere( Vector3.zero, 10000f, mask );
		foreach( Collider collider in colliders )
		{
			WorldEntity worldEntity = collider.GetComponent<WorldEntity>();
			if( worldEntity != null )
			{
				levelElements.Add( ConvertToLevelElement(worldEntity) );
			}
		}
		
	}

	public static LevelElement ConvertToLevelElement(WorldEntity e)
	{
		LevelElement element = new LevelElement();
		if( e.isMoving )
		{
			element.entityState = EntityState.MOVING;
		} 
		else if( e.isActive )
		{
			element.entityState = EntityState.ACTIVE;
		}
		else
		{
			element.entityState = EntityState.DISABLED;
		}
		element.entityType = e.entityType;
		element.position = new Vector2(e.transform.position.x, e.transform.position.z);
		return element;
	}
}
