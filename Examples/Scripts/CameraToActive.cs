﻿using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InspectorTween.InspectorTweenExamples {
	public class CameraToActive : MonoBehaviour {
		[SerializeField] private EventSystem eventSystem;
		protected TweenPosition moveTween;
		protected TweenRotation rotationTween;
		public GameObject currentSelected;
		protected TweenQueue[] becameSelectedTween;
		private const string Q_ON_SELECTED = "OnSelected";
		private const float MIN_TRAVEL_TIME = 0.2f;
		public float DISTANCE_PER_SECOND = 20f;

		protected virtual void Start() {
			if ( eventSystem == null ) {
				eventSystem = EventSystem.current;
			}

#if UNITY_2019_1_OR_NEWER			
			var numSelectables = Selectable.allSelectablesArray.Length;
#else
			var numSelectables = Selectable.allSelectables.Count;
#endif
			becameSelectedTween = new TweenQueue[numSelectables];
			for ( int index = 0; index < numSelectables; index++ ) {
#if UNITY_2019_1_OR_NEWER
				Selectable selectable = Selectable.allSelectablesArray[index];
#else				
				Selectable selectable = Selectable.allSelectables[index];
#endif
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
			moveTween.events.onLoopComplete = new UnityEvent();
			moveTween.events.onLoopComplete.AddListener(OnDestinationArrive);
			rotationTween = (TweenRotation) TweenBase.AddTween<TweenRotation>(gameObject, play: false, loop: false).SetAnimationCurve(AnimationCurves.AnimationCurveType.EaseIn);
			rotationTween.interpolationCurve.postWrapMode = WrapMode.Clamp;
			var initialRotation = transform.eulerAngles;
			rotationTween.values[0].x = initialRotation.x;
			rotationTween.values[0].z = initialRotation.z;
			rotationTween.values[1].x = initialRotation.x;
			rotationTween.values[1].z = initialRotation.z;
		}

		protected virtual void OnDestinationArrive() {
		}

		protected virtual void OnDestinationDepart() {
		}
		protected virtual void FixedUpdate() {
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
#if UNITY_2019_1_OR_NEWER
				currentIndex = Array.IndexOf<Selectable>(Selectable.allSelectablesArray, currentSelected.GetComponent<Selectable>());
#else
				currentIndex = Selectable.allSelectables.IndexOf(currentSelected.GetComponent<Selectable>());
#endif
				if ( currentIndex >= 0 && currentIndex < becameSelectedTween.Length ) {
					if ( becameSelectedTween[currentIndex] != null ) {
						int qInd = becameSelectedTween[currentIndex].GetNamedIndex(Q_ON_SELECTED);
						if ( qInd >= 0 ) {
							becameSelectedTween[currentIndex].itemQueue[qInd].reverse = true;
							becameSelectedTween[currentIndex].Play(qInd);
						}
					}
				}
				OnDestinationDepart();
			}

			currentSelected = eventSystem.currentSelectedGameObject;
			if ( currentSelected == null ) {
				return;
			}
#if UNITY_2019_1_OR_NEWER
			currentIndex = Array.IndexOf<Selectable>(Selectable.allSelectablesArray, currentSelected.GetComponent<Selectable>());
#else
			currentIndex = Selectable.allSelectables.IndexOf(currentSelected.GetComponent<Selectable>());
#endif
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

		protected float Fix360(float val) {
			if ( val > 180 ) {
				val -= 360;
			}

			if ( val < -180 ) {
				val += 360;
			}

			return val;
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
			float distanceToTravel = Vector3.Distance(currentSelected.transform.position , transform.position);
			moveTween.time = Mathf.Max(MIN_TRAVEL_TIME, distanceToTravel / DISTANCE_PER_SECOND);
			//if ( moveTween.enabled == false ) {
			moveTween.CancelTween(TweenBase.TweenCancelType.HardStop);
			moveTween.PlayForwards();
			
			var rot = moveTween.targetTransform.eulerAngles.y;
			rotationTween.values[0].y = Fix360(rot);
			rotationTween.values[1].y = Fix360(currentSelected.transform.eulerAngles.y);
			//TODO: fix 360 flip
			rotationTween.CancelTween(TweenBase.TweenCancelType.HardStop);
			rotationTween.PlayForwards();
			rotationTween.time = moveTween.time; //change to degrees per second calculation?...
		}
	}
}