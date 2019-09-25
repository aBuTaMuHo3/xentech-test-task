using System.Collections.Generic;
using UnityEngine;

namespace WebExercises.Runner
{
	public interface IBundles
	{
		void Store(string id, AssetBundle bundle);

		bool Has(string id);
		
		void Remove(BundleInfo info);

		void Remove(IEnumerable<BundleInfo> infoList);
	}

	public class Bundles : IBundles
	{
		private readonly IDictionary<string, AssetBundle> _bundles = new Dictionary<string, AssetBundle>();
		
		public void Store(string id, AssetBundle bundle)
		{
			_bundles[id] = bundle;
		}

		public bool Has(string id)
		{
			return _bundles.ContainsKey(id);
		}

		public void Remove(IEnumerable<BundleInfo> infoList)
		{
			foreach (var info in infoList)
			{
				Remove(info);
			}
		}

		public void Remove(BundleInfo info)
		{
			var id = info.Id;
			
			if (!info.Cache && Has(id))
			{
				_bundles[id].Unload(true);
				_bundles.Remove(id);
			}
		}
	}
}