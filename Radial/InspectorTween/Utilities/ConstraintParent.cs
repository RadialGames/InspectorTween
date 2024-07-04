using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace InspectorTween {
//[ExecuteInEditMode]
	public class ConstraintParent : MonoBehaviour {
		[Serializable]
		public class ConstraintTarget {
			public Transform target;
			[FormerlySerializedAs("usePosition")] public bool specifyPositionInTargetParentSpace = false;
			public Vector3 position;
			public float weight = 1f;
			public bool useSetRotation;
			public Vector3 rotation;
		};

		public ConstraintTarget[] targets = new ConstraintTarget[] {new ConstraintTarget()};

		//private Transform[] parent;

		public Vector3 positionalOffset = Vector3.zero;
		public bool xPosition = true;
		public bool yPosition = true;
		public bool zPosition = true;
		public bool useRotation = true;

		public Vector3 rotationalOffset;

		//private float[] weights;
		public bool maintainOffset = true;

		public bool rigigbodySweep = false;
		private Rigidbody _rigidbody;
#if UNITY_EDITOR
		private new Rigidbody rigidbody { //store renderer. needed for some child types.
#else
	private Rigidbody rigidbody{
#endif
			get {
				if ( _rigidbody == null ) {
					_rigidbody = GetComponent<Rigidbody>();
				}

				return _rigidbody;
			}
		}

		void Awake() {
			if ( targets.Length > 0 ) {
				if ( maintainOffset ) {
					InitializePositionalOffset();
				}
			}
		}

		public void InitializePositionalOffset() {
			positionalOffset = this.transform.position - GetWeightedPosition(true);
		}

		public void SetTarget(Transform targ, int indx, float weight) {
			if ( indx < targets.Length ) {
				targets[indx].target = targ;
				targets[indx].weight = weight;
				targets[indx].specifyPositionInTargetParentSpace = false;
			}
		}

		public void SetPosition(Vector3 worldPosition, int indx, float weight) {
			if ( indx < targets.Length ) {
				targets[indx].target = null;
				targets[indx].weight = weight;
				targets[indx].position = worldPosition;
				targets[indx].specifyPositionInTargetParentSpace = true;
			}
		}

		public void SetTargetWeight(int indx, float weight) {
			if ( indx < targets.Length ) {
				targets[indx].weight = weight;
			}
		}

		private Vector3 GetWeightedPosition(bool worldSpace = false) {
			var outV = this.transform.localPosition;
			if ( worldSpace ) {
				outV = this.transform.position;
				if ( targets.Length > 0 ) {
					for ( int i = 0; i < targets.Length; i++ ) {
						if ( targets[i].specifyPositionInTargetParentSpace ) {
							outV = Vector3.Lerp(outV, targets[i].position, targets[i].weight);
						} else if ( targets[i].target ) {
							outV = Vector3.Lerp(outV, targets[i].target.position, targets[i].weight);
						} else {
							outV = Vector3.Lerp(outV, this.transform.position, targets[i].weight);
						}
					}

					if ( !xPosition )
						outV.x = 0;
					if ( !yPosition )
						outV.y = 0;
					if ( !zPosition )
						outV.z = 0;
				}

				return outV;
			}

			if ( targets.Length > 0 ) {
				for ( int i = 0; i < targets.Length; i++ ) {
					if ( targets[i].specifyPositionInTargetParentSpace ) {
						outV = Vector3.Lerp(outV, this.transform.parent.InverseTransformPoint(targets[i].position), targets[i].weight);
					} else if ( targets[i].target ) {
						outV = Vector3.Lerp(outV, this.transform.parent.InverseTransformPoint(targets[i].target.position), targets[i].weight);
					} else {
						outV = Vector3.Lerp(outV, this.transform.localPosition, targets[i].weight);
					}
				}

				if ( !xPosition )
					outV.x = this.transform.localPosition.x;
				if ( !yPosition )
					outV.y = this.transform.localPosition.y;
				if ( !zPosition )
					outV.z = this.transform.localPosition.z;
			}

			return outV;
		}

		private Quaternion GetWeightedRotation() {
			var outV = this.transform.rotation;
			if ( targets.Length > 0 ) {
				for ( int i = 0; i < targets.Length; i++ ) {
					if ( targets[i].useSetRotation ) {
						outV = Quaternion.Slerp(outV, Quaternion.Euler(targets[i].rotation), targets[i].weight) * Quaternion.Euler(rotationalOffset);
					} else if ( targets[i].target ) {
						outV = Quaternion.Slerp(outV, targets[i].target.rotation, targets[i].weight) * Quaternion.Euler(rotationalOffset);
					}
				}
			}

			return outV;
		}

		// Update is called once per frame
		void LateUpdate() {
			if ( targets.Length > 0 ) {
				Vector3 pos;
				if ( maintainOffset ) {
					pos = GetWeightedPosition() + this.positionalOffset;
				} else {
					pos = GetWeightedPosition();
				}

				if ( rigigbodySweep ) {
					rigidbody.MovePosition(targets[0].target.parent.TransformPoint(pos));
				} else {
					this.transform.localPosition = pos;
				}

				if ( useRotation ) {
					this.transform.rotation = GetWeightedRotation();
				}
			}
		}
	}
}