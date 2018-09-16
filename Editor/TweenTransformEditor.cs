using InspectorTween;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TweenTransform))]
[CanEditMultipleObjects]
public class TweenTransformEditor : TweenBaseEditor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
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
    public virtual void SetToStart() {
        TweenTransform tween = ((TweenTransform) target);
        tween.targetTransform = tween.transform;
        tween.SetToLerpPoint(0f);
    }

    public virtual void SetToEnd() {
        TweenTransform tween = ((TweenTransform) target);
        tween.targetTransform = tween.transform;
        tween.SetToLerpPoint(1f);
    }

    protected virtual void InsertFrame() {
    }
}

[CustomEditor(typeof(TweenPosition))]
[CanEditMultipleObjects]
public class TweenPositionEditor : TweenTransformEditor {

    protected override void InsertFrame() {
        var tween = (TweenPosition) target;
        var keys = tween.values;
        int count = keys.Length;
        var newKeys = new Vector3[count + 1];
        for ( int i = 0; i < count; i++ ) {
            newKeys[i] = keys[i];
        }
        int insertFrame = count - 1;
        newKeys[count] = newKeys[count-1];
        newKeys[insertFrame] = tween.transform.localPosition;
        tween.values = newKeys;
    }
}

[CustomEditor(typeof(TweenScale))]
[CanEditMultipleObjects]
public class TweenScaleEditor : TweenTransformEditor {


    protected override void InsertFrame() {
        var tween = (TweenScale) target;
        var keys = tween.values;
        int count = keys.Length;
        var newKeys = new Vector3[count + 1];
        for ( int i = 0; i < count; i++ ) {
            newKeys[i] = keys[i];
        }
        int insertFrame = count - 1;
        newKeys[count] = newKeys[count-1];
        newKeys[insertFrame] = tween.transform.localScale;
        tween.values = newKeys;
    }
}

[CustomEditor(typeof(TweenRotation))]
[CanEditMultipleObjects]
public class TweenRotationEditor : TweenTransformEditor {


    protected override void InsertFrame() {
        var tween = (TweenRotation) target;
        var keys = tween.values;
        int count = keys.Length;
        var newKeys = new Vector3[count + 1];
        for ( int i = 0; i < count; i++ ) {
            newKeys[i] = keys[i];
        }
        int insertFrame = count - 1;
        newKeys[count] = newKeys[count-1];
        newKeys[insertFrame] = tween.transform.localRotation.eulerAngles;
        tween.values = newKeys;
    }
}

[CustomEditor(typeof(TweenColor))]
[CanEditMultipleObjects]
public class TweenColorEditor : TweenBaseEditor {

}
[CustomEditor(typeof(TweenColorRotation))]
[CanEditMultipleObjects]
public class TweenColorRotationEditor : TweenTransformEditor {
    public override void OnInspectorGUI() {
        // base.OnInspectorGUI();
        TweenColorRotation item = (TweenColorRotation)target;
        base.OnInspectorGUI();
        if (item.setMatrix) {
            EditorGUILayout.HelpBox("use _MatrixYIQ property for supplied shaders or equivalent.", MessageType.Info);
        }
        if ( GUILayout.Button("SetColor") ) {
            SetColor();
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
    public override void SetToStart() {
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


    public override void SetToEnd() {
        TweenColorRotation tween = ((TweenColorRotation) target);
        if ( !tween.useMaterial ) {
            tween.Awake();
            tween.SetToLerpPoint(1f);
        }

    }
    protected override void InsertFrame() {
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
public class TweenMaterialFloatEditor : TweenBaseEditor {

}
[CustomEditor(typeof(TweenProperty))]
[CanEditMultipleObjects]
public class TweenPropertyEditor : TweenBaseEditor {

}
