using InspectorTween;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TweenBase))]
[CanEditMultipleObjects]
public class TweenBaseEditor : Editor {
    public override void OnInspectorGUI() {
         base.OnInspectorGUI();
    }
}