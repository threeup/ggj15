using UnityEngine;
using System.Collections;

public class Brain : MonoBehaviour {

	public float lastMoveX = 1f;
	public bool lastDoJump = false;

	private BasicTimer changeDirTimer;
	
	public void DecideInput( out float moveX, out bool doJump )
	{
		moveX = lastMoveX;
		doJump = lastDoJump;
	}

	public void SideCollide(bool left)
	{
		if( changeDirTimer == null )
		{
			changeDirTimer = new BasicTimer(0.25f);
			lastMoveX = -lastMoveX;
		}
	}

	public void Update()
	{
		float deltaTime = Time.deltaTime;
		if( changeDirTimer != null && changeDirTimer.Tick(deltaTime) )
		{
			changeDirTimer = null;
		}
	}
}
