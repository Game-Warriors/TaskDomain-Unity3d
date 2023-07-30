using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace GameWarriors.TaskDomain.Abstraction
{
    /// <summary>
    /// The base abstraction to work with one time execution task by IEnumerator, Unity3d Coroutines and threading tasks
    /// </summary>
    public interface ITaskRunner
    {
        /// <summary>
        /// Start Unity3D Coroutine by passed routine and return Coroutine reference
        /// </summary>
        /// <param name="routine">Intent routine to start</param>
        /// <returns>started Unity3D Coroutine reference</returns>
        Coroutine StartCoroutineTask(IEnumerator routine);

        /// <summary>
        /// Start Unity3D Coroutine by passed routine and return Coroutine reference, call the action after completion
        /// </summary>
        /// <param name="routine">Intent routine to start</param>
        /// <param name="callback">Trigger action after routine completed</param>
        /// <returns>>started Unity3D Coroutine reference</returns>
        Coroutine StartCoroutineTask(IEnumerator routine, Action callback);

        /// <summary>
        /// Start Unity3D YieldInstruction by passed instruction and return Coroutine reference, call the action after completion
        /// </summary>
        /// <param name="instruction">Intent instruction to start</param>
        /// <param name="callback">Trigger action after routine completed</param>
        /// <returns>>started Unity3D Coroutine reference</returns>
        Coroutine StartCoroutineTask(YieldInstruction instruction, Action callback);

        /// <summary>
        /// Create the Coroutine which wraps task completion into the coroutine scheme. 
        /// </summary>
        /// <param name="task">Intent running task</param>
        /// <returns>return IEnumerator that break when task completed</returns>
        IEnumerator TaskToCoroutineAsync(Task task);

        /// <summary>
        /// Create the Coroutine which wraps task completion into the coroutine scheme. 
        /// </summary>
        /// <param name="task">Intent running task</param>
        /// <returns>return IEnumerator that break when task completed</returns>
        IEnumerator TaskToCoroutineAsync<T>(Task<T> task);

        /// <summary>
        /// Stop and break the routine.
        /// </summary>
        /// <param name="routine">Intent routine to stop</param>
        void StopCoroutineTask(Coroutine routine);

        /// <summary>
        /// Executing action after time delay.
        /// </summary>
        /// <param name="delay">delay for execution in seconds</param>
        /// <param name="action">Trigger the action after delay</param>
        /// <returns>started Unity3D Coroutine reference</returns>
        Coroutine StartDelayTask(float delay, Action action);

        /// <summary>
        ///   Wraps coroutine functionality into the async/task scheme. 
        /// </summary>
        /// <param name="routine">The routine will bind to task completion</param>
        /// <returns>started task reference</returns>
        Task CoroutineToTaskAsync(IEnumerator routine);

        /// <summary>
        /// Create the Coroutine that yield in current frame and execute next frame
        /// </summary>
        /// <param name="action">Trigger the action in next engine frame</param>
        /// <returns>started Unity3D Coroutine reference</returns>
        Coroutine DoNextFrame(Action action);

        /// <summary>
        /// Stop all coroutines which start by this instance of task runner.
        /// </summary>
        void StopAllTasks();
    }
}