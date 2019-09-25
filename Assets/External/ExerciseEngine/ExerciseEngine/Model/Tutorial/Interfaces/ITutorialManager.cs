using System;
namespace ExerciseEngine.Model.Tutorial.Interfaces
{
    public delegate void TutorialManagerUpdateHandler(ITutorialManagerUpdateVO vo);

    public interface ITutorialManager : IDisposable
    {
        /// <summary>
        ///  Function checking for tutorial and updating tutorial loop. It starts or finishes a tutorial if necessary
        ///  @return It returns tutorial type if a tutorial just started or still running after increasing tutorial loop 
        /// </summary>
        /// <returns>It returns null if no tutorial has started or finished after increasing tutorial loop.</returns>
        ITutorialStateHandler GetTutorialTypeAndUpdate();
       
        /// <summary>
        /// quick getter telling if currently a tutorial is running
        /// <see cref="T:ExerciseEngine.Model.Tutorial.Interfaces.ITutorialManager"/> is tutorial running.
        /// </summary>
        /// <value><c>true</c> if is tutorial running; otherwise, <c>false</c>.</value>
        bool IsTutorialRunning { get; }

        /// <summary>
        /// Occurs when on update.(start/finish tutorial)
        /// </summary>
        event TutorialManagerUpdateHandler OnUpdate;

        /// <summary>
        /// Gets the current tutorial loop.
        /// </summary>
        /// <value>The current tutorial loop.</value>
        int CurrentTutorialLoop{
            get;
        }
    }
}
