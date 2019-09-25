using System.Collections.Generic;
using MinLibs.Signals;
using MinLibs.Utils;

namespace MinLibs.MVC
{
	public abstract class MediatingBehaviour : MediatedBehaviour, IMediatingBehaviour
	{
		HashSet<IMediated> waitingForMediation;
		Signal<IMediated> onMediate;
		Signal<IMediated> onPostponeMediating;
		
		public Signal<IMediated> OnMediate {
			get {
				return onMediate ?? GetPostponeSignal();
			}
			set {
				onMediate = value;
				ResolveWaitingQueue();
			}
		}

		Signal<IMediated> GetPostponeSignal ()
		{
			if (onPostponeMediating == null) {
				waitingForMediation = new HashSet<IMediated>();
				onPostponeMediating = new Signal<IMediated>();
				onPostponeMediating.AddListener(AddToWaitingList);
			}

			return onPostponeMediating;
		}

		private void ResolveWaitingQueue ()
		{
			if (waitingForMediation != null) {
				waitingForMediation.Each(mediated => onMediate.Dispatch(mediated));
				waitingForMediation = null;
				onPostponeMediating.RemoveAllListeners();
				onPostponeMediating = null;
			}
		}

		private void AddToWaitingList (IMediated mediated)
		{
			waitingForMediation.Add(mediated);
		}
	}
}
