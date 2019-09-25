using UnityEditor;
using UnityEngine;

namespace WebExercises.Editor
{
	[CreateAssetMenu(menuName = "Exercises/Create PublishSettings", order = 0)]
	public class PublishSettings : ScriptableObject
	{
		[Header("Build")]
		public bool development;
		public bool allowDebugging;
		public bool connectWithProfiler;
		public bool compressWithLz4;
		[Header("Publish")]
		public int memorySize;
		public WebGLExceptionSupport exceptionSupport;
		public WebGLCompressionFormat compressionFormat;
		public WebGLLinkerTarget linkerTarget;
		public bool debugSymbols;
		public bool dataCaching;
		public ManagedStrippingLevel strippingLevel;
	}
}