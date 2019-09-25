using ExerciseEngine.Model.Enum;
using ExerciseEngine.Model.ValueObjects.Interfaces;
using SynaptikonFramework.Util.Math;
using SynaptikonFramework.View.Interfaces;

namespace FlashGlance.Model.ValueObjects
{
    public class FlashGlanceRoundItemVO: IRoundItem, ILinearSliderData
    {
        
        // All properties are set by the model, for now no need to take care about it
        public FlashGlanceRoundItemVO(int id, int cypher, SafeHashCodePoint gridPosition, int rotation, float scale, AvailableColors color)
        {
            Id = id;
            Cypher = cypher;
            GridPosition = gridPosition;
            Rotation = rotation;
            Scale = scale;
            Color = color;
        }

        public int Id { get; }
        
        /// <summary>
        /// The number of the item, shown on the display
        /// </summary>
        public int Cypher { get; }
        
        /// <summary>
        /// Defines the position in the view, needs to be translated to the actual grid size on screen
        /// Currently size is 9 x 4
        /// </summary>
        public SafeHashCodePoint GridPosition { get; set; }
        
        // Can be implemented, in higher levels there is some variation how the items are shown
        public int Rotation { get; }
        // Can be implemented, in higher levels there is some variation how the items are shown
        public float Scale { get; }
        
        // Can be ignored for now
        public AvailableColors Color { get; set; }
        public bool IsBusy { get; set; }

        public override string ToString()
        {
            return string.Concat("Cypher:", Cypher, ", Position: ", GridPosition.ToString());
        }

        public override bool Equals(object obj)
        {
            return obj is FlashGlanceRoundItemVO vO && Cypher == vO.Cypher;
        }

        public override int GetHashCode()
        {
            return Cypher.GetHashCode();
        }
    }
}