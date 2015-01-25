using UnityEngine;
using System.Collections;

public class SpriteSequencer : MonoBehaviour {

	public SpriteRenderer mainRenderer;
	public Sprite defaultSprite;

	private Sprite[] currentSet;

	public Sprite[] walkSet;

	private BasicTimer sequenceTimer = new BasicTimer(0.1f);
	private BasicIndex currentIndex = new BasicIndex(0);

	public void Awake()
	{
		SetCycle(walkSet);
	}

	public void SetCycle(Sprite[] nextSet)
	{
		currentSet = nextSet;
		currentIndex.SetMax(currentSet.Length-1);
	}

	public void Update()
	{
		float deltaTime = Time.deltaTime;
		if( sequenceTimer.Tick(deltaTime) )
		{
			currentIndex.Next();
			mainRenderer.sprite = currentSet[currentIndex.Val];
		}
	}
}
