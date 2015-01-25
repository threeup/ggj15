﻿using UnityEngine;
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
	public bool isInGame = false;

	public GameEntity mainCharacter;
	private int mainScore = 0;
	public int MainScore { get { return mainScore; } }
	private int mainStartHealth = 3;
	public int MainStartHealth { get { return mainStartHealth; } }


	private BasicTimer gameOverTimer = new BasicTimer(300f);

	public StoreProductChanger productChanger;

	void Awake()
	{
		Instance = this;
		DontDestroyOnLoad(this);
		uiMgr.HandleLevelSelect = LoadLevel;
		DOTween.Init(true, true, LogBehaviour.ErrorsOnly).SetCapacity(200, 10);

		Application.LoadLevelAdditive( "LevelSelect" );
	}

	void Start() 
	{
		NextScreen();
	}

	public void NextScreen()
	{
		if( !hasSeenIntro )
		{
			hasSeenIntro = true;
			Application.LoadLevelAdditive( "Cutscene" );
		}
		else
		{
			uiMgr.SetOverworldVisible(true);
			uiMgr.SetMenuVisible(true);
		}
		uiMgr.SetHudVisible(false);
		uiMgr.HudUpdate();
	}

	public void UnloadLevel()
	{	
		levelMgr.SaveAndPurge();
		uiMgr.SetOverworldVisible(true);
		uiMgr.SetMenuVisible(true);
		uiMgr.SetHudVisible(false);
		isInGame = false;
	}

	public void LoadLevel(int index)
	{
		isInGame = true;
		levelMgr.LoadLevel( index );
		uiMgr.SetOverworldVisible(false);
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

	public void ActorHitGoal(GameEntity hitActor)
	{
		if( this.mainCharacter == hitActor )
		{
			gameOverTimer.SetMin(0.5f);
		}
	}
	
	void Update() 
	{
		float deltaTime = Time.deltaTime;
		bool gameOver = isInGame && Input.GetButtonDown("Cancel");

		if(gameOver || gameOverTimer.Tick(deltaTime))
		{
			UnloadLevel();
		}
	}

	public void UnloadScene()
	{
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
