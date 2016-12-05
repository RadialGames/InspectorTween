using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System;
using InspectorTween;
[CustomEditor(typeof(TweenMove))]
public class TweenMoveEditor : Editor {
	public override void OnInspectorGUI() {
		DrawDefaultInspector();		
		if (GUILayout.Button("Set to Start")) {
			SetToStart();
		}
		if (GUILayout.Button("Set to End")) {
			SetToEnd();
		}
	}
	
	public void SetToStart() {
		((TweenTransform)target).SetToLerpPoint(0f);
	}
	public void SetToEnd(){
		((TweenTransform)target).SetToLerpPoint(1f);
	}
}
[CustomEditor(typeof(TweenScale))]
public class TweenScaleEditor : Editor {
	public override void OnInspectorGUI() {
		DrawDefaultInspector();		
		if (GUILayout.Button("Set to Start")) {
			SetToStart();
		}
		if (GUILayout.Button("Set to End")) {
			SetToEnd();
		}
	}
	
	public void SetToStart() {
		((TweenTransform)target).SetToLerpPoint(0f);
	}
	public void SetToEnd(){
		((TweenTransform)target).SetToLerpPoint(1f);
	}
}
[CustomEditor(typeof(TweenRotation))]
public class TweenRotationEditor : Editor {
	public override void OnInspectorGUI() {
		DrawDefaultInspector();		
		if (GUILayout.Button("Set to Start")) {
			SetToStart();
		}
		if (GUILayout.Button("Set to End")) {
			SetToEnd();
		}
	}
	
	public void SetToStart() {
		((TweenTransform)target).SetToLerpPoint(0f);
	}
	public void SetToEnd(){
		((TweenTransform)target).SetToLerpPoint(1f);
	}
}