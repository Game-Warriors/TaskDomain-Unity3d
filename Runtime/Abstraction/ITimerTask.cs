using System;

namespace GameWarriors.TaskDomain.Abstraction
{
    public interface ITimerTask
    {
        void StartLoopTimerTask(Action task, float interval);
        bool StopLoopTimerTask(Action task);
        void StartRepeatTimerTask(Action task, float interval, int repeatCount);
        bool StopRepeatTimerTask(Action task);
    }
}
