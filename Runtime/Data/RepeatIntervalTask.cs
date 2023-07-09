using GameWarriors.TaskDomain.Abstraction;
using System;

namespace GameWarriors.TaskDomain.Data
{
    internal struct RepeatIntervalTask : ITaskAction
    {
        private readonly float _interval;
        private readonly Action _action;
        private double _buffer;
        private short _repeatCount;

        public bool IsValid => _repeatCount > 0;
        public Action ActionRef => _action;
        public short RepeatCount => _repeatCount;

        public RepeatIntervalTask(Action action, float interval, short repeatCount)
        {
            _action = action;
            _interval = interval;
            _buffer = 0;
            _repeatCount = repeatCount;
        }

        public void Execute(double delta)
        {
            _buffer += delta;
            if (_buffer >= _interval)
            {
                _action();
                _buffer -= _interval;
                --_repeatCount;
            }
        }

        public void Clear()
        {
            _repeatCount = 0;
        }
    }
}