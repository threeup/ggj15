using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Director : MonoBehaviour 
{
	public static Director Instance;

	public UIManager uiMgr;
	public LevelManager levelMgr;
	public CameraFollow camFollow;
	public AudioClip musicClip;
	public bool mainMusicPlaying = false;

	public GameEntity mainCharacter;
	private int mainScore = 0;
	public int MainScore { get { return mainScore; } }
	private int mainStartHealth = 3;
	public int MainStartHealth { get { return mainStartHealth; } }

	void Awake()
	{
		Instance = this;
		DontDestroyOnLoad(this);
		uiMgr.HandleLevelSelect = LoadLevel;
		DOTween.Init(true, true, LogBehaviour.Verbose).SetCapacity(200, 10);
	}

	void Start() 
	{
		uiMgr.SetMenuVisible(true);
		uiMgr.SetHudVisible(false);
		uiMgr.HudUpdate();
	}

	public void UnloadLevel()
	{	
		levelMgr.SaveAndPurge();
		uiMgr.SetMenuVisible(true);
		uiMgr.SetHudVisible(false);
	}

	public void LoadLevel(int index)
	{
		levelMgr.LoadLevel( index );
		uiMgr.SetMenuVisible(false);
		uiMgr.SetHudVisible(true);
		uiMgr.HudUpdate();
		if( !mainMusicPlaying )
		{

			audio.clip = musicClip;
			audio.Play();
			mainMusicPlaying = true;
		}
	}

	public void SetAction(bool val, GameEntity mainCharacter)
	{
		if( val )
		{
			this.mainCharacter = mainCharacter;
			camFollow.followTarget = mainCharacter;
			camFollow.Center();
		}
	}
	
	void Update() 
	{
		
	}

	public void AddCoin(GameEntity actor, int amount)
	{
		if( actor == mainCharacter )
		{
			mainScore += amount;

			uiMgr.HudUpdate();
		}
	}
}
