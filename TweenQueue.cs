using UnityEngine;
using System.Collections;
namespace InspectorTween{
	[System.Serializable]
	public class TweenQueueItem{
		public string label;
		public TweenBase[] tweens = new TweenBase[1];
		public bool reverse;
		public bool reverseValues;
		public float timeLengthOverride = -1f;
		public bool setInitialTransforms;
	}
	[AddComponentMenu("InspectorTween/TweenQueue",8)]
	public class TweenQueue : MonoBehaviour {
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
				foreach(var tween in itemQueue[ind].tweens){
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
			foreach(var tween in itemQueue[queueIndex].tweens){
				if(tween && tween.enabled){
					tween.CancelTween(doInterupt);
				}
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
			foreach(var tweenI in itemQueue[ind].tweens){
				if(itemQueue[ind].setInitialTransforms){
					if(tweenI.GetType().IsSubclassOf(typeof(InspectorTween.TweenTransform))){
						((InspectorTween.TweenTransform)tweenI).SetInitial();
					}
				}
				if(tweenI == null) continue;
				if(itemQueue[ind].timeLengthOverride != -1){
					tweenI.timeSettings.time = itemQueue[ind].timeLengthOverride;
				}

				tweenI.timeSettings.reverseValues = itemQueue[ind].reverseValues;

				if(itemQueue[ind].reverse){
					tweenI.PlayReverse();
				}
				else{
					tweenI.PlayForwards();
				}
			}
		}
		
	}
}