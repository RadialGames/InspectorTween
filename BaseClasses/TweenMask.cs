using System;
using UnityEngine;

namespace InspectorTween {
	[CreateAssetMenu(fileName = "TweenMask", menuName = "", order = 1)]
	public class TweenMask : ScriptableObject {
		public bool simpleMode;
		[ConditionalHide("simpleMode",true,true)]
		public TweenBase.UpdateInterface updateSettings;

		
		public TweenBase.TimeInterface timeSettings;
		[NonSerialized] public float runtimeTime;
		[NonSerialized] public bool runtimeTimeReverse; //TODO : wrangle some fancyness so code side flipping doesn't mess up scriptable object initial setup.
		[NonSerialized] public bool runtimeTimeReverseValues; //TODO : wrangle some fancyness so code side flipping doesn't mess up scriptable object initial setup.

		public TweenBase.InterpolationInterface interpolation;

		private void Awake() {
			runtimeTime = timeSettings.time;
			runtimeTimeReverse = timeSettings.reverse;
			runtimeTimeReverseValues = timeSettings.reverseValues;
		}
		protected virtual void Reset() {//Called Editor only when Component is first added.
			if ( interpolation == null ) {
				interpolation = new TweenBase.InterpolationInterface();
				//Debug.Log("no class yet");
			}
			interpolation.interpolation.postWrapMode = WrapMode.Loop;
			interpolation.interpolation.preWrapMode = WrapMode.Loop;
#if UNITY_EDITOR
			simpleMode = UnityEditor.EditorPrefs.GetBool("SimpleMode", false);
#endif
		}
	}
}

