// #Generic

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
namespace InspectorTween{
[AddComponentMenu("InspectorTween/TweenColorRotation",8)]
public class TweenColorRotation : TweenColorBase{
	
	public enum ColorSpaces {HSV_YIQ,SaturationOnly,YUV_BlendExperiment}
	public ColorSpaces colorSpace;
	public Vector3[] hsvValues = new Vector3[2]{new Vector3(0f,1f,1f),new Vector3(360,1,1)};
	public bool setMatrix;

	
	public new void Awake(){
		base.Awake();
		propID = Shader.PropertyToID(materialProperty);
	}

	protected override void Reset() {
		base.Reset();
		//Initialize for shader based rendering.
		updateSettings = new UpdateInterface {pauseOffscreen = VisibilityPause.None};
		useMaterial = true;
		setMatrix = true;
		materialProperty = "_MatrixYIQ";
	}

	private static readonly Matrix4x4 matrixYIQtoRGB = new Matrix4x4 {
		m00 = 0.299f,
		m01 = 0.587f,
		m02 = 0.114f,
		m03 = 0,
		m10 = 0.596f,
		m11 = -0.274f,
		m12 = -0.321f,
		m13 = 0,
		m20 = 0.211f,
		m21 = -0.523f,
		m22 = 0.311f,
		m23 = 0,
		m30 = 0f,
		m31 = 0f,
		m32 = 0f,
		m33 = 1
	};
	private static readonly Matrix4x4 matrixRGBtoYIQ = new Matrix4x4 {
		m00 = 0.99966f,
		m01 = 0.95654f,
		m02 = 0.62086f,
		m03 = 0,
		m10 = 0.99962f,
		m11 = -0.27227f,
		m12 = -0.64745f,
		m13 = 0,
		m20 = 1.00281f,
		m21 = -1.10685f,
		m22 = 1.70541f,
		m23 = 0,
		m30 = 0f,
		m31 = 0f,
		m32 = 0f,
		m33 = 1
	};
	private static Matrix4x4 adjustMatrix = Matrix4x4.identity;
	
	public static Matrix4x4 YIQ (Vector3 hsv)
	{
		float VSU = hsv.z * hsv.y * Mathf.Cos(hsv.x*Mathf.Deg2Rad);
		float VSW = hsv.z * hsv.y * Mathf.Sin(hsv.x*Mathf.Deg2Rad);
		adjustMatrix.m00 = hsv.z;
		//adjustMatrix.m01 = 0;
		//adjustMatrix.m02 = 0f;
		//adjustMatrix.m03 = 0;
		
		//adjustMatrix.m10 = 0f;
		adjustMatrix.m11 = VSU;
		adjustMatrix.m12 = -VSW;
		//adjustMatrix.m13 = 0;
		
		//adjustMatrix.m20 = 0f;
		adjustMatrix.m21 = VSW;
		adjustMatrix.m22 = VSU;
		//adjustMatrix.m23 = 0f;
		
		//adjustMatrix.m30 = 0f;
		//adjustMatrix.m31 = 0f;
		//adjustMatrix.m32 = 0f;
		//adjustMatrix.m33 = 1;

		
		Matrix4x4 cMatrix = (matrixRGBtoYIQ * adjustMatrix) * matrixYIQtoRGB;
		return cMatrix;
	}
	public static Color YUV (Color initial,Vector3 hsv)
	{
		Matrix4x4 inMatrix;// = new Matrix4x4();// =  Matrix4x4.identity;
		inMatrix.m00 = 0.299f;
		inMatrix.m01 = 0.587f;
		inMatrix.m02 = 0.114f;
		inMatrix.m03 = 0;
		inMatrix.m10 = -0.14713f;
		inMatrix.m11 = -0.274f;
		inMatrix.m12 = 0.436f;
		inMatrix.m13 = 0;
		inMatrix.m20 = 0.615f;
		inMatrix.m21 = -0.51499f;
		inMatrix.m22 = -0.10001f;
		inMatrix.m23 = 0;
		inMatrix.m30 = 0f;
		inMatrix.m31 = 0f;
		inMatrix.m32 = 0f;
		inMatrix.m33 = 1;
		//inMatrix.SetRow(0,new Vector4(0.299f, 0.587f, 0.114f, 0f));
		//inMatrix.SetRow(1,new Vector4(-0.14713f, -0.274f, 0.436f,0f));
		//inMatrix.SetRow(2,new Vector4(0.615f, -0.51499f, -0.10001f, 0f));
		

		Matrix4x4 outMatrix;// = new Matrix4x4();// =  Matrix4x4.identity;
		outMatrix.m00 = 1;
		outMatrix.m01 = 0f;
		outMatrix.m02 = 1.13983f;
		outMatrix.m03 = 0;
		outMatrix.m10 = 1f;
		outMatrix.m11 = -0.39465f;
		outMatrix.m12 = -0.58060f;
		outMatrix.m13 = 0;
		outMatrix.m20 = 1f;
		outMatrix.m21 = 2.03211f;
		outMatrix.m22 = 0f;
		outMatrix.m23 = 0;
		outMatrix.m30 = 0f;
		outMatrix.m31 = 0f;
		outMatrix.m32 = 0f;
		outMatrix.m33 = 1;
		//outMatrix.SetRow(0,new Vector4(1f,  0f, 1.13983f,0f));
		//outMatrix.SetRow(1,new Vector4(1f, -0.39465f,-0.58060f,0f)); 
		//outMatrix.SetRow(2,new Vector4(1f, 2.03211f, 0f,0f));
		
		Vector4 cMatrix =  outMatrix * Vector4.Scale((inMatrix * (Vector4)initial) + (Vector4)hsv,new Vector4(1f,0.5f,0.5f,1f));
		return (Color)cMatrix;//(Color)(cMatrix*(Vector4)col);
	}


	protected Color SetSaturation(Color inCol,float lerp){

		var colorLum = Vector3.Dot (new Vector3(0.22f, 0.707f, 0.071f),new Vector3(inCol.r,inCol.g,inCol.b));
		return MathS.ColorLerpUnclamped( new Color(colorLum,colorLum,colorLum,inCol.a),inCol,lerp);
	}
	protected override Color LerpColor(float lerp,Color initial)
	{
		Vector3 lerpedVector = LerpArray(hsvValues,lerp,Vector3.Lerp);
		Color var;
		switch(colorSpace){
		case ColorSpaces.HSV_YIQ : var = (Color)(YIQ(lerpedVector)*(Vector4)initial); break;
		case ColorSpaces.YUV_BlendExperiment : var = (Color)(YUV(initial,lerpedVector)*(Vector4)initial); break;
		case ColorSpaces.SaturationOnly : var = SetSaturation(initial,lerpedVector.y);break;
		default : var = initial; break;
		}
		return var;
	}
	protected override bool HasValidParameters() {
		bool hasMat = mat != null;
		if(hasMat && useMaterial) {
			bool hasProperty = mat.HasProperty(propID);
			if ( !hasProperty ) {//For some reason this is incorrectly returning false???
				//Debug.LogWarning("material doesn't have specified property : " + propID);
			}
			//return hasProperty && base.HasValidParameters();
		} else if(useMaterial) {
			Debug.LogWarning("no material at this stage");
		}
		return (base.HasValidParameters());
	}
	protected override void LerpParameters(float lerp)
	{
		if(useMaterial){
			Color val = LerpColor(lerp,initialColor);
			if ( setMatrix ) {
				Vector3 lerpedVector = LerpArray(hsvValues,lerp,Vector3.Lerp);
				Matrix4x4 matrix = YIQ(lerpedVector);
				//Debug.Log(matrix);
				mat.SetMatrix(propID,matrix);
			} else {
				if ( mat != null ) {
					mat.SetColor(propID,val);
				} else {
					Debug.LogError("no material on tween");
				}

			}

		}
		else{
			base.LerpParameters(lerp);
		}
	}

}
}