using InspectorTween;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TweenMove))]
[CanEditMultipleObjects]
public class TweenMoveEditor : Editor {
    public override void OnInspectorGUI() {
        TweenBase item = (TweenBase)target;
        if (item.WarnCurveLooping(item)) {
            UnityEditor.EditorGUILayout.HelpBox("Curve is set to clamp, but tween is set to looping.", UnityEditor.MessageType.Warning);
        }
        if ( item.WarningRendererVisibilityCheck(item) ) {
            UnityEditor.EditorGUILayout.HelpBox("Auto Paused : Check PAUSE OFFSCREEN setting", UnityEditor.MessageType.Warning);
        }
        DrawDefaultInspector();
        if ( GUILayout.Button("Set to Start") ) {
            SetToStart();
        }
        if ( GUILayout.Button("Set to End") ) {
            SetToEnd();
        }
        if ( GUILayout.Button("InsertFrame") ) {
            InsertFrame();
        }
    }

    public void SetToStart() {
        TweenTransform tween = ((TweenTransform) target);
        tween.targetTransform = tween.transform;
        tween.SetToLerpPoint(0f);
    }

    public void SetToEnd() {
        TweenTransform tween = ((TweenTransform) target);
        tween.targetTransform = tween.transform;
        tween.SetToLerpPoint(1f);
    }
    protected void InsertFrame() {
        var tween = (TweenMove) target;
        var keys = tween.movePositions;
        int count = keys.Length;
        var newKeys = new Vector3[count + 1];
        for ( int i = 0; i < count; i++ ) {
            newKeys[i] = keys[i];
        }
        int insertFrame = count - 1;
        newKeys[count] = newKeys[count-1];
        newKeys[insertFrame] = tween.transform.localPosition;
        tween.movePositions = newKeys;
    }
}

[CustomEditor(typeof(TweenScale))]
[CanEditMultipleObjects]
public class TweenScaleEditor : Editor {
    public override void OnInspectorGUI() {
        TweenBase item = (TweenBase)target;
        if (item.WarnCurveLooping(item)) {
            UnityEditor.EditorGUILayout.HelpBox("Curve is set to clamp, but tween is set to looping.", UnityEditor.MessageType.Warning);
        }
        if ( item.WarningRendererVisibilityCheck(item) ) {
            UnityEditor.EditorGUILayout.HelpBox("Auto Paused : Check PAUSE OFFSCREEN setting", UnityEditor.MessageType.Warning);
        }
        DrawDefaultInspector();
        if ( GUILayout.Button("Set to Start") ) {
            SetToStart();
        }
        if ( GUILayout.Button("Set to End") ) {
            SetToEnd();
        }
        if ( GUILayout.Button("InsertFrame") ) {
            InsertFrame();
        }

    }

    protected void InsertFrame() {
        var tween = (TweenScale) target;
        var keys = tween.scales;
        int count = keys.Length;
        var newKeys = new Vector3[count + 1];
        for ( int i = 0; i < count; i++ ) {
            newKeys[i] = keys[i];
        }
        int insertFrame = count - 1;
        newKeys[count] = newKeys[count-1];
        newKeys[insertFrame] = tween.transform.localScale;
        tween.scales = newKeys;
    }
    
    public void SetToStart() {
        TweenTransform tween = ((TweenTransform) target);
        tween.targetTransform = tween.transform;
        tween.SetToLerpPoint(0f);
    }

    public void SetToEnd() {
        TweenTransform tween = ((TweenTransform) target);
        tween.targetTransform = tween.transform;
        tween.SetToLerpPoint(1f);
    }
}

[CustomEditor(typeof(TweenRotation))]
[CanEditMultipleObjects]
public class TweenRotationEditor : Editor {
    public override void OnInspectorGUI() {
       // base.OnInspectorGUI();
        TweenBase item = (TweenBase)target;
        if (item.WarnCurveLooping(item)) {
            UnityEditor.EditorGUILayout.HelpBox("Curve is set to clamp, but tween is set to looping.", UnityEditor.MessageType.Warning);
        }
        if ( item.WarningRendererVisibilityCheck(item) ) {
            UnityEditor.EditorGUILayout.HelpBox("Auto Paused : Check PAUSE OFFSCREEN setting", UnityEditor.MessageType.Warning);
        }
        DrawDefaultInspector();		

        if ( GUILayout.Button("Set to Start") ) {
            SetToStart();
        }
        if ( GUILayout.Button("Set to End") ) {
            SetToEnd();
        }
        if ( GUILayout.Button("InsertFrame") ) {
            InsertFrame();
        }

    }

    public void SetToStart() {
        TweenTransform tween = ((TweenTransform) target);
        tween.targetTransform = tween.transform;
        tween.SetToLerpPoint(0f);
    }

    public void SetToEnd() {
        TweenTransform tween = ((TweenTransform) target);
        tween.targetTransform = tween.transform;
        tween.SetToLerpPoint(1f);
    }
    protected void InsertFrame() {
        var tween = (TweenRotation) target;
        var keys = tween.moveRotations;
        int count = keys.Length;
        var newKeys = new Vector3[count + 1];
        for ( int i = 0; i < count; i++ ) {
            newKeys[i] = keys[i];
        }
        int insertFrame = count - 1;
        newKeys[count] = newKeys[count-1];
        newKeys[insertFrame] = tween.transform.localRotation.eulerAngles;
        tween.moveRotations = newKeys;
    }
}
[CustomEditor(typeof(TweenColor))]
[CanEditMultipleObjects]
public class TweenColorEditor : Editor {
    public override void OnInspectorGUI() {
        // base.OnInspectorGUI();
        TweenBase item = (TweenBase)target;
        if (item.WarnCurveLooping(item)) {
            UnityEditor.EditorGUILayout.HelpBox("Curve is set to clamp, but tween is set to looping.", UnityEditor.MessageType.Warning);
        }
        if ( item.WarningRendererVisibilityCheck(item) ) {
            UnityEditor.EditorGUILayout.HelpBox("Auto Paused : Check PAUSE OFFSCREEN setting", UnityEditor.MessageType.Warning);
        }
        DrawDefaultInspector();	
    }
}
[CustomEditor(typeof(TweenMaterialFloat))]
[CanEditMultipleObjects]
public class TweenMaterialFloatEditor : Editor {
    public override void OnInspectorGUI() {
        // base.OnInspectorGUI();
        TweenBase item = (TweenBase)target;
        if (item.WarnCurveLooping(item)) {
            UnityEditor.EditorGUILayout.HelpBox("Curve is set to clamp, but tween is set to looping.", UnityEditor.MessageType.Warning);
        }
        if ( item.WarningRendererVisibilityCheck(item) ) {
            UnityEditor.EditorGUILayout.HelpBox("Auto Paused : Check PAUSE OFFSCREEN setting", UnityEditor.MessageType.Warning);
        }
        DrawDefaultInspector();	
    }
}
[CustomEditor(typeof(TweenProperty))]
[CanEditMultipleObjects]
public class TweenPropertyEditor : Editor {
    public override void OnInspectorGUI() {
        // base.OnInspectorGUI();
        TweenBase item = (TweenBase)target;
        if (item.WarnCurveLooping(item)) {
            UnityEditor.EditorGUILayout.HelpBox("Curve is set to clamp, but tween is set to looping.", UnityEditor.MessageType.Warning);
        }
        if ( item.WarningRendererVisibilityCheck(item) ) {
            UnityEditor.EditorGUILayout.HelpBox("Auto Paused : Check PAUSE OFFSCREEN setting", UnityEditor.MessageType.Warning);
        }
        DrawDefaultInspector();	
    }
}