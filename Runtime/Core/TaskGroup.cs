using GameWarriors.TaskDomain.Abstraction;
using System;
using System.Collections.Generic;

namespace GameWarriors.TaskDomain.Core
{
    internal class TaskGroup<T> where T : ITaskAction
    {
        private readonly Dictionary<Action, int> _taskTable;
        private readonly List<int> _indexList;
        private T[] _tasks;
        private int _counter;

        public int TaskCount => _counter;

        public TaskGroup(int size)
        {
            _tasks = new T[size];
            _taskTable = new Dictionary<Action, int>(size);
            _indexList = new List<int>();
        }

        public void Start(T input)
        {
            if (input.ActionRef == null || _taskTable.ContainsKey(input.ActionRef))
                return;

            if (_counter >= _tasks.Length)
                Array.Resize(ref _tasks, _counter + 10);
            _tasks[_counter] = input;
            _taskTable.Add(input.ActionRef, _counter);
            ++_counter;
            return;
        }

        public bool Stop(Action task)
        {
            if (task != null && _taskTable.TryGetValue(task, out int stopIndex))
            {
                _indexList.Add(stopIndex);
                _taskTable.Remove(task);
                _tasks[stopIndex].Clear();
                return true;
            }
            return false;
        }

        public void Update(double deltaTime)
        {
            for (int i = 0; i < TaskCount; ++i)
            {
                if (_tasks[i].IsValid)
                    _tasks[i].Execute(deltaTime);
            }
        }

        public void CleanUp()
        {
            int length = _indexList.Count;
            for (int i = 0; i < length; ++i)
            {
                int index = _indexList[i];
                T tmp = _tasks[_counter - 1];
                _tasks[index] = tmp;
                if (tmp.IsValid)
                    _taskTable[tmp.ActionRef] = index;
                --_counter;
            }
            _indexList.Clear();
        }

        public bool IsExist(Action task)
        {
            return _taskTable.ContainsKey(task);
        }
    }
}