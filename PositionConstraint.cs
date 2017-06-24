using UnityEngine;
using System.Collections;
using UnityEngine.Events;
//[ExecuteInEditMode]
public class PositionConstraint : MonoBehaviour {
	public float lag;
	public float rotLag;
	public bool applyRotation;
	[System.Serializable]
	public class ConstraintPositionTarget{
		public Transform target;
		public float weight = 1f;
	};

	public ConstraintPositionTarget[] targets = new ConstraintPositionTarget[] {new ConstraintPositionTarget()};
	protected Vector3 smoothDampVel = Vector3.zero;
	protected Vector3 smoothDampRotVel = Vector3.zero;
	[Tooltip("Event to fire if target 0 is lost")]//Should fix to be all targets.
	public UnityEvent onTargetLoss;
	private Vector3 GetWeightedPosition()
	{
		//guaranteed to have length > 0.
		if(targets[0].target == null && onTargetLoss.GetPersistentEventCount() > 0) {
			onTargetLoss.Invoke();
		}
		Vector3 outV =  targets[0].target != null ? targets[0].target.position : this.transform.position;
		for(int i=1 ; i < targets.Length;i++)
		{
			if(targets[i].target != null)
			{
				outV = Vector3.Lerp(outV, targets[i].target.position,targets[i].weight);
			}
			else{ //null target  == self.
				outV = Vector3.Lerp(outV, this.transform.position,targets[i].weight);
			}
		}
		return outV;
	}

	private Quaternion GetWeightedRotation() {
		//guaranteed to have length > 0.
		Quaternion outV = targets[0].target != null ? targets[0].target.rotation : this.transform.rotation;
		for (int i = 1; i < targets.Length; i++) {
			if (targets[i].target != null) {
				outV = Quaternion.Slerp(outV, targets[i].target.rotation, targets[i].weight);
			} else { //null target  == self.
				outV = Quaternion.Slerp(outV, this.transform.rotation, targets[i].weight);
			}
		}
		return outV;
	}


	// Update is called once per frame
	void LateUpdate () {
		if(targets.Length > 0)
		{
			Vector3 toPosition = GetWeightedPosition();
			Quaternion toRotation = GetWeightedRotation();
			if(lag > 0f){
				toPosition = Vector3.SmoothDamp(transform.position, toPosition, ref smoothDampVel, lag);
				toRotation = Quaternion.Slerp(transform.rotation, toRotation, rotLag);
			}
			this.transform.position = toPosition;
			if (applyRotation) {
				this.transform.rotation = toRotation;
			}
		}
	}
}
