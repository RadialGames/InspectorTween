using UnityEngine;
using System.Collections;

public class LookAt : MonoBehaviour {
	public enum worldUpVectors {up,down,left,right,forward,backward,specified};
	public enum referenceSpace {world,root,parent,target};
	public referenceSpace space;
	public worldUpVectors up;
	public Transform target;
	public Vector3? position = null;
	private Renderer rend;
	public worldUpVectors lookAxis;
	public Transform upTarget;
	public float lookAheadBy;
	[Header("Update Limiter")]
	public bool limitUpdate;
	public float targetFPS = 30;
	protected float nextRenderTime;
	[Header("OnVisible")]
	public bool updateOnlyWhenVisible;
	Vector3 GetWorldUpVector()
	{
		Vector3 rVal = Vector3.zero;
		switch(lookAxis) {
			case(worldUpVectors.up):rVal = Vector3.up;break;
			case(worldUpVectors.down):rVal = -Vector3.up;break;
			case(worldUpVectors.left):rVal = -Vector3.right;break;
			case(worldUpVectors.right):rVal = Vector3.right;break;
			case(worldUpVectors.forward):rVal = Vector3.forward;break;
			case(worldUpVectors.backward):rVal = -Vector3.forward;break;
		}
		return rVal;
	}

	Vector3 GetUpVector(Transform trans)
	{
		Vector3 rVal = Vector3.zero;
		if(trans)
		{
			switch(up){
				case(worldUpVectors.up):rVal = trans.up;break;
				case(worldUpVectors.down):rVal = -trans.up;break;
				case(worldUpVectors.left):rVal = -trans.right;break;
				case(worldUpVectors.right):rVal = trans.right;break;
				case(worldUpVectors.forward):rVal = trans.forward;break;
				case(worldUpVectors.backward):rVal = -trans.forward;break;
				case (worldUpVectors.specified):
					if (upTarget) {
						rVal = (this.transform.position - upTarget.transform.position);
					} else { rVal = Vector3.up; }
					break;

			}
		}
		else{
			rVal = GetWorldUpVector();
		}
		return rVal;
	}

	Vector3 GetUpVector()
	{
		if(space == referenceSpace.world)
		{
			return GetWorldUpVector();
		}
		Transform spaceT=this.transform.parent;
		switch(space)
		{
		case referenceSpace.parent: spaceT = this.transform.parent; break;
		case referenceSpace.root: spaceT = this.transform.root; break;
		case referenceSpace.target : if(target)spaceT = this.target; break;
		};
		return GetUpVector(spaceT);
	}
	void Awake(){
		if (updateOnlyWhenVisible) {
			rend = GetComponent<Renderer>();
		}
	}
	// Update is called once per frame
	void Update () {
		if (updateOnlyWhenVisible && rend && !rend.isVisible) {
			return;
		}
		if (limitUpdate && targetFPS != 0) {
			if (Time.unscaledTime > nextRenderTime) {
				nextRenderTime = Time.unscaledTime + (1f / targetFPS);
			} else {
				return;
			}
		}

		Vector3 tpos = Vector3.zero;
		if (target) {
			tpos = target.position + (target.forward * lookAheadBy);
		} else if (position.HasValue) {
			tpos = position.Value;
		}
		this.transform.LookAt(tpos, GetUpVector());
	}


}
