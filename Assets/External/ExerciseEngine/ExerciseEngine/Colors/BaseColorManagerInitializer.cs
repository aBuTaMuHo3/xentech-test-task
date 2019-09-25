using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExerciseEngine.Model.Enum;

namespace ExerciseEngine.Colors
{
    public class BaseColorManagerInitializer : IColorManagerInitializer
    {

        protected List<AvailableColors> _colors;

        protected readonly Random _random;

        // TODO: this is taking the number of colors from RawColors. What if other palette is used ????
        public virtual int NumColors
        {
            get
            {
                return RawColors.Count;
            }
        }

        public virtual Dictionary<AvailableColors, string> RawColors { get; } = new Dictionary<AvailableColors, string>()
        {
            { AvailableColors.Red, "CC3300" },
            { AvailableColors.Orange, "FF8C00" },
            { AvailableColors.Brown, "864300" },
            { AvailableColors.Black, "333333" },
            { AvailableColors.Purple, "990099" },
            { AvailableColors.Turquoise, "66CCCC" },
            { AvailableColors.Green, "129948" }
        };

        public virtual Dictionary<AvailableColors, string> DistinctColors { get; } = new Dictionary<AvailableColors, string>()
        {
            {AvailableColors.DistinctBlue, "3C79FD"},
            {AvailableColors.DistinctRed, "C43225"},
            {AvailableColors.DistinctOrange, "FF9D00"},
            {AvailableColors.DistinctBlack, "000000"},
            {AvailableColors.DistinctPink, "F749A6"},
            {AvailableColors.DistinctTurquoise, "62BEC6"},
            {AvailableColors.DistinctGreen, "6CAF00"},
            {AvailableColors.DistinctPurple, "A73BC0"}
        };

        public virtual int ColorType { get; protected set; }

        public BaseColorManagerInitializer()
        {
            _random = new Random();
        }

        public virtual AvailableColors GetRandomColor(AvailableColorPalettes fromPalette = AvailableColorPalettes.Default)
        {
            if (_colors == null)
            {
                switch (fromPalette)
                {
                    default:
                        _colors = RawColors.Keys.ToList();
                        break;
                    case AvailableColorPalettes.Distinct:
                        _colors = DistinctColors.Keys.ToList();
                        break;
                }
            }

            return _colors[_random.Next(0, _colors.Count)];
        }

        public AvailableColors[] GetRandomColors(int amount, AvailableColorPalettes fromPalette = AvailableColorPalettes.Default)
        {
            switch (fromPalette)
            {
                default:
                    _colors = RawColors.Keys.ToList();
                    break;
                case AvailableColorPalettes.Distinct:
                    _colors = DistinctColors.Keys.ToList();
                    break;
            }

            var usedAmount = amount > _colors.Count ? _colors.Count : amount;
            AvailableColors[] returnColors = new AvailableColors[usedAmount];

            int randIndex = 0;
            for (int i = 0; i < usedAmount; i++)
            {
                randIndex = _random.Next(0, _colors.Count);
                returnColors[i] = _colors[randIndex];
                _colors.RemoveAt(randIndex);
            }
            _colors = null;

            return returnColors;
        }

        public virtual Task LoadAllColors()
        {
            return null;
        }
    }
}