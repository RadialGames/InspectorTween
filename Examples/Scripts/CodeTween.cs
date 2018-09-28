using UnityEngine;

namespace InspectorTween {
	public class CodeTween : MonoBehaviour {
		public TweenPosition newTween;

		private void Start() {
			newTween =(TweenPosition) TweenBase.AddTween<TweenPosition>(this.gameObject).SetStartValue(Vector3.zero).SetEndValue(0, 5f, 0).SetTime(0.6f).SetAnimationCurve(AnimationCurves.AnimationCurveType.Sin);
			newTween.events.onLoopComplete.AddListener(KillTweenComponent);
		}

		private void KillTweenComponent() {
			if ( newTween != null ) {
				Destroy(newTween);
			}
		}
	}
}