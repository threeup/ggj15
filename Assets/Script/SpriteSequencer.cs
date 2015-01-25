﻿using UnityEngine;
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

	public Transform mainTransform;
	public Vector3 srcTranslation;
	public Vector3 srcRotation;
	public Vector3 srcScale;

	public void Awake()
	{
		SwitchState(idleSet, true);
		if( mainTransform == null )
		{
			mainTransform = this.transform;
		}
		srcTranslation = mainTransform.position;
		srcRotation = mainTransform.eulerAngles;
		srcScale = mainTransform.localScale;
	}

	public void SwitchState(SpriteData[] nextSet, bool looping)
	{
		if( currentSet != null )
		{
			mainTransform.DOLocalMove(srcTranslation, 0.01f);
			mainTransform.DOLocalRotate(srcRotation, 0.01f);
			mainTransform.DOScale(srcScale, 0.01f);
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
			Vector3 translation = srcTranslation;
			Vector3 rotation = srcRotation;
			Vector3 scale = srcScale;
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
				mainTransform.DOLocalMove(translation, spriteData.duration);
				mainTransform.DOLocalRotate(rotation, spriteData.duration);
				mainTransform.DOScale(scale, spriteData.duration);
			}
			lastSpriteData = spriteData;
			sequenceTimer.TimeVal = spriteData.duration;
			currentIndex.Next();
			
		}

	}
}
