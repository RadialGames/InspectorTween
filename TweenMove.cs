﻿// #Generic

using UnityEngine;
using System.Collections;
namespace InspectorTween{
	[AddComponentMenu("InspectorTween/TweenPosition",1)]
	public class TweenMove : InspectorTween.TweenTransform 
	{
		public Vector3[] movePositions = new Vector3[2]{Vector3.zero,Vector3.one};
		private Vector3 initialPosition;
		private Vector3 initialAnchor;
		private RectTransform rTransform;
		void MatchStartToCurrent() {
			movePositions[0] = this.transform.localPosition;
		}
		void MatchEndToCurrent() {
			movePositions[movePositions.Length-1] = this.transform.localPosition;
		}
		protected override void LerpParameters(float lerp)
		{
			if(rTransform){
				rTransform.anchoredPosition3D = LerpParameter(this.movePositions,lerp);
			}else{
				targetTransform.localPosition = LerpParameter(this.movePositions,lerp);
			}

		}
		public override void SetInitial(){
			if (!targetTransform) {
				targetTransform = transform;
			}
			initialPosition = targetTransform.localPosition;
			rTransform= GetComponent<RectTransform>();
			if(rTransform)
				initialAnchor = rTransform.anchoredPosition3D;
		}
		protected override bool HasValidParameters()
		{
			return movePositions.Length > 0;
		} 
		new protected void Awake()
		{
			base.Awake();
			SetInitial();

		}
		protected override Vector3 GetRelative (Vector3 endVal, float lerp)
		{
			return endVal + (rTransform!= null?initialAnchor:initialPosition);
		}
		protected override Vector3 GetStartRelative(Vector3 prelerped,float lerp)
		{
			return (prelerped + ((rTransform!= null?initialAnchor:initialPosition)*lerp));
		}
		protected override Vector3 GetEndRelative(Vector3 prelerped,float lerp)
		{
			return GetStartRelative(prelerped,1-lerp);
		}
	}
}