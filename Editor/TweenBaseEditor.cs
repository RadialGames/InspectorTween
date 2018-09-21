using InspectorTween;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TweenBase.InterpolationInterface))]
public class InterpolationInterfaceInspector : PropertyDrawer {
	private AnimationCurves.AnimationCurveType setCurve = AnimationCurves.AnimationCurveType.Custom;
	private AnimationCurves.AnimationCurveType lastCurve;
	public override void OnGUI(Rect rect, SerializedProperty prop, GUIContent label) {
		EditorGUILayout.PropertyField(prop, true);//This draws default inspector elements
		
		setCurve = (AnimationCurves.AnimationCurveType)EditorGUILayout.EnumPopup("Set PredefinedCurve",setCurve);
		
		if (Application.isPlaying == false && setCurve != lastCurve ) {
			lastCurve = setCurve;
			SetAnimationCurve(prop,(int)setCurve);
		}
		if ( EditorPrefs.GetBool("IsDeveloper", false) ) {
			if (GUILayout.Button("Debug : Log Curve")) {
				LogCurve(prop);
			}
		}
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
	bool DoDrawDefaultInspector(SerializedObject obj)
	{
		EditorGUI.BeginChangeCheck();
		obj.Update();
		SerializedProperty iterator = obj.GetIterator();
		for (bool enterChildren = true; iterator.NextVisible(enterChildren); enterChildren = false)
		{

			
			using (new EditorGUI.DisabledScope("m_Script" == iterator.propertyPath)) {
				EditorGUILayout.PropertyField(iterator, true, new GUILayoutOption[0]);
			}
			

		}
		obj.ApplyModifiedProperties();
		return EditorGUI.EndChangeCheck();
	}


	



	
}