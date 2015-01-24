using UnityEngine;
using System.Collections;

public class TitleScreenSpawner : MonoBehaviour {

	public TitleScreenScroll[] elements;
	public TitleScreenScroll[] ground;
	public TitleScreenScroll[] clouds;

	//Heights
	public float minHeightCloud;
	public float maxHeightCloud;
	public float groundHeight;
	public float minHeightHump;
	public float maxHeightHump;
	public float minHeightSkinnyHump;
	public float maxHeightSkinnyHump;
	public float minHeightBush;
	public float maxHeightBush;

	//Information about how to spawn random elements and when to stop
	public float spawnX;
	public float finishedX;
	public float spawnTimer;
	public int minMoveSpeed;
	public int maxMoveSpeed;

	//Used to cycle through bg clouds
	private int currentBGCloud =0;

	//Information about how to spawn ground. Will be on a set timer with a predetermined.
	public float groundSpawnSpeed;
	public int groundMoveSpeed;

	//used to make the character animate
	public int FPSchar;
	public TitleScreenChar character;

	private float time;
	private float groundTime;

	// Use this for initialization
	void Start () {
		character.charFPS=FPSchar;
		for(int i=0; i<elements.Length; i++)
		{
			elements[i].Initiate(minHeightCloud, maxHeightCloud, groundHeight, minHeightHump, maxHeightHump, minHeightSkinnyHump, maxHeightSkinnyHump, minHeightBush, maxHeightBush, spawnX, finishedX);
			//Seed the elements already on screen with a random speed so they can start immediately
			if(elements[i].go == true)
			{
				int ranSpeed = Random.Range (minMoveSpeed, maxMoveSpeed+1);
				elements[i].StartMoving(ranSpeed, true);
			}
		}
		for(int i=0; i<ground.Length; i++)
		{
			ground[i].Initiate(minHeightCloud, maxHeightCloud, groundHeight, minHeightHump, maxHeightHump, minHeightSkinnyHump, maxHeightSkinnyHump, minHeightBush, maxHeightBush, spawnX, finishedX);
			ground[i].speed = groundMoveSpeed;
			if(ground[i].go == true)
			{
				//start moving the ground immediately
				ground[i].StartMoving(groundMoveSpeed, true);
			}
		}
		for(int i=0; i<clouds.Length; i++)
		{
			clouds[i].Initiate(minHeightCloud, maxHeightCloud, groundHeight, minHeightHump, maxHeightHump, minHeightSkinnyHump, maxHeightSkinnyHump, minHeightBush, maxHeightBush, spawnX, finishedX);
		}
	}
	
	// Update is called once per frame
	void Update () {
		//handle elements differently than ground
		time+=Time.deltaTime;
		groundTime+=Time.deltaTime;
		int ranSpeed = Random.Range (minMoveSpeed, maxMoveSpeed);
		if(time>=spawnTimer)
		{
			time=0;
			int ranElement1 = Random.Range (0, elements.Length);
			int ranElement2 = Random.Range (0, elements.Length);
			int ranElement3 = Random.Range (0, elements.Length);
			if(elements[ranElement1].go==false)
			{
				elements[ranElement1].StartMoving(ranSpeed, false);
				if(elements[ranElement1].cloud==false)
				{
					CloudCheck ();
				}
			}else if(elements[ranElement2].go==false){
				elements[ranElement2].StartMoving(ranSpeed, false);
				if(elements[ranElement2].cloud==false)
				{
					CloudCheck ();
				}
			}else if(elements[ranElement3].go==false){
				elements[ranElement3].StartMoving(ranSpeed, false);
				if(elements[ranElement3].cloud==false)
				{
					CloudCheck ();
				}
			}
		}
		if(groundTime>=groundSpawnSpeed)
		{
			groundTime=0;
			//spawn the first ground segment not currently moving
			for(int i=0; i<ground.Length; i++)
			{
				if(ground[i].go==false)
				{
					ground[i].StartMoving(groundMoveSpeed, false);
					break;
				}
			}
		}
	}

	public void CloudCheck()
	{
		//If we didn't spawn a cloud do a 50/50 chance to spawn a cloud as well. If we do we need it to be in the background.
		float randomCheck = Random.Range (0.0f, 1.0f);
		int ranSpeed = Random.Range (minMoveSpeed, maxMoveSpeed);
		if(randomCheck>0.5f)
		{
			if(clouds[currentBGCloud].go==false)
			{
				clouds[currentBGCloud].StartMoving(ranSpeed, false);
				currentBGCloud+=1;
				if(currentBGCloud==clouds.Length)
				{
					currentBGCloud=0;
				}
			}
		}
	}
}
