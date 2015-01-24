#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(Level))]
public class LevelEditor : Editor
{
	public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        Level level = (Level)target;
        if(GUILayout.Button("Assign UID"))
        {
        	level.ResetUID();
            level.ScanAssignUID();
        }
    }
}
#endif