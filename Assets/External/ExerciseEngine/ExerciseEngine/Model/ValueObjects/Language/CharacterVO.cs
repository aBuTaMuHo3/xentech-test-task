using System;
namespace ExerciseEngine.Model.ValueObjects.Language
{
    public class CharacterVO
    {
        public char Character { get; set; }

        public int IconAssetId { get; set; }

        public float Seconds { get; set; }

        public int WordIndex { get; }

        public string Formula { get; set; }

        public CharacterMovementVO Rotation {get;set;}

        public CharacterVO(char character, int wordIndex = 0)
        {
            this.Character = character;
            this.WordIndex = wordIndex;
            IconAssetId = -1;
        }
    }
}
