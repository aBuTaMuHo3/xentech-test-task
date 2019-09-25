using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MinLibs.Utils
{
	public static class UriExtentsions
	{
		public static Dictionary<string, string> GetParams(this Uri uri)
		{
			return GetParams(uri.Query);
		}
		
		public static Dictionary<string, string> GetParams(string query)
		{
			var matches = Regex.Matches(query, @"[\?&](([^&=]+)=([^&=#]*))", RegexOptions.Compiled);
			return matches.Cast<Match>().ToDictionary(
				m => Uri.UnescapeDataString(m.Groups[2].Value),
				m => Uri.UnescapeDataString(m.Groups[3].Value)
			);
		}
	}
}