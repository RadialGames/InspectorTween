using System;
using InspectorTween;
using UnityEditor;
using UnityEngine;

//[CustomPropertyDrawer(typeof(TweenQueueItem))]
//public class TweenQueueItemInspector : PropertyDrawer {

//}

[CustomEditor(typeof(TweenQueue))]
public class TweenQueueEditor : Editor {
	public static void Show(SerializedProperty list) {
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PropertyField(list, GUIContent.none, false, GUILayout.Width(30));
		Rect rect = EditorGUILayout.GetControlRect();
		rect.position -= Vector2.right*30;
		rect.size = new Vector2(30,16);
		EditorGUI.PrefixLabel(rect, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("Tweens : Size"));
		EditorGUILayout.PropertyField(list.FindPropertyRelative("Array.size"), GUIContent.none); //Use "Array.size" so that it's editable.
		EditorGUILayout.EndHorizontal();
		EditorGUI.indentLevel += 1;
		if ( list.isExpanded ) {
			for ( int i = 0; i < list.arraySize; i++ ) {
				var element = list.GetArrayElementAtIndex(i); //this is the tween reference
				//element.name = "data" // element.displayName == "Element#"
				//element.type == "PPtr<$TweenBase>"
				//element.propertyType == "ObjectReference"

				string tweenName = element.displayName;
				TweenBase tween = null;
				if ( element.objectReferenceValue != null ) {
					tween = (TweenBase) element.objectReferenceValue;
					string nameVal = tween.name;
					if ( string.IsNullOrEmpty(nameVal) == false ) {
						tweenName = nameVal;
					} else {
						int thisIndex = Array.IndexOf( tween.GetComponents<TweenBase>(), tween) + 1;
						tweenName = "Tween-" + thisIndex + " : " + tween.GetType().Name.Replace("Tween","");
						
						
					}
				}

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PropertyField(element, new GUIContent(tweenName), true); //GUIContent.none,true);//
				bool next = GUILayout.Button(new GUIContent("*\u25bc", "select Next tween on target"), EditorStyles.miniButton, GUILayout.Width(30));
				bool previous = GUILayout.Button(new GUIContent("*\u25b2", "select Previous tween on target"), EditorStyles.miniButton, GUILayout.Width(30));
				if ( tween != null ) {
					if ( next ) {
						element.objectReferenceValue = IncrementTarget(tween, false);
					}

					if ( previous ) {
						element.objectReferenceValue = IncrementTarget(tween, true);
					}
				}

				EditorGUILayout.EndHorizontal();
			}
		}

		EditorGUI.indentLevel -= 1;
	}

	public static TweenBase IncrementTarget(TweenBase target, bool reverse) {
		var tweensOnObject = target.GetComponents<TweenBase>();
		int thisIndex = Array.IndexOf(tweensOnObject, target);
		if ( thisIndex == -1 ) {
			Debug.LogError("Wha??? isn't on selF???");
			return target;
		}

		if ( thisIndex == tweensOnObject.Length - 1 && reverse == false ) {
			return target;
		}

		if ( thisIndex == 0 && reverse ) {
			return target;
		}

		int returnIndex = thisIndex + (reverse ? -1 : 1);
		return tweensOnObject[returnIndex];
	}


	public void DoGUI(Rect rect, SerializedProperty prop, GUIContent label) {
		rect = EditorGUILayout.GetControlRect();
		rect.size = new Vector2(16,16); 
		EditorGUI.PropertyField(rect, prop, GUIContent.none, false); //Draw 'Element' / collapser thing
		rect.position += new Vector2(10,0);
		rect.size = new Vector2(300,16); 
		EditorGUI.PropertyField(rect, prop.FindPropertyRelative("label"), GUIContent.none, true); //,GUIContent.none, true,GUILayout.MinWidth(200));

		if ( prop.isExpanded ) {
			//GUILayout.Space(-16);
			EditorGUI.indentLevel += 1;

			SerializedProperty tweenProp = prop.FindPropertyRelative("tweens");
			int depth = tweenProp.depth;

			Show(tweenProp);
			//#Draw the rest
			tweenProp.NextVisible(false);
			do {
				EditorGUILayout.PropertyField(tweenProp, true); //This draws default inspector elements
				tweenProp.NextVisible(true);
			} while ( tweenProp.depth == depth );

			prop.serializedObject.ApplyModifiedProperties();
			EditorGUI.indentLevel -= 1;
		}

		//EditorGUI.EndProperty();
	}


	public override void OnInspectorGUI() {
		EditorGUI.BeginChangeCheck();
		this.serializedObject.Update();
		var property = this.serializedObject.GetIterator();//first is Monobehaviour itself.
		property.NextVisible(true);//Script property.
		EditorGUILayout.PropertyField(property, true);
		bool hasNext = true;
		property.NextVisible(true);//Assume this to be TweenQueue
		while ( hasNext &&  property.name != "itemQueue" ) {
			EditorGUILayout.PropertyField(property, true); //This draws default inspector elements
			hasNext = property.NextVisible(true);
		}
		
		

		Rect rect = GUILayoutUtility.GetLastRect();
		GUILayoutUtility.GetRect(0, 30);

		rect = GUILayoutUtility.GetLastRect();
		rect.size = new Vector2(60,16); 
		EditorGUI.PropertyField(rect, property, new GUIContent(property.displayName), false);
		rect.position = rect.position + new Vector2(100, 0);
		rect.size = new Vector2(60,16); 
		EditorGUI.PropertyField(rect, property.FindPropertyRelative("Array.size"), GUIContent.none, false); //Use "Array.size" so that it's editable.

		
		
		int depth = property.depth;
		if ( property.isExpanded ) {
			EditorGUI.indentLevel += 1;
			GUILayoutUtility.GetRect(30, -16);
			for ( int i = 0; i < property.arraySize; i++ ) {
				rect.size = new Vector2(60,16); 
				DoGUI(rect, property.GetArrayElementAtIndex(i),new GUIContent(property.displayName));
			}
			EditorGUI.indentLevel -= 1;
		}
		hasNext = property.NextVisible(false);//go onto next non child property
		
		do {
			EditorGUILayout.PropertyField(property, true); //This draws default inspector elements
			hasNext = property.NextVisible(true);
		} while (hasNext && property.depth == depth );
		//DrawDefaultInspector();


		if ( GUILayout.Button("Play Queue") ) {
			PlayQueue();
		}

		var style = new GUIStyle();
		style.alignment = TextAnchor.MiddleCenter;
		GUILayout.BeginHorizontal(style);
		if ( GUILayout.Button("Set to Start", GUILayout.Width(100)) ) {
			SetToPoint(0);
		}

		if ( GUILayout.Button("Set to Mid", GUILayout.Width(100)) ) {
			SetToPoint(0.5f);
		}

		if ( GUILayout.Button("Set to End", GUILayout.Width(100)) ) {
			SetToPoint(1f);
		}

		GUILayout.EndHorizontal();
		serializedObject.ApplyModifiedProperties();
		EditorGUI.EndChangeCheck();
	}

	public void PlayQueue() {
		if ( Application.isPlaying ) {
			Debug.Log("playing queue");
			var queue = (TweenQueue) target;
			queue.Play(queue.tweenToPlay);
		} else {
			Debug.Log("Only works while application is running");
		}
	}

	private void SetQueueState(TweenQueueItem queue, float lerp) {
		foreach ( var tween in queue.tweens ) {
			if ( !tween ) {
				continue;
			}

			tween.SetToLerpPoint(lerp);
		}

		//Debug.Log("Setting : " +queue.label + " to " + lerp);
	}

	public void SetToPoint(float val) {
		var queue = (TweenQueue) target;
		SetQueueState(queue.itemQueue[queue.tweenToPlay], val);
	}
}