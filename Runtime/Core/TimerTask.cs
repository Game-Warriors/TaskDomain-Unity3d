using GameWarriors.TaskDomain.Data;
using System;
using System.Collections.Generic;

namespace GameWarriors.TaskDomain.Core
{
    public class TimerTask
    {
        private LoopIntervalTask[] _loopTask;
        private Dictionary<Action, int> _loopTaskTable;
        private List<int> _removeIndex;
        private int _loopCounter;

        public TimerTask()
        {
            _loopTask = new LoopIntervalTask[20];
            _loopTaskTable = new Dictionary<Action, int>();
            _removeIndex = new List<int>();
        }

        public void StartLoopTimerTask(Action task, float interval, double startTime)
        {
            if (task == null || _loopTaskTable.ContainsKey(task))
                return;

            if (_loopCounter > _loopTask.Length)
                Array.Resize(ref _loopTask, _loopCounter + 10);
            _loopTask[_loopCounter] = new LoopIntervalTask(task, interval, startTime);
            _loopTaskTable.Add(task,_loopCounter);
            ++_loopCounter;
            return;
        }

        public bool StopLoopTimerTask(Action task)
        {
            if (task != null && _loopTaskTable.TryGetValue(task, out int stopIndex))
            {
                _removeIndex.Add(stopIndex);
                _loopTaskTable.Remove(task);
                _loopTask[stopIndex].Clear();
                return true;
            }
            return false;
        }

        public void StartRepeatTimerTask(Action task, float interval, int repeatCount)
        {
            throw new NotSupportedException();
        }

        public bool StopRepeatTimerTask(Action task)
        {
            throw new NotSupportedException();
        }

        public void SystemUpdate(double deltaTime)
        {
            for (int i = 0; i < _loopCounter; ++i)
            {
                if (_loopTask[i].IsValid)
                    _loopTask[i].Execute(deltaTime);
            }
            CleanUp();
        }

        private void CleanUp()
        {
            int length = _removeIndex.Count;
            for (int i = 0; i < length; ++i)
            {
                int index = _removeIndex[i];
                LoopIntervalTask tmp = _loopTask[_loopCounter - 1];
                _loopTask[index] = tmp;
                if (tmp.IsValid)
                    _loopTaskTable[tmp.ActionRef] = index;
                --_loopCounter;
            }
            _removeIndex.Clear();
        }
    }
}
