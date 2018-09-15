// #Generic

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
namespace InspectorTween{
[AddComponentMenu("InspectorTween/TweenColor",4)]

public class TweenColor : TweenColorBase {
	public Gradient colorOverTime;
	public enum colorFunctions{Normal,Add,Multiply,Overlay,MultiplyAdd,Dodge};
	public colorFunctions colorFunction;
	public float colorOverTimeMultiplier = 1;


	protected float Overlay(float a, float b){
		return a<0.5f?2f*a*b:1-(2f*(1f-a)*(1f-b));
	}
	protected Color Overlay(Color a, Color b){
		return new Color(Overlay(a.r,b.r),Overlay(a.g,b.g),Overlay(a.b,b.b),Overlay(a.a,b.a));
	}
	Color ColorDodge(Color baseColor,Color overColor){
		for(int i=0;i<4;i++){
			baseColor[i] = baseColor[i] / (1f - Mathf.Min(0.99f,overColor[i]));
		}
		return baseColor;
	}
	protected override Color LerpColor(float lerp,Color initial) 
	{//Can add any : en.wikipedia.org/wiki/Blend_modes . Underlay? 
		Color var;
		
		if ( timeSettings.reverseValues ) {
			var = colorOverTime.Evaluate(1 - lerp) * colorOverTimeMultiplier;
		} else {
			var = colorOverTime.Evaluate(lerp)*colorOverTimeMultiplier;
		}
		switch(colorFunction){
			case colorFunctions.Normal : break;
			case colorFunctions.Multiply : var *= initial; break;
			case colorFunctions.Add : var += initial; break;
			case colorFunctions.Overlay : var = Overlay(var,initial); break;
			case colorFunctions.MultiplyAdd : var = initial +  (initial * var);break;
			case colorFunctions.Dodge : var = ColorDodge(initial,var);break;
		}
		return var;
	}
	protected override bool HasValidParameters()
	{
		return (  (colorOverTime.alphaKeys.Length>0 || colorOverTime.colorKeys.Length>0));
	}
}
}