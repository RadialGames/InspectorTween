using System;
using System.Collections.Generic;
using InspectorTween;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class CameraToActive : MonoBehaviour {
	[SerializeField]private EventSystem eventSystem;
	private TweenPosition moveTween;
	private GameObject currentSelected;
	private TweenQueue[] becameSelectedTween;
	void Start() {
		becameSelectedTween = new TweenQueue[Selectable.allSelectables.Count];
		for ( int index = 0; index < Selectable.allSelectables.Count; index++ ) {
			Selectable selectable = Selectable.allSelectables[index];
			becameSelectedTween[index] = selectable.GetComponent<TweenQueue>();
		}

		moveTween = (TweenPosition)TweenBase.AddTween<TweenPosition>(gameObject,false).SetAnimationCurve(AnimationCurves.AnimationCurveType.EaseIn);
		moveTween.interpolation.loop = false;
		moveTween.updateSettings.pauseOffscreen = TweenBase.VisibilityPause.None;
	}
	void FixedUpdate () {
		if ( eventSystem.currentSelectedGameObject == null && currentSelected != null ) {
			eventSystem.SetSelectedGameObject(currentSelected);//fix for mouse click unselecting
			return;
		}
		if ( currentSelected == eventSystem.currentSelectedGameObject ) {
			return;
		}

		currentSelected = eventSystem.currentSelectedGameObject;
		if ( currentSelected == null ) {
			return;
		}

		int currentIndex = Selectable.allSelectables.IndexOf( currentSelected.GetComponent<Selectable>());
		if(currentIndex > 0 && currentIndex < becameSelectedTween.Length) {
			if ( becameSelectedTween[currentIndex] != null ) {
				becameSelectedTween[currentIndex].Play("OnSelected");
			}
		}
		SetCameraDestination();
	}

	public void SetCameraDestination() {
		if ( moveTween == null ) {
			return;
		}
		moveTween.values[0] = transform.position;
		moveTween.values[1] = currentSelected.transform.position;
		float distanceToTravel = Mathf.Abs(currentSelected.transform.position.x - transform.position.x);
		moveTween.time = Mathf.Max(0.2f,distanceToTravel/10f);
		moveTween.PlayForwards();
	}
	
}
