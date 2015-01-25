#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(SpriteSequencer))]
public class SpriteSequencerEditor : Editor
{
	public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        SpriteSequencer spriter = (SpriteSequencer)target;
        if(GUILayout.Button("Default"))
        {
        	spriter.idleSet = new SpriteData[1];
        	spriter.idleSet[0] = new SpriteData(spriter.defaultSprite);
        	spriter.primarySet = new SpriteData[1];
        	spriter.primarySet[0] = new SpriteData(spriter.defaultSprite);
        	spriter.secondarySet = new SpriteData[1];
        	spriter.secondarySet[0] = new SpriteData(spriter.defaultSprite);
        	spriter.disabledSet = new SpriteData[1];
        	spriter.disabledSet[0] = new SpriteData(spriter.defaultSprite);

        }
    }
}
#endif