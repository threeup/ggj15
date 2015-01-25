using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OpeningCutscene : MonoBehaviour {

	//Used for the shopkeeper's wordbubble
	public TextBubbleAdd wordBubbleText;
	public Image wordBubbleVisual;
	public float textSpeed;
	public GameObject bubble;
	public AudioSource talkingNoise;
	public AudioSource music;

	//Used for fade in/out
	private bool fadeFromBlack = false;
	private bool fadeToBlack = false;
	public Image blackFade;
	public float fadeSpeed;

	//used to cycle between open/closed mouth
	public Image shopkeeper;
	public Sprite openMouth;
	public Sprite closedMouth;
	public float mouthSpeed;
	private bool mouthOpen;
	private float time;

	public string[] phraseList;
	public float firstPhraseTime;
	public float secondPhraseTime;
	public float thirdPhraseTime;
	public float fourthPhraseTime;
	public float fifthPhraseTime;
	public float endTime;
	
	public void Start()
	{
		bubble.SetActive(false);
		FadeFromBlack();
	}

	// Update is called once per frame
	void Update () {
		if(wordBubbleText.finished==false)
		{
			time+=Time.deltaTime;
			if(time>=(1.0f/mouthSpeed)){
				time=0;
				if(mouthOpen==true)
				{
					mouthOpen=false;
					shopkeeper.sprite=closedMouth;
					if(talkingNoise.isPlaying==false)
					{
						talkingNoise.Play();
					}
				}else{
					mouthOpen=true;
					shopkeeper.sprite = openMouth;
				}
			}
		}else{
			mouthOpen=false;
			shopkeeper.sprite=closedMouth;
			talkingNoise.Stop();
		}
		if(fadeFromBlack==true)
		{
			Color a = blackFade.color;
			a.a-=(fadeSpeed*Time.deltaTime);
			if(a.a<=0)
			{
				//Done
				fadeFromBlack=false;
				FirstPhrase();
			}else{
				blackFade.color = a;
			}
		}
		if(fadeToBlack==true)
		{
			Color a = blackFade.color;
			a.a+=(fadeSpeed*Time.deltaTime/1.2f);
			music.volume=0.5f-a.a;
			if(a.a>=1.0f)
			{
				//Done
				fadeToBlack = false;
				Application.LoadLevel("Title");
			}else{
				blackFade.color = a;
			}
		}
	}

	public void FirstPhrase()
	{
		Speak(0);
		Invoke ("SecondPhrase", secondPhraseTime);
	}

	public void SecondPhrase()
	{
		Speak(1);
		Invoke ("ThirdPhrase", thirdPhraseTime);
	}

	public void ThirdPhrase()
	{
		Speak(2);
		Invoke ("FourthPhrase", fourthPhraseTime);
	}

	public void FourthPhrase()
	{
		Speak(3);
		Invoke ("FifthPhrase", fifthPhraseTime);
	}

	public void FifthPhrase()
	{
		Speak(4);
		Invoke ("FadeToBlack", endTime);
	}

	public void FadeToBlack()
	{
		fadeToBlack=true;
	}

	public void Speak(int phraseNum)
	{
		bubble.SetActive(true);
		wordBubbleText.beginWriting(phraseList[phraseNum], textSpeed);
	}

	public void FadeFromBlack()
	{
		fadeFromBlack=true;
	}
}
