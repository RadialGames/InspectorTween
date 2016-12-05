using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System;
using InspectorTween;
[CustomEditor(typeof(TweenQueue))]
public class TweenQueueEditor : Editor {
	public override void OnInspectorGUI() {
		DrawDefaultInspector();		
		if (GUILayout.Button("Play Queue")) {
			PlayQueue();
		}
		var style = new GUIStyle();
		style.alignment = TextAnchor.MiddleCenter;
		GUILayout.BeginHorizontal(style);
		if (GUILayout.Button("Set to Start",GUILayout.Width(100))) {
			SetToPoint(0);
		}
		if (GUILayout.Button("Set to Mid",GUILayout.Width(100))) {
			SetToPoint(0.5f);
		}
		if (GUILayout.Button("Set to End",GUILayout.Width(100))) {
			SetToPoint(1f);
		}
		GUILayout.EndHorizontal();
	}
	
	public void PlayQueue() {
		if(Application.isPlaying){
			Debug.Log ("playing queue");
			var queue = (TweenQueue)target;
			queue.Play(queue.tweenToPlay);
		}
		else{
			Debug.Log ("Only works while application is running");
		}
	}
	private void SetQueueState(TweenQueueItem queue,float lerp){
		foreach(var tween in queue.tweens){
			if(!tween){
				continue;
			}
			tween.SetToLerpPoint(lerp);
		}
		//Debug.Log("Setting : " +queue.label + " to " + lerp);
	}
	public void SetToPoint(float val) {
		var queue = (TweenQueue)target;
		SetQueueState(queue.itemQueue[queue.tweenToPlay],val);
	}

}

