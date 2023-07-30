using System;

namespace GameWarriors.TaskDomain.Abstraction
{
    /// <summary>
    /// The base abstraction to work with loops like update, fixed, late.
    /// </summary>
    public interface IUpdateTask
    {
        /// <summary>
        /// Register action in update loop container.
        /// </summary>
        /// <param name="updateAction">Intended update method</param>
        /// <returns>update count in countainer</returns>
        int RegisterUpdateTask(Action updateAction);

        /// <summary>
        /// Remove action in update loop container.
        /// </summary>
        /// <param name="updateAction"></param>
        void UnRegisterUpdateTask(Action updateAction);

        /// <summary>
        /// Register action in fixed update loop container.
        /// </summary>
        /// <param name="fixedUpdateAction"></param>
        /// <returns></returns>
        int RegisterFixedUpdateTask(Action fixedUpdateAction);

        /// <summary>
        /// Remove action in fixed update loop container.
        /// </summary>
        /// <param name="fixedUpdateAction"></param>
        void UnRegisterFixedUpdateTask(Action fixedUpdateAction);

        /// <summary>
        /// Register action in late update loop container.
        /// </summary>
        /// <param name="lateUpdateAction"></param>
        /// <returns></returns>
        int RegisterLateUpdateTask(Action lateUpdateAction);

        /// <summary>
        /// Remove action in late update loop container.
        /// </summary>
        /// <param name="lateUpdateAction"></param>
        void UnRegisterLateUpdateTask(Action lateUpdateAction);

        /// <summary>
        /// Enable all loops execution.
        /// </summary>
        void EnableUpdate();

        /// <summary>
        /// Disalbe all loops execution.
        /// </summary>
        void DisableUpdate();
    }
}