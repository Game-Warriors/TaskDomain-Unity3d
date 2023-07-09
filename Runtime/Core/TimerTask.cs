using GameWarriors.TaskDomain.Data;
using System;

namespace GameWarriors.TaskDomain.Core
{
    public class TimerTask
    {
        private readonly TaskGroup<LoopIntervalTask> _loopTask;
        private readonly TaskGroup<RepeatIntervalTask> _repeatTask;

        public TimerTask()
        {
            _loopTask = new TaskGroup<LoopIntervalTask>(20);
            _repeatTask = new TaskGroup<RepeatIntervalTask>(20);
        }

        public void StartLoopTimerTask(Action task, float interval, double startTime)
        {
            _loopTask.Start(new LoopIntervalTask(task, interval, startTime));
        }

        public bool StopLoopTimerTask(Action task)
        {
            return _loopTask.Stop(task);
        }

        public void StartRepeatTimerTask(Action task, float interval, int repeatCount)
        {
            _repeatTask.Start(new RepeatIntervalTask(task, interval, (short)repeatCount));
        }

        public bool StopRepeatTimerTask(Action task)
        {
            return _repeatTask.Stop(task);
        }

        public void SystemUpdate(double deltaTime)
        {
            _loopTask.Update(deltaTime);
            _loopTask.CleanUp();

            _repeatTask.Update(deltaTime);
            _repeatTask.CleanUp();
        }
    }
}
