﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace InspectorTween {
	[AddComponentMenu("InspectorTween/TweenSprite",10)]
	public class TweenSprite : TweenBase {
	
		
		public Component target;
		public Sprite[] sprites = new Sprite[2];

		private Image image;
		private SpriteRenderer spriteRenderer;
		private new ParticleSystem particleSystem;
		
		public enum objectType {None,Sprite,Image,Particle,Material };
		protected objectType type;
		
		protected override void LerpParameters(float lerp) {
			int index = Mathf.Min(sprites.Length-1,Mathf.FloorToInt(lerp * sprites.Length));
			Sprite currentSprite = sprites[index];
			switch ( type ) {
				case objectType.None:
					break;
				case objectType.Sprite:
					spriteRenderer.sprite = currentSprite;
					break;
				case objectType.Image:
					image.sprite = currentSprite;
					break;
				case objectType.Particle:
					var shapeModule = particleSystem.shape;
					shapeModule.sprite = currentSprite;
					//particleSystem.shape = shapeModule;
					break;
				case objectType.Material:
					throw new NotImplementedException();
				default:
					throw new ArgumentOutOfRangeException();
			}
		}


		protected override bool HasValidParameters()
		{
				return (interpolation.interpolation.length>0 && (sprites.Length>0) && base.HasValidParameters() );
		}
		protected override void Awake() {
			if ( target == null ) {
				target = this;
			}
			base.Awake();
			if (!HasValidParameters()) {
				Debug.LogWarning("Parameter validation error : " + name + " ");
			}
			image = target.GetComponent<Image>();
			if ( image != null ) {
				type = objectType.Image;
				return;
			}
			spriteRenderer = target.GetComponent<SpriteRenderer>();
			if ( spriteRenderer != null ) {
				type = objectType.Sprite;
				renderer = spriteRenderer;
				return;
			}

			particleSystem = target.GetComponent<ParticleSystem>();
			if ( particleSystem != null ) {
				type = objectType.Particle;
				//return;
			}
			


		}
	}
}