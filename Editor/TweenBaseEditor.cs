using InspectorTween;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TweenBase))]
[CanEditMultipleObjects]
public class TweenBaseEditor : Editor{
	private AnimationCurve[] savedCurves = {
		 new AnimationCurve(new Keyframe(0,0,1,1,0.3333333f,0.3333333f),new Keyframe(1,1,1,1,0.3333333f,0.3333333f)),//Linear
		 new AnimationCurve(new Keyframe(0, 0, 0, 0, 0, 0), new Keyframe(1, 1, 0, 0, 0, 0)),//Hermite
		 new AnimationCurve(new Keyframe(0f,0f,2f,2f,0.3333333f,0.3333333f),new Keyframe(1f,1f,0f,0f,0.3333333f,0.3333333f)),//EaseIn
		 new AnimationCurve(new Keyframe(0f,0f,0f,0f,0.3333333f,0.3333333f),new Keyframe(1f,1f,2f,2f,0.3333333f,0.3333333f)),//EaseOut
		 new AnimationCurve(new Keyframe(0f,0f,0f,float.PositiveInfinity,0f,0f),new Keyframe(0.5f,1f,0f,0f,0.3333333f,0.3333333f),new Keyframe(1f,1f,0f,0f,0f,0f)),//Step                       
		 new AnimationCurve(new Keyframe(0f,0f,-1.085797f,-1.085797f,0.3333333f,0.1098927f),new Keyframe(1f,1f,0f,0f,0.3333333f,0.3333333f))//Back In
	};

    public override void OnInspectorGUI() {
	    TweenBase item = (TweenBase) target;
	    if ( item.WarnCurveLooping(item) ) {
		    UnityEditor.EditorGUILayout.HelpBox("Curve is set to clamp, but tween is set to looping.", UnityEditor.MessageType.Warning);
	    }

	    if ( item.WarningRendererVisibilityCheck(item) ) {
		    UnityEditor.EditorGUILayout.HelpBox("Auto Paused : Check PAUSE OFFSCREEN setting", UnityEditor.MessageType.Warning);
	    }
	    DoDrawDefaultInspector(this.serializedObject);
         //base.OnInspectorGUI();

    }
	bool DoDrawDefaultInspector(SerializedObject obj)
	{
		EditorGUI.BeginChangeCheck();
		obj.Update();
		SerializedProperty iterator = obj.GetIterator();
		for (bool enterChildren = true; iterator.NextVisible(enterChildren); enterChildren = false)
		{

			
			using (new EditorGUI.DisabledScope("m_Script" == iterator.propertyPath)) {
				//Debug.Log($"{iterator.displayName} has children : {iterator.propertyType} | {iterator.type.EndsWith("Interface")}" );
				//if ( iterator.type.EndsWith("Interface") ) {
				//	DoDrawDefaultInspector(iterator.serializedObject);
				//} else {
					EditorGUILayout.PropertyField(iterator, true, new GUILayoutOption[0]);
				//}
				
			}
			
			if ( InspectorTweenPreferences.isDeveloper && iterator.name == "timeSettings" ) {
				if (GUILayout.Button("Log Curve")) {
					LogCurve();
				}
				//EditorGUILayout.CurveField()
			}
		}
		obj.ApplyModifiedProperties();
		return EditorGUI.EndChangeCheck();
	}

	public void SetAnimationCurve(int index) {
		var tween = (TweenBase)target;
		tween.interpolationCurve = savedCurves[index];
	}
	
	public void LogCurve() {
		var tween = (TweenBase)target;
		//var c = new AnimationCurve(new Keyframe{new Keyframe()});
		AnimationCurve curve = tween.interpolation.interpolation;
		string pStr = "new AnimationCurve(";
		for ( int i = 0; i < curve.keys.Length; i++ ) {
			var key = curve.keys[i];
			pStr += $"new Keyframe({key.time}f,{key.value}f,{key.inTangent}f,{key.outTangent}f,{key.inWeight}f,{key.outWeight}f)";
			if ( i != curve.keys.Length - 1 ) {
				pStr += ",";
			}
		}
		pStr += ")";
		Debug.Log(pStr);
	}


	
}