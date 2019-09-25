using System;
namespace ExerciseEngine.Model.ValueObjects.Language
{
    public class CharacterMovementVO
    {
        public float MinSec { get; }

        public float MaxSec { get; }

        public CharacterMovementVO(float minSec, float maxSec)
        {
            this.MinSec = minSec;
            this.MaxSec = maxSec;
        }
    }
}
