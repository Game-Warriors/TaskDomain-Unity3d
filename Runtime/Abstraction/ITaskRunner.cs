using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace GameWarriors.TaskDomain.Abstraction
{
    public interface ITaskRunner
    {
        Coroutine StartCoroutineTask(IEnumerator routine);

        /// <summary>
        /// Use as parameter for StartCoroutine() method.
        /// e.g StartCoroutine( CoroutineWithCallback(method with IEnumerator return type, finish callback ) )
        /// </summary>
        /// <param gameObjectName="routine"></param>
        /// <param gameObjectName="callback"></param>
        /// <returns></returns>
        Coroutine StartCoroutineTask(IEnumerator routine, Action callback);

        Coroutine StartCoroutineTask(YieldInstruction instruction, Action callback);
        /// <summary>
        /// Wraps task functionality into the coroutine scheme. 
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        IEnumerator TaskToCoroutineAsync(Task task);
        void StopCoroutineTask(Coroutine routine);

        /// <summary>
        /// Wraps task functionality into the coroutine scheme. 
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        IEnumerator TaskToCoroutineAsync<T>(Task<T> task);
        Coroutine StartDelayTask(float delay, Action action);
        Coroutine StartDelayTask(YieldInstruction delay, Action callback);

        /// <summary>
        /// Wraps coroutine functionality into the async/task scheme. 
        /// </summary>
        /// <param name="coroutine"></param>
        /// <returns></returns>
        Task CoroutineToTaskAsync(IEnumerator coroutine);
        Coroutine DoNextFrame(Action action);
        void StopAllTasks();
    }
}