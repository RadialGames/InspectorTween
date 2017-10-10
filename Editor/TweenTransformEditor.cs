using InspectorTween;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TweenMove))]
public class TweenMoveEditor : Editor {
    public override void OnInspectorGUI() {
        TweenBase item = (TweenBase)target;
        if (item.WarnCurveLooping(item)) {
            UnityEditor.EditorGUILayout.HelpBox("Curve is set to clamp, but tween is set to looping.", UnityEditor.MessageType.Warning);
        }
        DrawDefaultInspector();
        if ( GUILayout.Button("Set to Start") ) {
            SetToStart();
        }
        if ( GUILayout.Button("Set to End") ) {
            SetToEnd();
        }
    }

    public void SetToStart() {
        ((TweenTransform) target).SetToLerpPoint(0f);
    }

    public void SetToEnd() {
        ((TweenTransform) target).SetToLerpPoint(1f);
    }
}

[CustomEditor(typeof(TweenScale))]
public class TweenScaleEditor : Editor {
    public override void OnInspectorGUI() {
        TweenBase item = (TweenBase)target;
        if (item.WarnCurveLooping(item)) {
            UnityEditor.EditorGUILayout.HelpBox("Curve is set to clamp, but tween is set to looping.", UnityEditor.MessageType.Warning);
        }
        DrawDefaultInspector();
        if ( GUILayout.Button("Set to Start") ) {
            SetToStart();
        }
        if ( GUILayout.Button("Set to End") ) {
            SetToEnd();
        }

    }

    public void SetToStart() {
        ((TweenTransform) target).SetToLerpPoint(0f);
    }

    public void SetToEnd() {
        ((TweenTransform) target).SetToLerpPoint(1f);
    }
}

[CustomEditor(typeof(TweenRotation))]
public class TweenRotationEditor : Editor {
    public override void OnInspectorGUI() {
       // base.OnInspectorGUI();
        TweenBase item = (TweenBase)target;
        if (item.WarnCurveLooping(item)) {
            UnityEditor.EditorGUILayout.HelpBox("Curve is set to clamp, but tween is set to looping.", UnityEditor.MessageType.Warning);
        }
        DrawDefaultInspector();		

        if ( GUILayout.Button("Set to Start") ) {
            SetToStart();
        }
        if ( GUILayout.Button("Set to End") ) {
            SetToEnd();
        }

    }

    public void SetToStart() {
        ((TweenTransform) target).SetToLerpPoint(0f);
    }

    public void SetToEnd() {
        ((TweenTransform) target).SetToLerpPoint(1f);
    }
}
[CustomEditor(typeof(TweenColor))]
public class TweenColorEditor : Editor {
    public override void OnInspectorGUI() {
        // base.OnInspectorGUI();
        TweenBase item = (TweenBase)target;
        if (item.WarnCurveLooping(item)) {
            UnityEditor.EditorGUILayout.HelpBox("Curve is set to clamp, but tween is set to looping.", UnityEditor.MessageType.Warning);
        }
        DrawDefaultInspector();	
    }
}
[CustomEditor(typeof(TweenMaterialFloat))]
public class TweenMaterialFloatEditor : Editor {
    public override void OnInspectorGUI() {
        // base.OnInspectorGUI();
        TweenBase item = (TweenBase)target;
        if (item.WarnCurveLooping(item)) {
            UnityEditor.EditorGUILayout.HelpBox("Curve is set to clamp, but tween is set to looping.", UnityEditor.MessageType.Warning);
        }
        DrawDefaultInspector();	
    }
}
[CustomEditor(typeof(TweenProperty))]
public class TweenPropertyEditor : Editor {
    public override void OnInspectorGUI() {
        // base.OnInspectorGUI();
        TweenBase item = (TweenBase)target;
        if (item.WarnCurveLooping(item)) {
            UnityEditor.EditorGUILayout.HelpBox("Curve is set to clamp, but tween is set to looping.", UnityEditor.MessageType.Warning);
        }
        DrawDefaultInspector();	
    }
}