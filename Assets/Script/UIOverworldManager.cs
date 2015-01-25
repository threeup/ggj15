using UnityEngine;
using System.Collections;

public class UIOverworldManager : MonoBehaviour {

	public LevelSelectPath path;
	public int desiredWaypoint = 0;

	public Canvas canvas;
	// Use this for initialization
	void Awake () {
		
		if( Director.Instance != null)
		{
			Director.Instance.uiMgr.overworldMgr = this;
			this.canvas.enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		if( path.go || !Director.Instance.isInLevelSelect)
		{
			//wait
			return;
		}
		float moveX = Input.GetAxis("Horizontal");
		if( moveX > 0.1f )
		{
			desiredWaypoint++;
		}
		else if (moveX < -0.1f )
		{
			desiredWaypoint--;	
		}
		desiredWaypoint = Mathf.Clamp(desiredWaypoint, 0, 8);
		if( path.targetWaypoint != desiredWaypoint )
		{
			path.targetWaypoint = desiredWaypoint;
			path.go = true;
		}
	}

	public int CurrentLevelNumber()
	{
		return path.levelNumbers[path.currentWaypoint];
	}
}
