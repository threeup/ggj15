using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public GameEntity followTarget = null;
	private float leftEdge;
	private float rightEdge;
	private float speed = 3f;
	
	void Update () 
	{

		float deltaTime = Time.deltaTime;
		if( followTarget != null )
		{

			if( followTarget.transform.position.x > rightEdge )
			{
				this.transform.position = this.transform.position + Vector3.right*speed*deltaTime;
			}
			if( followTarget.transform.position.x < leftEdge )
			{
				this.transform.position = this.transform.position - Vector3.right*speed*deltaTime;
			}
		}
		leftEdge = this.transform.position.x - 5f;
		rightEdge = this.transform.position.x + 5f;
	}
}
