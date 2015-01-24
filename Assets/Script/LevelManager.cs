using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour 
{

	public List<LevelData> levels = new List<LevelData>();
	private LevelData currentLevel;

	void Awake()
	{
		DontDestroyOnLoad(this);
	}

	public void LoadLevel(int index)
	{
		currentLevel = levels[index];
		Application.LoadLevelAdditive( "Level"+index );
		
	}
	
	void Update() 
	{
		
	}

	public void FinishLevel()
	{
		Debug.Log("Finished "+currentLevel);
	}
}
