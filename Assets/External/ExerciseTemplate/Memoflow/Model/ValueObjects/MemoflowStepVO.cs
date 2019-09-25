using System;
using System.Collections.Generic;
using ExerciseEngine.Model.Enum;
using ExerciseEngine.Model.ValueObjects.Interfaces;
using SynaptikonFramework.View.Interfaces;

namespace Memoflow.Model.ValueObjects
{
    public class MemoflowStepVO : IRoundItem, ILinearSliderData
    {
        public readonly float Rot;
        public AvailableColors SymbolColor { get; set; }
        public readonly int SymbolId;
        public readonly AvailableColors BorderColor;

        public MemoflowStepVO(int symbol, AvailableColors symbolColor, AvailableColors borderColor, float rot = 0)
        {
            BorderColor = borderColor;
            SymbolId = symbol;
            SymbolColor = symbolColor;
            Rot = rot;
        }

        public MemoflowMatchType[] Match(MemoflowStepVO other, MemoflowMatchType[] _matchTypes)
        {
            List<MemoflowMatchType> matches = new List<MemoflowMatchType>();
            if (Array.IndexOf(_matchTypes, MemoflowMatchType.SYMBOL) != -1 && SymbolId == other.SymbolId)
            {
                matches.Add(MemoflowMatchType.SYMBOL);
            }
            if (Array.IndexOf(_matchTypes, MemoflowMatchType.COLOR) != -1 && BorderColor.Equals(other.BorderColor))
            {
                matches.Add(MemoflowMatchType.COLOR);
            }
            if (matches.Count == 0)
            {
                matches.Add(MemoflowMatchType.NO);
            }
            return matches.ToArray();
        }
    }
}
