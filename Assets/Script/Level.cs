using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level : MonoBehaviour 
{
	public string LevelID;

	IEnumerator Start()
	{
		//wait for whole thing ot finish?
		yield return new WaitForSeconds(0.1f);
		LevelManager.Instance.RegisterLevel(this);
	}

	public void EnableLevel()
	{

	}

	public void ApplyLevelData(LevelData levelData)
	{

	}
}
