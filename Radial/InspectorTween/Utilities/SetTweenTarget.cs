using UnityEngine;
using System.Collections;
namespace InspectorTween
{
	public class SetTweenTarget : MonoBehaviour
	{
		public enum SetTarget { Parent, Root, Self };
		public SetTarget targetType;
		public TweenTransform tween;
		public bool playOnSet = false;

		void Set() {
			Transform target = transform;
			switch (targetType) {
				case SetTarget.Parent:
					target = transform.parent;
					break;
				case SetTarget.Root:
					target = transform.root;
					break;
				case SetTarget.Self:
					target = transform;
					break;
			}
			//At this point it'd be good to set up differently for material tweens etc too
			tween.targetTransform = target;
			if (playOnSet) {
				tween.PlayForwards();
			}
		}
		void Awake() {
			if (tween == null) {
				tween = GetComponent<TweenTransform>();
			}
			if (tween == null) {
				return;
			}
			Set();
		}
	}
}