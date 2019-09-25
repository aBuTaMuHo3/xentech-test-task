using System;
using ExerciseEngine.Model.ValueObjects.Interfaces;

namespace Memoflow.Model.ValueObjects
{
    public class MemoflowSolution : IRoundItem
    {
        public readonly MemoflowMatchType[] MatchTypes;

        public MemoflowSolution(MemoflowMatchType matchType)
        {
            MatchTypes = new MemoflowMatchType[] { matchType };
        }

        public MemoflowSolution(MemoflowMatchType[] matchTypes)
        {
            MatchTypes = matchTypes;
        }

        public override bool Equals(object obj)
        {
            if (obj is MemoflowSolution)
            {
                return (MemoflowSolution)obj == this;
            }
            else
            {
                return base.Equals(obj);
            }
        }

        public override int GetHashCode()
        {
            int result = 0;
            int shift = 0;
            for (int i = 0; i < MatchTypes.Length; i++)
            {
                shift = (shift + 11) % 21;
                result ^= ((int)MatchTypes[i] + 1024) << shift;
            }
            return result;
        }

        public static bool operator ==(MemoflowSolution a, MemoflowSolution b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }
            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            if (a.MatchTypes.Length != b.MatchTypes.Length)
            {
                return false;
            }
            // Return true if the fields match:
            bool allEqual = true;
            for (int i = 0; i < a.MatchTypes.Length; i++)
            {
                allEqual = allEqual && (Array.IndexOf(b.MatchTypes, a.MatchTypes[i]) != -1);
            }
            return allEqual;
        }

        public static bool operator !=(MemoflowSolution a, MemoflowSolution b)
        {
            return !(a == b);
        }
    }
}
