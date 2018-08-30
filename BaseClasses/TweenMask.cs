using System;
using UnityEngine;

namespace InspectorTween {
	[CreateAssetMenu(fileName = "TweenMask", menuName = "", order = 1)]
	public class TweenMask : ScriptableObject {
		public TweenBase.UpdateInterface updateSettings;

		
		public TweenBase.TimeInterface timeSettings;
		[NonSerialized] public float runtimeTime;
		[NonSerialized] public bool runtimeTimeReverse; //TODO : wrangle some fancyness so code side flipping doesn't mess up scriptable object initial setup.
		[NonSerialized] public bool runtimeTimeReverseValues; //TODO : wrangle some fancyness so code side flipping doesn't mess up scriptable object initial setup.

		public TweenBase.InterpolationInterface interpolation;
		public TweenBase.EventInterface events;

		private void Awake() {
			runtimeTime = timeSettings.time;
			runtimeTimeReverse = timeSettings.reverse;
			runtimeTimeReverseValues = timeSettings.reverseValues;
		}
	}
}

