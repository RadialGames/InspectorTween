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
		public int GetNamedIndex(string name){
			int propInd = -1;
			for(int ind=0;ind<itemQueue.Length;ind++){
				if(itemQueue[ind].label.ToLower() == name.ToLower()){
					propInd = ind;
					break;
				}
			}
			return propInd;
		}
		public void Play(){
			Play (0);
		}
		public void Play(string label){
			Play (GetNamedIndex(label));
		}
		public void Play(int ind)
		{
			if(itemQueue.Length > ind)
			{
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
}