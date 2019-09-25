using UnityEngine;

namespace MinLibs.Utils
{
	public interface IPlatform
	{
		string URL { get; }
		bool IsEditor { get; }
		string DataPath { get; }
        int AntiAliasing { get; set; }
		int EnableLogging { get; set; }
		bool CaptureInput { set; }
	}

	public class UnityPlatform : IPlatform
	{
		public string URL { get; } = Application.absoluteURL;
		public bool IsEditor => Application.isEditor;
		public string DataPath => Application.dataPath;
		
		public int AntiAliasing
		{
			get { return QualitySettings.antiAliasing; }
			set { QualitySettings.antiAliasing = value; }
		}

        public int EnableLogging { get; set; }

        public bool CaptureInput
		{
			set
			{
#if UNITY_WEBGL && UNITY_EDITOR
				//UnityEngine.WebGLInput.captureAllKeyboardInput = value;
#endif
			}
		}
	}
}