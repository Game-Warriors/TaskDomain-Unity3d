using System;

namespace GameWarriors.TaskDomain.Abstraction
{
    internal interface ITaskAction
    {
        public Action ActionRef { get; }
        bool IsValid { get; }

        void Execute(double delta);
        void Clear();
    }
}