using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Director : MonoBehaviour 
{

	public enum DirectorState
	{
		CUTSCENE,
		INTRO,
		OVERWORLD,
		STORE,
		GAME,

	}
	public static Director Instance;

	public UIManager uiMgr;
	public LevelManager levelMgr;
	public CameraFollow camFollow;
	public AudioClip introOverWorldClip;
	public AudioClip musicClip;
	public bool introMusicPlaying = false;
	public bool mainMusicPlaying = false;

	public bool hasSeenIntro = false;
	public bool hasSeenTitle = false;

	public DirectorState dState = DirectorState.CUTSCENE;

	public ActorEntity mainCharacter;
	private int mainScore = 0;
	public int MainScore { get { return mainScore; } }
	private int mainStartHealth = 1;
	public int MainStartHealth { get { return mainStartHealth; } }
	private int mainUpgradeCost = 5;
	public int MainUpgradeCost { get { return mainUpgradeCost; } }


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
			SetState(DirectorState.INTRO);
			Application.LoadLevelAdditive( "Title" );	
			unloadSceneTimer = new BasicTimer(0.25f);
		}
		else
		{
			SetState(DirectorState.OVERWORLD);
			uiMgr.SetOverworldVisible(true);
			Debug.Log("next scene overowrld");
			//uiMgr.SetMenuVisible(true);
		}
		uiMgr.SetHudVisible(false);
		uiMgr.HudUpdate();
	}

	public void SetState(DirectorState dState)
	{
		if( this.dState == dState )
		{
			return;
		}
		this.dState = dState;
		if( this.dState == DirectorState.GAME )
		{
			if( !mainMusicPlaying )
			{
				introMusicPlaying = false;
				mainMusicPlaying = true;
				audio.clip = musicClip;
				audio.Play();
			}
		}
		else
		{
			if( !introMusicPlaying )
			{
				introMusicPlaying = true;
				mainMusicPlaying = false;
				audio.clip = introOverWorldClip;
				audio.Play();
			}
		}

		if( this.dState == DirectorState.STORE )
		{
			uiMgr.storeMgr.BeginScreen();
		}
	}

	public void UnloadLevel()
	{	
		Debug.Log("UnloadLevel");
		levelMgr.SaveAndPurge();
		uiMgr.SetOverworldVisible(true);
		//uiMgr.SetMenuVisible(true);
		uiMgr.SetHudVisible(false);
		camFollow.followTarget = null;
		camFollow.Center();
		
		SetState(DirectorState.OVERWORLD);
	}

	public void LoadLevel(int index)
	{
		if( index == -1 )
		{
			return;
		}
		if( index == 0 )
		{
			SetState(DirectorState.STORE);

			uiMgr.SetOverworldVisible(false);
			uiMgr.SetStoreVisible(true);
			uiMgr.SetHudVisible(true);
			return;
		}
		uiMgr.SetStoreVisible(false);
		SetState(DirectorState.GAME);
		levelMgr.LoadLevel( index );
		
		uiMgr.SetOverworldVisible(false);
		//uiMgr.SetMenuVisible(false);
		uiMgr.SetHudVisible(true);
		uiMgr.HudUpdate();
	}

	public void SetAction(bool val, ActorEntity mainCharacter)
	{
		if( val )
		{
			this.mainCharacter = mainCharacter;
			camFollow.followTarget = mainCharacter;
			camFollow.Center();
		}
	}

	public bool ActorHitGoal(ActorEntity hitActor)
	{
		if( this.mainCharacter == hitActor )
		{
			gameOverTimer.SetMin(0.5f);
			FinishLevel();
			return true;
		}
		return false;
	}

	public void ActorDied(ActorEntity deadActor)
	{
		if( this.mainCharacter == deadActor )
		{
			gameOverTimer.SetMin(0.5f);
		}
	}
	
	void Update() 
	{
		float deltaTime = Time.deltaTime;
		bool gameOver = dState == DirectorState.GAME && Input.GetButtonDown("Cancel");
		bool levelSelect = dState == DirectorState.OVERWORLD && Input.GetButtonDown("Jump");
		if( levelSelect )
		{
			LoadLevel(uiMgr.overworldMgr.CurrentLevelNumber());
		}

		bool cheat = dState == DirectorState.GAME && Input.GetButtonDown("Fire3");
		if( cheat )
		{
			IncreaseStartHealth();
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

	public bool PurchaseProgress(int coinCost)
	{
		if( mainScore > coinCost )
		{
			mainScore -= coinCost;
			IncreaseStartHealth();
			return true;
		}
		return false;
	}

	public void FinishLevel()
	{
		SetProgress(mainStartHealth, true);
	}

	public void IncreaseStartHealth()
	{

		mainStartHealth++;
		if( mainCharacter != null )
		{
			mainCharacter.SetMaxHealth(mainStartHealth);
		}
	
		uiMgr.HudUpdate();
	}

	public void SetProgress(int i, bool canBuy)
	{
		if(!canBuy)
		{
			productChanger.currentState = StoreProductChanger.StoreStates.noProduct;
			
		}
		else
		{
			switch(i)
			{
				case 0: productChanger.currentState = StoreProductChanger.StoreStates.intro; break;
				case 1: productChanger.currentState = StoreProductChanger.StoreStates.shell; break;
				case 2: productChanger.currentState = StoreProductChanger.StoreStates.wings; break;
				case 3: productChanger.currentState = StoreProductChanger.StoreStates.finished; break;
			}
		}
		uiMgr.storeMgr.Refresh();
		productChanger.updateProduct();
		Debug.Log("set progress"+i+" ="+productChanger.currentState);
		
	}
}
