using UnityEngine;
using System.Collections;
using DG.Tweening;

[System.Serializable]
public struct SpriteData
{
	public Sprite sprite;
	public bool hasTween;
	public Vector3 translation;
	public Vector3 rotation;
	public Vector3 scale;
	public float duration;

	public SpriteData(Sprite sprite)
	{
		this.sprite = sprite;
		this.hasTween = false;
		this.translation = Vector3.zero;
		this.rotation = Vector3.zero;
		this.scale = Vector3.one;
		this.duration = 0.1f;
	}
}

public class SpriteSequencer : MonoBehaviour {

	public SpriteRenderer mainRenderer;
	public Sprite defaultSprite;

	private SpriteData[] currentSet;

	public SpriteData[] idleSet;
	public SpriteData[] primarySet;
	public SpriteData[] secondarySet;
	public SpriteData[] disabledSet;

	private SpriteData lastSpriteData = new SpriteData(null);

	private BasicTimer sequenceTimer = new BasicTimer(0.1f);
	private BasicIndex currentIndex = new BasicIndex(0);

	public void Awake()
	{
		SwitchState(idleSet, true);
	}

	public void SwitchState(SpriteData[] nextSet, bool looping)
	{
		if( currentSet != null )
		{
			this.transform.DOLocalMove(Vector3.zero, 0.01f);
			this.transform.DOLocalRotate(Vector3.zero, 0.01f);
		}
		currentSet = nextSet;

		if( currentSet == null || currentSet.Length == 0 )
		{
			Debug.LogWarning(this+" Missing sprites", this.transform.parent);
		}

		currentIndex.SetMax(currentSet.Length-1);
		currentIndex.SetLooping(looping);
	}

	public void Update()
	{
		float deltaTime = Time.deltaTime;

		if( sequenceTimer.Tick(deltaTime) )
		{
			bool hasTween = lastSpriteData.hasTween;
			Vector3 translation = Vector3.zero;
			Vector3 rotation = Vector3.zero;
			Vector3 scale = Vector3.one;
			if( lastSpriteData.hasTween)
			{
				//translation = -lastSpriteData.translation;
				//rotation = -lastSpriteData.rotation;
				//scale = new Vector3(1f/lastSpriteData.scale.x, 1f/lastSpriteData.scale.y, 1f/lastSpriteData.scale.z);
			}
			SpriteData spriteData = currentSet[currentIndex.Val];
			mainRenderer.sprite = spriteData.sprite;
			hasTween |= spriteData.hasTween;
			if( spriteData.hasTween )
			{
				translation += spriteData.translation;
				rotation += spriteData.rotation;
				scale = new Vector3(scale.x*lastSpriteData.scale.x, scale.y*lastSpriteData.scale.y, scale.z*lastSpriteData.scale.z);
			}
			if( hasTween )
			{
				this.transform.DOLocalMove(translation, spriteData.duration);
				this.transform.DOLocalRotate(rotation, spriteData.duration);
				this.transform.DOScale(scale, spriteData.duration);
			}
			lastSpriteData = spriteData;
			sequenceTimer.TimeVal = spriteData.duration;
			currentIndex.Next();
			
		}
	}
}
