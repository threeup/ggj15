using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextureCycle : MonoBehaviour {

	public float framesPerSec;
	public Image me;
	public Sprite[] sprites;

	private int currentSprite;
	private float time;


	// Use this for initialization
	void Start () {
		currentSprite=0;
	}
	
	// Update is called once per frame
	void Update () {
		time+=Time.deltaTime;
		if(time>=framesPerSec/1)
		{
			time=0;
			currentSprite+=1;
			if(currentSprite==sprites.Length)
			{
				currentSprite=0;
			}
			me.sprite=sprites[currentSprite];
		}
	}
}
