using UnityEngine;
using System.Collections;

public class Brain : MonoBehaviour {

	public float lastMoveX = 1f;
	public bool lastDoJump = false;
	public bool isMoveable = true;
	public bool isJumpable = false;

	private BasicTimer changeDirTimer;
	
	public void DecideInput( bool isGrounded, out float moveX, out bool doJump )
	{
		if( !isMoveable )
		{
			moveX = 0f;
			doJump = false;
			return;
		}
		moveX = lastMoveX;
		doJump = isJumpable && isGrounded;

		lastMoveX = moveX;
		lastDoJump = doJump;
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
