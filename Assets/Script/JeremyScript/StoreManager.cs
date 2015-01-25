using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour {

	//Used for the shopkeeper's wordbubble
	public TextBubbleAdd wordBubbleText;
	public Image wordBubbleVisual;
	public float textSpeed;
	public GameObject bubble;

	//used to cycle between open/closed mouth
	public Image shopkeeper;
	public Sprite openMouth;
	public Sprite closedMouth;
	public float mouthSpeed;
	private bool mouthOpen;
	private float time;

	//used for the product
	public Image storeProduct;
	public Text productName;
	public Text productPrice;

	public string[] phraseList;

	public void Start()
	{
		bubble.SetActive(false);
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
				}else{
					mouthOpen=true;
					shopkeeper.sprite = openMouth;
				}
			}
		}else{
			mouthOpen=false;
			shopkeeper.sprite=closedMouth;
		}
	}

	public void Speak(int phraseNum)
	{
		bubble.SetActive(true);
		wordBubbleText.beginWriting(phraseList[phraseNum], textSpeed);
	}

}
