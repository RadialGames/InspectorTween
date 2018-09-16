// #Generic

using UnityEngine;
using System.Collections;
namespace InspectorTween{

	[AddComponentMenu("InspectorTween/TweenPosition",1)]
	public class TweenPosition : TweenTransform
	{
		public Vector3[] movePositions = new Vector3[2]{Vector3.zero,Vector3.one};
		private Vector3 initialPosition;
		private Vector3 initialAnchor;
		private RectTransform rTransform;

		protected override void Awake()
		{
			base.Awake();
			SetInitial();
			CacheReversedTweenValues(movePositions);
		}
		public override Vector3[] values {
			get => movePositions;
			set => movePositions = value;
		}
		void MatchStartToCurrent() {
			movePositions[0] = this.transform.localPosition;
		}
		void MatchEndToCurrent() {
			movePositions[movePositions.Length-1] = this.transform.localPosition;
		}
		protected override void LerpParameters(float lerp) {
			Vector3[] values = movePositions;
			if ( timeSettings.reverseValues ) {
				values = reversedValues;
			}
			if(rTransform){
				rTransform.anchoredPosition3D = LerpParameter(values,lerp);
			}else{
				targetTransform.localPosition = LerpParameter(values,lerp);
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

		public new TweenPosition SetTime(float val) {
			return (TweenPosition) base.SetTime(val);
		}
		/// <summary>
		/// Set first position in tween values
		/// </summary>
		public TweenPosition SetStartValue(Vector3 val) {
			movePositions[0] = val;
			return this;
		}
		public TweenPosition SetStartValue(float x,float y,float z) {
			movePositions[0].x =x;
			movePositions[0].y =y;
			movePositions[0].z =z;
			return this;
		}
		/// <summary>
		/// Set last position in tween values
		/// </summary>
		public TweenPosition SetEndValue(Vector3 val) {
			movePositions[movePositions.Length-1] = val;
			return this;
		}
		public TweenPosition SetEndValue(float x,float y,float z) {
			movePositions[movePositions.Length-1].x =x;
			movePositions[movePositions.Length-1].y =y;
			movePositions[movePositions.Length-1].z =z;
			return this;
		}
	}
}