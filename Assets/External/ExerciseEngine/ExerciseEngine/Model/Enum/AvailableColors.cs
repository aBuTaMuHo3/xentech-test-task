using Newtonsoft.Json;
using SynaptikonFramework.Util;

namespace ExerciseEngine.Model.Enum {

    [JsonConverter(typeof(TolerantEnumConverter))]
    public enum AvailableColors {
        LightBlue,
        LightGreen,
        Blue,
        Purple,
        Orange,
        Green,
        Red,
        Yellow,
        Black,
        Pink,
        Brown,
        Grey,
        Turquoise,
        DistinctBlue,
        DistinctRed,
        DistinctOrange,
        DistinctBlack,
        DistinctPink,
        DistinctTurquoise,
        DistinctGreen,
        DistinctPurple
    }

    [JsonConverter(typeof(TolerantEnumConverter))]
    public enum AvailableColorPalettes {
        Default,
        Distinct
    }
}
