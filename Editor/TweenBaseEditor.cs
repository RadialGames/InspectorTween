using InspectorTween;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TweenBase.InterpolationInterface))]
public class InterpolationInterfaceInspector : PropertyDrawer {
	private AnimationCurves.AnimationCurveType setCurve = AnimationCurves.AnimationCurveType.Custom;
	private AnimationCurves.AnimationCurveType lastCurve;
	public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label) {
		//NB!!! Defauly unity GUI doesn't like GUILayout usage??
		//EditorGUILayout.PropertyField(prop, true);
		EditorGUI.PropertyField(rect, property, label, true);//This draws default inspector elements
		bool curvesVisible = property.FindPropertyRelative("useCurve").boolValue;
		if ( curvesVisible && property.isExpanded ) {
			
			//EditorGUI.indentLevel += 1;
			//Rect propREct = GUILayoutUtility.GetRect(300, 16);
			int addPosition = 16;


			if ( EditorPrefs.GetBool("IsDeveloper", false) ) {
				Rect buttonRect = new Rect(rect.position.x + 60, rect.position.y + rect.height - addPosition, 120, 16);
				addPosition *= 2;
				if ( GUI.Button(buttonRect, new GUIContent("Debug : Log Curve")) ) {
					LogCurve(property);
				}
			}
			Rect enumRect = new Rect(rect.position.x + 30, rect.position.y + rect.height - addPosition, rect.width - 30, 16);
			setCurve = (AnimationCurves.AnimationCurveType) EditorGUI.EnumPopup(enumRect, new GUIContent("Set PredefinedCurve"), setCurve);

			if ( Application.isPlaying == false && setCurve != lastCurve ) {
				lastCurve = setCurve;
				SetAnimationCurve(property, (int) setCurve);
			}
			

		}
	}
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		bool curvesVisible = property.FindPropertyRelative("useCurve").boolValue;
		if ( curvesVisible && property.isExpanded ) {
			int add = 16;
			if ( EditorPrefs.GetBool("IsDeveloper", false) ) {
				add = 32;
			}
			return EditorGUI.GetPropertyHeight(property) + add;
		}
		return EditorGUI.GetPropertyHeight(property);
	}
	private static void SetAnimationCurve(SerializedProperty prop,int index) {
		if ( index < 0 || index >= AnimationCurves.savedCurves.Length ) {
			return;
		}
		SerializedProperty sp = prop.FindPropertyRelative("interpolation");
		AnimationCurve refCurve = AnimationCurves.savedCurves[index];
		sp.animationCurveValue = new AnimationCurve(refCurve.keys){postWrapMode = refCurve.postWrapMode,preWrapMode = refCurve.preWrapMode};
	}

	private void LogCurve(SerializedProperty prop) {
		AnimationCurve curve = prop.FindPropertyRelative("interpolation").animationCurveValue;
		string pStr = "new AnimationCurve(";
		for ( int i = 0; i < curve.keys.Length; i++ ) {
			var key = curve.keys[i];
			#if UNITY_2018_1_OR_NEWER
				pStr += $"new Keyframe({key.time}f,{key.value}f,{key.inTangent}f,{key.outTangent}f,{key.inWeight}f,{key.outWeight}f)";
			#else
				pStr += $"new Keyframe({key.time}f,{key.value}f,{key.inTangent}f,{key.outTangent}f)";
			#endif
			if ( i != curve.keys.Length - 1 ) {
				pStr += ",";
			}
		}
		pStr += ")";
		Debug.Log(pStr);
	}
	
}

[CustomEditor(typeof(TweenBase))]
[CanEditMultipleObjects]
public class TweenBaseEditor : Editor {

    public override void OnInspectorGUI() {
	    TweenBase item = (TweenBase) target;
	    if ( item.WarnCurveLooping(item) ) {
		    EditorGUILayout.HelpBox("Curve is set to clamp, but tween is set to looping.", MessageType.Warning);
	    }

	    if ( item.WarningRendererVisibilityCheck(item) ) {
		    EditorGUILayout.HelpBox("Auto Paused : Check PAUSE OFFSCREEN setting", MessageType.Warning);
	    }
	    //DoDrawDefaultInspector(this.serializedObject);
        base.OnInspectorGUI();
    }
	//bool DoDrawDefaultInspector(SerializedObject obj) {
	//	return DoDrawDefaultInspector(obj);
		//EditorGUI.BeginChangeCheck();
/*		obj.Update();
		SerializedProperty iterator = obj.GetIterator();
		for (bool enterChildren = true; iterator.NextVisible(enterChildren); enterChildren = false)
		{
			using (new EditorGUI.DisabledScope("m_Script" == iterator.propertyPath)) {
				EditorGUILayout.PropertyField(iterator, true, new GUILayoutOption[0]);
			}
		}
		obj.ApplyModifiedProperties();*/
		//return EditorGUI.EndChangeCheck();
	//}
	
}