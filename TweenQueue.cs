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
		public void Play(int ind) {
			if ( ind < 0 || itemQueue.Length <= ind || itemQueue[ind].tweens == null ) {
				return;
			}


			foreach(TweenBase tweenI in itemQueue[ind].tweens){
				if(itemQueue[ind].setInitialTransforms){
					if(tweenI.GetType().IsSubclassOf(typeof(InspectorTween.TweenTransform))){
						((TweenTransform)tweenI).SetInitial();
					}
				}
				if(tweenI == null) continue;
				if(itemQueue[ind].timeLengthOverride != -1){
					tweenI.time = ( itemQueue[ind].timeLengthOverride );
				}

				tweenI.ReverseValues(itemQueue[ind].reverseValues);

				if(itemQueue[ind].reverse){
					tweenI.PlayReverse(true);
				}
				else{
					tweenI.PlayForwards(true);
				}
			}
			itemQueue[ind].onPlay.Invoke();
			currentlyPlaying = ind;
			this.enabled = true;
		}
		


		public void PlayReverse(int ind) {
			if ( ind < 0 || itemQueue.Length <= ind || itemQueue[ind].tweens == null ) {
				return;
			}
			foreach(TweenBase tweenI in itemQueue[ind].tweens){
				if(itemQueue[ind].setInitialTransforms){
					if(tweenI.GetType().IsSubclassOf(typeof(InspectorTween.TweenTransform))){
						((TweenTransform)tweenI).SetInitial();
					}
				}
				if(tweenI == null) continue;
				if(itemQueue[ind].timeLengthOverride != -1){
					tweenI.time = ( itemQueue[ind].timeLengthOverride );
				}
				tweenI.ReverseValues(itemQueue[ind].reverseValues);
				tweenI.PlayReverse(true);

			}
			itemQueue[ind].onPlay.Invoke();
			currentlyPlaying = ind;
			this.enabled = true;
		}
		public void SetToLerpPoint( float val) {
			foreach ( TweenBase tweenI in itemQueue[0].tweens ) {
				if ( tweenI == null ) {
					continue;
				}
				tweenI.SetToLerpPoint(val);
			}
		}
		public void SetToLerpPoint(int queueIndex, float val) {
			if ( queueIndex < 0 || itemQueue.Length <= queueIndex || itemQueue[queueIndex].tweens == null ) {
				return;
			}
			foreach ( TweenBase tweenI in itemQueue[queueIndex].tweens ) {
				if ( tweenI == null ) {
					continue;
				}
				tweenI.SetToLerpPoint(val);
			}
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
				if ( currentTween.enabled ) {
					return; //don't do anything if something's playing.
				}
			}
			//if we get here without returning everything should be finished.
			currentlyPlaying = -1;//stop Watching
			current.onEnd?.Invoke();
		}
	}
}