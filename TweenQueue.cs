using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace InspectorTween{
	[System.Serializable]
	public class TweenQueueItem{
		public string label;
		public TweenBase[] tweens = new TweenBase[1];
		public bool reverse;
		public bool reverseValues;
		public float timeLengthOverride = -1f;
		public bool setInitialTransforms;
		
		public UnityEvent onPlay;
		[Header("Must enable 'watch for end' to work")]
		public UnityEvent onEnd;

		public bool test;
	}
	[AddComponentMenu("InspectorTween/TweenQueue",8)]
	[HelpURL("https://github.com/RadialGames/InspectorTween/wiki/Tween-Queue")]
	public class TweenQueue : MonoBehaviour {
		public bool watchForEnd;
		private int currentlyPlaying = -1;
		
		public TweenQueueItem[] itemQueue = new TweenQueueItem[1]{new TweenQueueItem()};
		public int tweenToPlay = 0;//for editor script

		
		/// <summary>
		/// Call CancelTween on all tweens in all queues
		/// </summary>
		public void CancelAll() {
			CancelAll(false);
        }
		
        public void CancelAll(bool doInterupt){
			for(int ind=0;ind<itemQueue.Length;ind++){
				foreach(TweenBase tween in itemQueue[ind].tweens){
					if(tween && tween.enabled){
						tween.CancelTween(doInterupt);
					}
				}
			}
		}
		/// <summary>
		/// Cancel all tweens on specified queue item.
		/// </summary>
		/// <param name="queueIndex">index of queue. use GetNamedIndex to find index by name</param>
		/// <param name="doInterupt"></param>
		public void Cancel(int queueIndex,bool doInterupt=false) {
			if ( queueIndex > itemQueue.Length || queueIndex < 0 ) {
				return;
			}
			foreach(TweenBase tween in itemQueue[queueIndex].tweens){
				if(tween != null && tween.enabled){
					tween.CancelTween(doInterupt);
				}
			}
		}
		/// <summary>
		/// Set overide time on specified queue item
		/// </summary>
		/// <param name="index"></param>
		public void SetOverrideTime(int index, float time) {
			itemQueue[index].timeLengthOverride = time;
		}
		public void SetOverrideTime(float time) {
			for ( int i = 0; i < itemQueue.Length; i++ ) {
				SetOverrideTime(i,time);
			}
		}
		public int GetNamedIndex(string inStr){
			int propInd = -1;
			for(int ind=0;ind<itemQueue.Length;ind++){
				if(string.Compare(itemQueue[ind].label,inStr,true) == 0){
					propInd = ind;
					break;
				}
			}
			return propInd;
		}

		public void ToggleReverseValues(int ind) {
			if ( itemQueue.Length > ind ) {
				itemQueue[ind].reverseValues = !itemQueue[ind].reverseValues;
			}
		}
		public void Play(){
			Play (0);
		}
		public void Play(string label){
			Play (GetNamedIndex(label));
		}
		public void Play(int queueIndex) {
			if (ValidQueueIndex(queueIndex) == false ) {
				return;
			}


			foreach(TweenBase tweenI in itemQueue[queueIndex].tweens){
				if(itemQueue[queueIndex].setInitialTransforms){
					if(tweenI.GetType().IsSubclassOf(typeof(InspectorTween.TweenTransform))){
						((TweenTransform)tweenI).SetInitial();
					}
				}
				if(tweenI == null) continue;
				if(itemQueue[queueIndex].timeLengthOverride != -1){
					tweenI.time = ( itemQueue[queueIndex].timeLengthOverride );
				}

				tweenI.ReverseValues(itemQueue[queueIndex].reverseValues);

				if(itemQueue[queueIndex].reverse){
					tweenI.PlayReverse(true);
				}
				else{
					tweenI.PlayForwards(true);
				}
			}
			itemQueue[queueIndex].onPlay.Invoke();
			currentlyPlaying = queueIndex;
			this.enabled = true;
		}
		


		public void PlayReverse(int queueIndex) {
			if (ValidQueueIndex(queueIndex) == false ) {
				return;
			}
			foreach(TweenBase tweenI in itemQueue[queueIndex].tweens){
				if(itemQueue[queueIndex].setInitialTransforms){
					if(tweenI.GetType().IsSubclassOf(typeof(InspectorTween.TweenTransform))){
						((TweenTransform)tweenI).SetInitial();
					}
				}
				if(tweenI == null) continue;
				if(itemQueue[queueIndex].timeLengthOverride != -1){
					tweenI.time = ( itemQueue[queueIndex].timeLengthOverride );
				}
				tweenI.ReverseValues(itemQueue[queueIndex].reverseValues);
				tweenI.PlayReverse(true);

			}
			itemQueue[queueIndex].onPlay.Invoke();
			currentlyPlaying = queueIndex;
			this.enabled = true;
		}

		
		private int setToQueueIndex = 0;
		/// <summary>
		/// Can only have one input for event functions, so to SetToLerpPoint, call method to set index, then call with value.
		/// </summary>
		/// <param name="ind"></param>
		public void SetToLerpPointActiveIndex(int ind) {
			setToQueueIndex = ind;
		}
		public void SetToLerpPoint( float val) {
			SetToLerpPoint(setToQueueIndex,val);
		}
		/// <summary>
		/// Test if specified queue index is in the valid range.
		/// </summary>
		/// <param name="queueIndex"></param>
		/// <returns></returns>
		private bool ValidQueueIndex(int queueIndex) {
			return (queueIndex < 0 || itemQueue.Length <= queueIndex || itemQueue[queueIndex].tweens == null) == false;
		}
		public void SetToLerpPoint(int queueIndex, float val) {
			//Debug.Log($"Queue SetToLerpPoint - ind {queueIndex} val :{val} ");
			if ( ValidQueueIndex(queueIndex) == false ) {
				return;
			}
			foreach ( TweenBase tweenI in itemQueue[queueIndex].tweens ) {
				if ( tweenI == null ) {
					continue;
				}
				tweenI.SetToLerpPoint(val);
			}
		}
		/// <summary>
		/// Find the MAXIMUM runtime of the specified queue. Some parameters affect a tweens length randomly.
		/// </summary>
		/// <param name="queueIndex"></param>
		/// <returns>time in seconds</returns>
		public float GetQueuePlayTime(int queueIndex) {
			float length = 0f;
			if ( ValidQueueIndex(queueIndex) == false ) {
				return length;
			}
			
			foreach ( TweenBase tween in itemQueue[queueIndex].tweens ) {
				if ( tween == null ) {
					continue;
				}
				
				float maxRuntime = tween.time * Mathf.Max(tween.timeSettings.timeRandomScale.x, tween.timeSettings.timeRandomScale.y);
				float delay = 0;
				if ( tween.timeSettings.randomStartDelay.x > 0 || tween.timeSettings.randomStartDelay.y > 0 ) {
					delay = Mathf.Max(tween.timeSettings.randomStartDelay.x, tween.timeSettings.randomStartDelay.y);
				} else {
					delay = tween.timeSettings.startDelay;//this is 0 if not used,
				}

				maxRuntime += delay;
				length = Mathf.Max(length,maxRuntime);
			}

			return length;
		}
		private void OnEnable() {
			if ( currentlyPlaying < 0 ) {
				enabled = false;//only stay enabled if invoking a queue item.
			}
		}

		private void Update() {
			if ( !watchForEnd ) {
				this.enabled = false;
			}

			if ( currentlyPlaying < 0 ) {
				return;
			}

			TweenQueueItem current = itemQueue[currentlyPlaying];
			foreach ( TweenBase currentTween in current.tweens ) {
				if ( currentTween != null && currentTween.enabled && (currentTween.interpolation.loop == false || currentTween.interpolation.loopNumberOfTimes > 0) ) {
					return; //don't do anything if something's playing.
				}
			}
			current.onEnd?.Invoke();
			//if we get here without returning everything should be finished.
			currentlyPlaying = -1;//stop Watching
			this.enabled = false;
		}
	}
}