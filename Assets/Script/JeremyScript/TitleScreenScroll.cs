using UnityEngine;
using System.Collections;

public class TitleScreenScroll : MonoBehaviour {

	private GameObject self;
	public bool cloud;
	public bool bgCloud;
	public bool ground;
	public bool hump;
	public bool skinnyHump;
	public bool bush;
	public SpriteRenderer front1;
	public SpriteRenderer back1;
	public SpriteRenderer back2;

	private float start;
	private float stop;
	private float minHeight;
	private float maxHeight;
	public bool go;

	public float speed;

	void Start()
	{
		self= this.gameObject;
		Debug.Log (self);
	}

	// Update is called once per frame
	void Update () {
		if(go==true)
		{
			Vector3 location = self.transform.localPosition;
			location.x-=(speed*Time.deltaTime);
			if(location.x<=stop)
			{
				//We made it
				location.x=stop;
				self.transform.localPosition = location;
				go=false;
			}else{
				self.transform.localPosition = location;
			}
		}
	}

	public void Initiate(float minCloud, float maxCloud, float groundHeight, float minHump, float maxHump, float minSkinnyHump, float maxSkinnyHump, float minBush, float maxBush, float spawnX, float finishX)
	{
		start=spawnX;
		stop=finishX;
		if(cloud==true)
		{
			minHeight = minCloud;
			maxHeight = maxCloud;
		}else if(ground==true){
			minHeight = groundHeight;
			maxHeight = groundHeight;
		}else if(hump==true){
			minHeight= minHump;
			maxHeight = maxHump;
		}else if(bush==true){
			minHeight = minBush;
			maxHeight = maxBush;
		}else if(skinnyHump==true){
			minHeight = minSkinnyHump;
			maxHeight = maxSkinnyHump;
		}
	}

	public void StartMoving(int speedToMove, bool startFromCurrentXPos)
	{
		float speedHolder = speedToMove/20.0f;
		speed = speedHolder;
		float height = Random.Range (minHeight, maxHeight);
		Vector3 temp = self.transform.localPosition;
		if(startFromCurrentXPos==false)
		{
			temp.x=start;
		}
		temp.y=height;
		self.transform.localPosition = temp;
		//This allows for parallax.. hacky but should work.
		if(front1!=null)
		{
			if(bgCloud==false)
			{
				front1.sortingOrder=(speedToMove*10);
			}else{
				front1.sortingOrder=(speedToMove*2);
			}
		}
		if(back1!=null)
		{
			if(bgCloud==false)
			{
				back1.sortingOrder=((speedToMove*10)-1);
			}else{
				back1.sortingOrder=((speedToMove*2)-1);
			}
		}
		if(back2!=null)
		{
			if(bgCloud==false)
			{
				back2.sortingOrder=((speedToMove*10)-1);
			}else{
				back2.sortingOrder=((speedToMove*2)-1);
			}
		}
		go=true;
	}
	
}
