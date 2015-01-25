using UnityEngine;
using System.Collections;

public class Brain : MonoBehaviour {

	public float lastMoveX = 1f;
	public bool lastDoJump = false;
	
	public void DecideInput( out float moveX, out bool doJump )
	{
		moveX = lastMoveX;
		doJump = lastDoJump;
	}
}
