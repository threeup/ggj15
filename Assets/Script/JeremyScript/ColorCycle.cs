using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ColorCycle : MonoBehaviour {

	public float framesPerSec;
	public Image me;
	public Color[] colors;

	private int currentColor;
	private float time;


	// Use this for initialization
	void Start () {
		currentColor=0;
	}
	
	// Update is called once per frame
	void Update () {
		time+=Time.deltaTime;
		if(time>=framesPerSec/1)
		{
			time=0;
			currentColor+=1;
			if(currentColor==colors.Length)
			{
				currentColor=0;
			}
			me.color=colors[currentColor];
		}
	}
}
