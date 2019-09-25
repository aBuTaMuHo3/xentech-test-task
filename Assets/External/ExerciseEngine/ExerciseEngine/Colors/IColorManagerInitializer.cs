using ExerciseEngine.Model.Enum;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExerciseEngine.Colors
{
    public interface IColorManagerInitializer
    {
        int NumColors { get; }

        Task LoadAllColors();
        AvailableColors[] GetRandomColors(int amount, AvailableColorPalettes fromPalette = AvailableColorPalettes.Default);
        AvailableColors GetRandomColor(AvailableColorPalettes fromPalette = AvailableColorPalettes.Default);
        Dictionary<AvailableColors, string> RawColors { get; }
        Dictionary<AvailableColors, string> DistinctColors { get; }
    }
}
