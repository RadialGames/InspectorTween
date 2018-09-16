using UnityEngine;

namespace InspectorTween {
	public class CodeTween : MonoBehaviour {
		private TweenPosition newTween;

		private void Start() {
			newTween = TweenBase.AddTween<TweenPosition>(this.gameObject).SetTime(0.6f).SetStartValue(Vector3.zero).SetEndValue(0, 5f, 0);
		}
	}
}