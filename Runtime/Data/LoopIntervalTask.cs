using GameWarriors.TaskDomain.Abstraction;
using System;
using System.Diagnostics;

namespace GameWarriors.TaskDomain.Data
{
    public struct LoopIntervalTask
    {
        private readonly float _interval;
        private Action _action;
        private double _buffer;

        public bool IsValid => _action != null;
        public Action ActionRef => _action;

        public LoopIntervalTask(Action action, float interval, double startTime)
        {
            _action = action;
            _interval = interval;
            _buffer = startTime;
        }

        public void Execute(double delta)
        {
            _buffer += delta;
            if (_buffer >= _interval)
            {
                _action();
                _buffer -= _interval;
            }
        }

        public void Clear()
        {
            _action = null;
        }
    }
}