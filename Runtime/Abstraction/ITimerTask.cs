using System;

namespace GameWarriors.TaskDomain.Abstraction
{
    /// <summary>
    /// The base abstraction to work with timer base tasks which has execution time interval.
    /// </summary>
    public interface ITimerTask
    {
        /// <summary>
        /// Check loop timer task container and return item existence result.
        /// </summary>
        /// <param name="task">Intended task</param>
        /// <returns>retutn true if item exist</returns>
        bool IsLoopTimerTaskExist(Action task);

        /// <summary>
        /// Check repeat timer task container and return item existence result.
        /// </summary>
        /// <param name="task">Intended task</param>
        /// <returns>retutn true if item exist</returns>
        bool IsRepeatTimerTaskExist(Action task);

        /// <summary>
        /// Registering in loop timer task container and start timer. if task not exist, ends operation without exception.
        /// </summary>
        /// <param name="task">Intended task</param>
        /// <param name="interval">Time interval between each execution in seconds</param>
        void StartLoopTimerTask(Action task, float interval);

        /// <summary>
        /// Remove from loop timer task container and stop timer. if task not exist, ends operation without exception.
        /// </summary>
        /// <param name="task">Intended task</param>
        /// <returns>return true if operation was success, false if operation failed or task not exist</returns>
        bool StopLoopTimerTask(Action task);

        /// <summary>
        /// Register in repeat timer task container and start timer. if task does not exist, ends operation without exception.
        /// </summary>
        /// <param name="task">Intended task</param>
        /// <param name="interval">Time interval between each execution in seconds</param>
        /// <param name="repeatCount">Executing count after each time interval</param>
        void StartRepeatTimerTask(Action task, float interval, int repeatCount);

        /// <summary>
        /// Remove from repeat timer task container and stop timer. if task does not exist, ends operation without exception.
        /// </summary>
        /// <param name="task">Intended task</param>
        /// <returns>return true if operation was success, false if operation failed or task not exist</returns>
        bool StopRepeatTimerTask(Action task);

        /// <summary>
        /// Pause all timers counter.
        /// </summary>
        void DisableAllTimer();

        /// <summary>
        /// Start or resume all timers counter.
        /// </summary>
        void EnableAllTimer();
    }
}
