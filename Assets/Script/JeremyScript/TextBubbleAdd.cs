using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextBubbleAdd : MonoBehaviour {

	public string textToWrite;	
	public float CharsPerSecond;
	public Text myText;
	
	private float currentChars;
	public bool finished = true;
	private float timeTracker;
	
	
	public void beginWriting(string dialogue, float speed)
	{
		timeTracker=0;
		textToWrite= dialogue;
		CharsPerSecond = speed;
		finished=false;
	}
	
	public void Update()
	{
		if(finished==false)
		{
			timeTracker+= Time.deltaTime;
			currentChars=Mathf.Floor(CharsPerSecond*timeTracker);
			if(currentChars<=textToWrite.Length)
			{
				myText.text = textToWrite.Substring(0,(int)currentChars);
			}else{
				myText.text = textToWrite.Substring(0,textToWrite.Length);
			}
			if(currentChars>=textToWrite.Length)
			{
				finished=true;
			}
		}
	}
}
