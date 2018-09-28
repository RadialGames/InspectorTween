using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InspectorTween.InspectorTweenExamples {
	public class CameraToActive : MonoBehaviour {
		[SerializeField] private EventSystem eventSystem;
		private TweenPosition moveTween;
		private GameObject currentSelected;
		private TweenQueue[] becameSelectedTween;
		private const string Q_ON_SELECTED = "OnSelected";
		private const float MIN_TRAVEL_TIME = 0.2f;
		private const float DISTANCE_PER_SECOND = 10f;

		private void Start() {
			becameSelectedTween = new TweenQueue[Selectable.allSelectables.Count];
			for ( int index = 0; index < Selectable.allSelectables.Count; index++ ) {
				Selectable selectable = Selectable.allSelectables[index];
				becameSelectedTween[index] = selectable.GetComponent<TweenQueue>();
				int qInd = becameSelectedTween[index].GetNamedIndex(Q_ON_SELECTED);
				if ( qInd < 0 ) {
					continue;
				}

				foreach ( TweenBase tweenBase in becameSelectedTween[index].itemQueue[qInd].tweens ) {
					tweenBase.SetToLerpPoint(0);
				}
			}

			moveTween = (TweenPosition) TweenBase.AddTween<TweenPosition>(gameObject, play:false,loop:false).SetAnimationCurve(AnimationCurves.AnimationCurveType.EaseIn);
		}

		private void FixedUpdate() {
			if ( eventSystem.currentSelectedGameObject == null && currentSelected != null ) {
				eventSystem.SetSelectedGameObject(currentSelected); //fix for mouse click unselecting
				return;
			}

			if ( currentSelected == eventSystem.currentSelectedGameObject ) {
				return;
			}

			int currentIndex;
			if ( currentSelected != null ) {
				//Find queue where we are and deselect that
				currentIndex = Selectable.allSelectables.IndexOf(currentSelected.GetComponent<Selectable>());
				if ( currentIndex >= 0 && currentIndex < becameSelectedTween.Length ) {
					if ( becameSelectedTween[currentIndex] != null ) {
						int qInd = becameSelectedTween[currentIndex].GetNamedIndex(Q_ON_SELECTED);
						if ( qInd >= 0 ) {
							becameSelectedTween[currentIndex].itemQueue[qInd].reverse = true;
							becameSelectedTween[currentIndex].Play(qInd);
						}
					}
				}
			}

			currentSelected = eventSystem.currentSelectedGameObject;
			if ( currentSelected == null ) {
				return;
			}

			currentIndex = Selectable.allSelectables.IndexOf(currentSelected.GetComponent<Selectable>());
			if ( currentIndex >= 0 && currentIndex < becameSelectedTween.Length ) {
				if ( becameSelectedTween[currentIndex] != null ) {
					int qInd = becameSelectedTween[currentIndex].GetNamedIndex(Q_ON_SELECTED);
					if ( qInd >= 0 ) {
						becameSelectedTween[currentIndex].itemQueue[qInd].reverse = false;
						becameSelectedTween[currentIndex].Play(qInd);
					}
				}
			}

			SetCameraDestination();
		}

		/// <summary>
		/// Set the camera tween to point and play
		/// </summary>
		public void SetCameraDestination() {
			if ( moveTween == null ) {
				return;
			}

			moveTween.values[0] = transform.position;
			moveTween.values[1] = currentSelected.transform.position;
			float distanceToTravel = Mathf.Abs(currentSelected.transform.position.x - transform.position.x);
			moveTween.time = Mathf.Max(MIN_TRAVEL_TIME, distanceToTravel / DISTANCE_PER_SECOND);
			moveTween.PlayForwards();
		}
	}
}