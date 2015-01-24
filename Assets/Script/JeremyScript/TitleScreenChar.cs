using UnityEngine;
using System.Collections;

public class TitleScreenChar : MonoBehaviour {

	public SpriteRenderer mainChar;
	public Sprite[] animList;

	public int charFPS;
	private float time;
	private int currentFrame=0;

	void Start()
	{
		mainChar.sprite=animList[currentFrame];
	}
	
	void Update() 
	{
		time+=Time.deltaTime;
		if(time>=(1.0f/charFPS))
		{
			time=0;
			currentFrame+=1;
			if(currentFrame==animList.Length)
			{
				currentFrame=0;
			}
			mainChar.sprite=animList[currentFrame];
		}
	}
}
