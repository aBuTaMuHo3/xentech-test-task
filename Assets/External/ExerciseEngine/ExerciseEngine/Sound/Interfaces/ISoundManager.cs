using System;
using System.Threading.Tasks;

namespace ExerciseEngine.Sound.Interfaces
{
    public interface ISoundManager : IDisposable
    {
        bool Mute { get; set; }

        Task InitAsync();
        void PlaySound(string soundID);
        void StopSound(string soundID);
    }
}
