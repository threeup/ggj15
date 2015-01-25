using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public GameEntity followTarget = null;
	public Bounds bounds;
	public float rightEdge;
	public float speed = 3f;

	void Awake()
	{
		bounds = new Bounds(Vector3.zero, new Vector3(1f,1f));
	}
	
	void Update () 
	{
		float deltaTime = Time.deltaTime;
		if( followTarget != null )
		{

			if( followTarget.transform.position.x > bounds.max.x )
			{
				this.transform.position = this.transform.position + Vector3.right*speed*deltaTime;
			}
			if( followTarget.transform.position.x < bounds.min.x )
			{
				this.transform.position = this.transform.position - Vector3.right*speed*deltaTime;
			}
			if( followTarget.transform.position.y > bounds.max.y )
			{
				this.transform.position = this.transform.position + Vector3.up*speed*deltaTime;
			}
			if( followTarget.transform.position.y < bounds.min.y )
			{
				this.transform.position = this.transform.position - Vector3.up*speed*deltaTime;
			}
		}
		bounds.center = this.transform.position;
	}
	public void Center()
	{
		Vector3 start = this.transform.position;
		start.x = followTarget.transform.position.x;
		start.y = followTarget.transform.position.y;
		this.transform.position = start;
	}
}
