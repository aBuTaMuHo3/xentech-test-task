using System;
namespace SynaptikonFramework.Util
{
    public static class FloatExtension
    {
        public static float GetEpsilon(this float fl)
        {
            return 1E-5f;
        }
    }
}
