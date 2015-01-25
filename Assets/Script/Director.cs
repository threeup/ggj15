using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Director : MonoBehaviour 
{
	public static Director Instance;

	public UIManager uiMgr;
	public LevelManager levelMgr;
	public CameraFollow camFollow;
	public AudioClip introOverWorldCap;
	public AudioClip musicClip;
	public bool mainMusicPlaying = false;

	public GameEntity mainCharacter;
	private int mainScore = 0;
	public int MainScore { get { return mainScore; } }
	private int mainStartHealth = 3;
	public int MainStartHealth { get { return mainStartHealth; } }

	public StoreProductChanger productChanger;

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

	public void SetProgress(int i, bool canBuy)
	{
		if(!canBuy)
		{
			productChanger.currentState = StoreProductChanger.StoreStates.noProduct;
			return;
		}
		switch(i)
		{
			case 0: productChanger.currentState = StoreProductChanger.StoreStates.intro; break;
			case 1: productChanger.currentState = StoreProductChanger.StoreStates.shell; break;
			case 2: productChanger.currentState = StoreProductChanger.StoreStates.wings; break;
			case 3: productChanger.currentState = StoreProductChanger.StoreStates.finished; break;
		}
		
	}
}
