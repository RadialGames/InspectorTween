using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InspectorTween
{
    public class TweenAnchoredPosition : TweenBase
    {
        public Vector2 from;
        public Vector2 to;
        private RectTransform _rectTransform;

        protected override void Awake()
        {
            base.Awake();
            _rectTransform = (RectTransform)transform;
        }

        protected override void LerpParameters(float lerp)
        {
            _rectTransform.anchoredPosition = Vector2.Lerp(from, to, lerp);
        }
    }
}