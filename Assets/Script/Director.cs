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

	public bool hasSeenIntro = false;
	public bool hasSeenTitle = false;
	public bool isInGame = false;
	public bool isInLevelSelect = false;
	public bool isInStore = false;

	public GameEntity mainCharacter;
	private int mainScore = 0;
	public int MainScore { get { return mainScore; } }
	private int mainStartHealth = 1;
	public int MainStartHealth { get { return mainStartHealth; } }


	private BasicTimer unloadSceneTimer = null;
	private BasicTimer gameOverTimer = new BasicTimer(300f);

	public StoreProductChanger productChanger;

	void Awake()
	{
		Instance = this;
		DontDestroyOnLoad(this);
		uiMgr.HandleLevelSelect = LoadLevel;
		DOTween.Init(true, true, LogBehaviour.ErrorsOnly).SetCapacity(200, 10);

		Application.LoadLevelAdditive( "LevelSelect" );
		Application.LoadLevelAdditive( "StoreScreen" );
	}

	void Start() 
	{
		uiMgr.SetMenuVisible(false);
		NextScreen();
	}

	public void NextScreen()
	{
		if( !hasSeenIntro )
		{
			hasSeenIntro = true;
			Application.LoadLevelAdditive( "Cutscene" );
			unloadSceneTimer = new BasicTimer(0.25f);
		}
		else if( !hasSeenTitle)
		{
			hasSeenTitle = true;
			Application.LoadLevelAdditive( "Title" );	
			unloadSceneTimer = new BasicTimer(0.25f);
		}
		else
		{
			isInLevelSelect = true;
			uiMgr.SetOverworldVisible(true);
			Debug.Log("next scene overowrld");
			//uiMgr.SetMenuVisible(true);
		}
		uiMgr.SetHudVisible(false);
		uiMgr.HudUpdate();
	}

	public void UnloadLevel()
	{	
		Debug.Log("UnloadLevel");
		levelMgr.SaveAndPurge();
		isInLevelSelect = true;
		uiMgr.SetOverworldVisible(true);
		//uiMgr.SetMenuVisible(true);
		uiMgr.SetHudVisible(false);
		camFollow.followTarget = null;
		camFollow.Center();
		
		isInGame = false;
	}

	public void LoadLevel(int index)
	{
		if( index == -1 )
		{
			return;
		}
		if( index == 0 )
		{
			isInLevelSelect = false;
			isInStore = true;
			uiMgr.SetOverworldVisible(false);
			uiMgr.SetStoreVisible(true);
			return;
		}
		uiMgr.SetStoreVisible(false);
		isInGame = true;
		levelMgr.LoadLevel( index );
		isInLevelSelect = false;
		uiMgr.SetOverworldVisible(false);
		//uiMgr.SetMenuVisible(false);
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

	public void ActorHitGoal(GameEntity hitActor)
	{
		if( this.mainCharacter == hitActor )
		{
			mainStartHealth++;
			gameOverTimer.SetMin(0.5f);
		}
	}
	
	void Update() 
	{
		float deltaTime = Time.deltaTime;
		bool gameOver = isInGame && Input.GetButtonDown("Cancel");
		bool levelSelect = isInLevelSelect && Input.GetButtonDown("Jump");
		if( levelSelect )
		{
			LoadLevel(uiMgr.overworldMgr.CurrentLevelNumber());
		}

		if(gameOver || gameOverTimer.Tick(deltaTime))
		{
			UnloadLevel();
		}

		if( unloadSceneTimer != null && unloadSceneTimer.Tick(deltaTime) )
		{
			unloadSceneTimer = null;	
		}
	}

	public void UnloadScene()
	{
		if( unloadSceneTimer != null )
		{
			return;
		}
		Application.LoadLevel( "Blank" );
		NextScreen();
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
