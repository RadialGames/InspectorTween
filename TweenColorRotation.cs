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

	new void Awake(){
		base.Awake();

		sprite = GetComponent<SpriteRenderer>();
		if(sprite){
			type = objectType.Sprite;
			initialColor = sprite.color;
			return;
		}
		image = GetComponent<Graphic>();//Image,Text,RawImage...
		if(image){
			type = objectType.Graphic;
			this.updateSettings.pauseOffscreen = VisibilityPause.None; //no renderer on these?
			initialColor = image.color;
			return;
		}
		psys = GetComponent<ParticleSystem>();
		if(psys){
			type = objectType.Particle;
#if UNITY_5_6_OR_NEWER
				initialColor = psys.main.startColor.color;
#else
				initialColor = psys.startColor;
#endif
			return;
		}
	}
	public static Matrix4x4 YIQ (Vector3 hsv)
	{
		Matrix4x4 inMatrix =  Matrix4x4.identity;
		inMatrix.SetRow(0,new Vector4(0.299f, 0.587f, 0.114f, 0f));
		inMatrix.SetRow(1,new Vector4(0.596f, -0.274f, -0.321f,0f));
		inMatrix.SetRow(2,new Vector4(0.211f, -0.523f, 0.311f, 0f));
		
		Matrix4x4 adjustMatrix = Matrix4x4.identity;
		float VSU = hsv.z * hsv.y * Mathf.Cos(hsv.x*Mathf.PI/180f);
		float VSW = hsv.z * hsv.y * Mathf.Sin(hsv.x*Mathf.PI/180f);
		adjustMatrix.SetRow(0,new Vector4(hsv.z, 0f,0f,0f));
		adjustMatrix.SetRow(1,new Vector4(0f, VSU, VSW,0f));
		adjustMatrix.SetRow(2,new Vector4(0f, -VSW, VSU,0f));
		
		Matrix4x4 outMatrix =  Matrix4x4.identity;
		outMatrix.SetRow(0,new Vector4(0.99966f,  0.95654f, 0.62086f,0f));
		outMatrix.SetRow(1,new Vector4(0.99962f, -0.27227f,-0.64745f,0f)); 
		outMatrix.SetRow(2,new Vector4(1.00281f, -1.10685f, 1.70541f,0f));
		
		var cMatrix = (inMatrix * adjustMatrix) * outMatrix;
		return cMatrix;//(Color)(cMatrix*(Vector4)col);
	}
	public static Color YUV (Color initial,Vector3 hsv)
	{
		Matrix4x4 inMatrix =  Matrix4x4.identity;
		inMatrix.SetRow(0,new Vector4(0.299f, 0.587f, 0.114f, 0f));
		inMatrix.SetRow(1,new Vector4(-0.14713f, -0.274f, 0.436f,0f));
		inMatrix.SetRow(2,new Vector4(0.615f, -0.51499f, -0.10001f, 0f));
		

		Matrix4x4 outMatrix =  Matrix4x4.identity;
		outMatrix.SetRow(0,new Vector4(1f,  0f, 1.13983f,0f));
		outMatrix.SetRow(1,new Vector4(1f, -0.39465f,-0.58060f,0f)); 
		outMatrix.SetRow(2,new Vector4(1f, 2.03211f, 0f,0f));
		
		var cMatrix =  outMatrix * Vector4.Scale((inMatrix * (Vector4)initial) + (Vector4)hsv,new Vector4(1f,0.5f,0.5f,1f));
		return (Color)cMatrix;//(Color)(cMatrix*(Vector4)col);
	}


	protected Color SetSaturation(Color inCol,float lerp){

		var colorLum = Vector3.Dot (new Vector3(0.22f, 0.707f, 0.071f),new Vector3(inCol.r,inCol.g,inCol.b));
		return MathS.ColorLerpUnclamped( new Color(colorLum,colorLum,colorLum,inCol.a),inCol,lerp);
	}
	protected override Color LerpColor(float lerp)
	{
		var lerpedVector = LerpArray(hsvValues,lerp,Vector3.Lerp);
		Color var;
		switch(colorSpace){
		case ColorSpaces.HSV_YIQ : var = (Color)(YIQ(lerpedVector)*(Vector4)initialColor); break;
		case ColorSpaces.YUV_BlendExperiment : var = (Color)(YUV(initialColor,lerpedVector)*(Vector4)initialColor); break;
		case ColorSpaces.SaturationOnly : var = SetSaturation(initialColor,lerpedVector.y);break;
		default : var = initialColor; break;
		}
		return var;
	}
	protected override bool HasValidParameters()
	{
		if(useMaterial){
			return mat.HasProperty(propID);
		}
		return (base.HasValidParameters());
	}
	protected override void LerpParameters(float lerp)
	{
		if(useMaterial){
			var val = LerpColor(lerp);
			mat.SetColor(propID,val);
		}
		else{
			base.LerpParameters(lerp);
		}
	}

}
}