using System;

namespace GameWarriors.TaskDomain.Abstraction
{
    public interface IUpdateTask
    {
        int RegisterUpdateTask(Action updateAction);
        void UnRegisterUpdateTask(Action updateAction);

        int RegisterFixedUpdateTask(Action fixedUpdateAction);
        void UnRegisterFixedUpdateTask(Action fixedUpdateAction);

        int RegisterLateUpdateTask(Action lateUpdateAction);
        void UnRegisterLateUpdateeTask(Action lateUpdateAction);

        void EnableUpdate();
        void DisableUpdate();
    }
}