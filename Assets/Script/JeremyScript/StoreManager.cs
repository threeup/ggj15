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
	public StoreProductChanger product;

	public string[] phraseList;

	public void Start()
	{
		Director.Instance.productChanger = product;
		bubble.SetActive(false);
		//Figure out which state to be in, then set the product to the right state.
		product.updateProduct();
		if(product.currentState==StoreProductChanger.StoreStates.intro)
		{
			//Start the intro conversation
			Speak (0);
			Invoke ("SecondText", 8.0f);
		}
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

	public void SecondText()
	{
		Speak (1);
		Invoke ("ThirdText", 13.0f);
	}

	public void ThirdText()
	{
		Speak (2);
		Invoke ("FourthText", 10.0f);
	}

	public void FourthText()
	{
		Speak (3);
	}

	public void BackToLevelSelect()
	{
		Application.LoadLevel("LevelSelect");
	}

	public void BuyButton()
	{
		//Buy code goes here
		if(product.currentState==StoreProductChanger.StoreStates.intro || product.currentState==StoreProductChanger.StoreStates.noProduct || product.currentState==StoreProductChanger.StoreStates.noProduct2 || product.currentState==StoreProductChanger.StoreStates.finished)
		{
			Speak (11);
		}
		else
		{
			bool success = Director.Instance.PurchaseProgress(product.currentProductPrice);
		}
	}

}
