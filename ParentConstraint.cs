using UnityEngine;
using System.Collections;
//[ExecuteInEditMode]
public class ParentConstraint : MonoBehaviour {

	[System.Serializable]
	public class ConstraintTarget{
		public Transform target;
		public bool usePosition;
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

	void Awake()
	{
		if(targets.Length > 0)
		{
			if(maintainOffset)
			{
				InitializePositionalOffset();
			}
		}
	}

	public void InitializePositionalOffset(){
		positionalOffset = this.transform.localPosition - GetWeightedPosition();
	}
	public void SetTarget(Transform targ, int indx,float weight){
		if(indx < targets.Length){
			targets[indx].target = targ;
			targets[indx].weight = weight;
			targets[indx].usePosition = false;
		}
	}
	public void SetPosition(Vector3 worldPosition,int indx,float weight){
		if(indx < targets.Length){
			targets[indx].target = null;
			targets[indx].weight = weight;
			targets[indx].position = worldPosition;
			targets[indx].usePosition = true;
		}
	}
	public void SetTargetWeight(int indx, float weight){
		if(indx < targets.Length){
			targets[indx].weight = weight;
		}
	}

	private Vector3 GetWeightedPosition()
	{
		var outV = this.transform.localPosition;
		if(targets.Length > 0)
		{
			//if(targets[0].usePosition){
			//	outV = this.transform.parent.InverseTransformPoint(targets[0].position);
			//}
			//else if(targets[0].target != null){
			//	outV = this.transform.parent.InverseTransformPoint(targets[0].target.position);
			//}
			for(int i=0;i < targets.Length;i++)
			{
				if(targets[i].usePosition){
					outV = Vector3.Lerp(outV, this.transform.parent.InverseTransformPoint(targets[i].position),targets[i].weight);
				}else if(targets[i].target)
				{
					outV = Vector3.Lerp(outV, this.transform.parent.InverseTransformPoint(targets[i].target.position),targets[i].weight);
				}
				else{
					outV = Vector3.Lerp(outV, this.transform.localPosition,targets[i].weight);
				}
			}
			if(!xPosition) outV.x = this.transform.localPosition.x;
			if(!yPosition) outV.y = this.transform.localPosition.y;
			if(!zPosition) outV.z = this.transform.localPosition.z;

		}
		return outV;
	}

	private Quaternion GetWeightedRotation()
	{
		var outV = this.transform.rotation;
		if(targets.Length > 0)
		{
			for(int i=0;i < targets.Length;i++)
			{
				if(targets[i].useSetRotation){
					outV = Quaternion.Slerp(outV,Quaternion.Euler(targets[i].rotation),targets[i].weight) * Quaternion.Euler(rotationalOffset);
				}
				else if(targets[i].target)
				{
					outV = Quaternion.Slerp(outV,targets[i].target.rotation,targets[i].weight) * Quaternion.Euler(rotationalOffset);
				}
			}
		}
		return outV;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(targets.Length > 0)
		{
			if(maintainOffset)
			{
				this.transform.localPosition = GetWeightedPosition() + this.positionalOffset;
			}
			else
			{
				this.transform.localPosition = GetWeightedPosition();
			}
			if(useRotation){
				this.transform.rotation = GetWeightedRotation();
			}
		}
	}
}
