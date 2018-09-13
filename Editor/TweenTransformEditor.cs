using InspectorTween;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TweenMove))]
[CanEditMultipleObjects]
public class TweenMoveEditor : TweenBaseEditor {
    public override void OnInspectorGUI() {
        TweenBase item = (TweenBase)target;
        if (item.WarnCurveLooping(item)) {
            UnityEditor.EditorGUILayout.HelpBox("Curve is set to clamp, but tween is set to looping.", UnityEditor.MessageType.Warning);
        }
        if ( item.WarningRendererVisibilityCheck(item) ) {
            UnityEditor.EditorGUILayout.HelpBox("Auto Paused : Check PAUSE OFFSCREEN setting", UnityEditor.MessageType.Warning);
        }
        base.OnInspectorGUI();
        //if ( GUILayout.Button("Set to Start") ) {
        //EditorGUILayout.GetControlRect(false, 50);
        Rect buttonSize = EditorGUILayout.GetControlRect(false, 19);
        buttonSize.width = buttonSize.width * 0.5f;
        
        if ( GUI.Button(buttonSize,"Set to Start") ) {
            SetToStart();
        }

        buttonSize.x += buttonSize.width;
        if ( GUI.Button(buttonSize,"Set to End") ) {
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
[CustomEditor(typeof(TweenColorRotation))]
[CanEditMultipleObjects]
public class TweenColorRotationEditor : Editor {
    
    public override void OnInspectorGUI() {
        // base.OnInspectorGUI();
        TweenColorRotation item = (TweenColorRotation)target;
        if (item.WarnCurveLooping(item)) {
            UnityEditor.EditorGUILayout.HelpBox("Curve is set to clamp, but tween is set to looping.", UnityEditor.MessageType.Warning);
        }
        if ( item.WarningRendererVisibilityCheck(item) ) {
            UnityEditor.EditorGUILayout.HelpBox("Auto Paused : Check PAUSE OFFSCREEN setting", UnityEditor.MessageType.Warning);
        }
        DrawDefaultInspector();	
   
        if (item.setMatrix) {
            UnityEditor.EditorGUILayout.HelpBox("use _MatrixYIQ property for supplied shaders or equivalent.", UnityEditor.MessageType.Info);
        }
        if ( GUILayout.Button("SetColor") ) {
            SetColor();
        }
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

    private bool initialSet;

    public void SetColor() {
        if ( !initialSet ) {
            return;
        }
        TweenColorRotation tween = ((TweenColorRotation) target);
        tween.Awake();
    }
    public void SetToStart() {
        TweenColorRotation tween = ((TweenColorRotation) target);
        if ( !tween.useMaterial ) {
            Color initial;
            if ( initialSet ) {
                initial = tween.initialColor;
                tween.Awake();
                tween.initialColor = initial;
            } else {
                tween.Awake();
                initialSet = true;
            }
            tween.SetToLerpPoint(0f);
        } else {
            
            //if ( tween.material != null ) {
            //    tween.material.SetMatrix(tween.materialProperty,);
            //}
        }
    }


    public void SetToEnd() {
        TweenColorRotation tween = ((TweenColorRotation) target);
        if ( !tween.useMaterial ) {
            tween.Awake();
            tween.SetToLerpPoint(1f);
        }

    }
    protected void InsertFrame() {
        var tween = (TweenColorRotation) target;
        var keys = tween.hsvValues;
        int count = keys.Length;
        var newKeys = new Vector3[count + 1];
        for ( int i = 0; i < count; i++ ) {
            newKeys[i] = keys[i];
        }
        int insertFrame = count - 1;
        newKeys[count] = newKeys[count-1];
        newKeys[insertFrame] = keys[count-1];
        tween.hsvValues = newKeys;
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
