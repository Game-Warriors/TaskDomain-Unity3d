using System;
using System.Collections;
using System.Threading.Tasks;
using GameWarriors.TaskDomain.Abstraction;
using UnityEngine;

namespace GameWarriors.TaskDomain.Core
{
    /// <summary>
    /// This class provide all system feature like: loop routine update,fixed update and lated update methods, running and control coroutines, time interval execution methods by limited or unlimited repeat count.
    /// </summary>
    public class TaskSystem : MonoBehaviour, ITaskRunner, IUpdateTask, ITimerTask
    {
        private readonly TimerTask _timerTask;

        private Action[] _updateArray;
        private Action[] _lateUpdateArray;
        private Action[] _fixedUpdateArray;
        private int _updateCount;
        private int _lateUpdateCount;
        private int _fixedUpdateCount;
        private bool _updateEnable;
        private bool _isUpdateTimer;


        [UnityEngine.Scripting.Preserve]
        public TaskSystem()
        {
            _updateArray = new Action[15];
            _updateCount = 0;
            _lateUpdateArray = new Action[5];
            _lateUpdateCount = 0;
            _fixedUpdateArray = new Action[10];
            _fixedUpdateCount = 0;
            _timerTask = new TimerTask();
            _isUpdateTimer = true;
        }

        Coroutine ITaskRunner.StartCoroutineTask(IEnumerator routine)
        {
            return StartCoroutine(routine);
        }

        void ITaskRunner.StopCoroutineTask(Coroutine routine)
        {
            StopCoroutine(routine);
        }

        Coroutine ITaskRunner.StartCoroutineTask(IEnumerator routine, Action onDone)
        {
            return StartCoroutine(YieldAndDone(routine, onDone));
        }

        Coroutine ITaskRunner.StartCoroutineTask(YieldInstruction instruction, Action callback)
        {
            return StartCoroutine(YieldAndDone(instruction, callback));
        }

        IEnumerator ITaskRunner.TaskToCoroutineAsync(Task task)
        {
            yield return new WaitUntil(() => task.IsCompleted);
        }

        IEnumerator ITaskRunner.TaskToCoroutineAsync<T>(Task<T> task)
        {
            yield return new WaitUntil(() => task.IsCompleted);
        }

        Task ITaskRunner.CoroutineToTaskAsync(IEnumerator routine)
        {
            TaskCompletionSource<bool> taskSource = new TaskCompletionSource<bool>();
            StartCoroutine(SignalOnCoroutineDone(routine, taskSource));
            return taskSource.Task;
        }

        Coroutine ITaskRunner.DoNextFrame(Action callback)
        {
            return StartCoroutine(YieldAndDone((YieldInstruction)null, callback));
        }

        Coroutine ITaskRunner.StartDelayTask(float delay, Action callback)
        {
            return StartCoroutine(YieldAndDone(new WaitForSeconds(delay), callback));
        }

        void ITaskRunner.StopAllTasks()
        {
            StopAllCoroutines();
        }

        int IUpdateTask.RegisterUpdateTask(Action updateAction)
        {
            if (_updateCount >= _updateArray.Length)
                Array.Resize(ref _updateArray, _updateCount + 10);
            _updateArray[_updateCount] = updateAction;
            return _updateCount++;
        }

        void IUpdateTask.UnRegisterUpdateTask(Action updateAction)
        {
            if (_updateCount == 0)
                return;

            int length = _updateCount;
            int index = -1;
            for (int i = 0; i < length; ++i)
            {
                if (_updateArray[i] == updateAction)
                {
                    index = i;
                    break;
                }
            }

            if (index > -1)
            {
                --_updateCount;
                _updateArray[index] = _updateCount != index ? _updateArray[_updateCount] : null;
            }
        }

        int IUpdateTask.RegisterFixedUpdateTask(Action fixedUpdateAction)
        {
            if (_fixedUpdateCount >= _fixedUpdateArray.Length)
                Array.Resize(ref _fixedUpdateArray, _fixedUpdateCount + 10);
            _fixedUpdateArray[_fixedUpdateCount] = fixedUpdateAction;
            return _fixedUpdateCount++;
        }

        void IUpdateTask.UnRegisterFixedUpdateTask(Action fixedUpdateAction)
        {
            if (_fixedUpdateCount == 0)
                return;

            int length = _fixedUpdateCount;
            int index = -1;
            for (int i = 0; i < length; ++i)
            {
                if (_fixedUpdateArray[i] == fixedUpdateAction)
                {
                    index = i;
                    break;
                }
            }

            if (index > -1)
            {
                --_fixedUpdateCount;
                _fixedUpdateArray[index] = _fixedUpdateCount != index ? _fixedUpdateArray[_fixedUpdateCount] : null;
            }
        }

        int IUpdateTask.RegisterLateUpdateTask(Action lateUpdateAction)
        {
            if (_lateUpdateCount >= _lateUpdateArray.Length)
                Array.Resize(ref _lateUpdateArray, _lateUpdateCount + 5);
            _lateUpdateArray[_lateUpdateCount] = lateUpdateAction;
            return _lateUpdateCount++;
        }

        void IUpdateTask.UnRegisterLateUpdateTask(Action lateUpdateAction)
        {
            if (_lateUpdateCount == 0)
                return;

            int length = _lateUpdateCount;
            int index = -1;
            for (int i = 0; i < length; ++i)
            {
                if (_lateUpdateArray[i] == lateUpdateAction)
                {
                    index = i;
                    break;
                }
            }

            if (index > -1)
            {
                --_lateUpdateCount;
                _lateUpdateArray[index] = _lateUpdateCount != index ? _lateUpdateArray[_lateUpdateCount] : null;
            }
        }

        void IUpdateTask.EnableUpdate()
        {
            _updateEnable = true;
        }

        void IUpdateTask.DisableUpdate()
        {
            _updateEnable = false;
        }


        void ITimerTask.StartLoopTimerTask(Action task, float interval)
        {
            _timerTask.StartLoopTimerTask(task, interval, 0);
        }

        bool ITimerTask.StopLoopTimerTask(Action task)
        {
            return _timerTask.StopLoopTimerTask(task);
        }

        void ITimerTask.StartRepeatTimerTask(Action task, float interval, int repeatCount)
        {
            _timerTask.StartRepeatTimerTask(task, interval, repeatCount);
        }

        bool ITimerTask.StopRepeatTimerTask(Action task)
        {
            return _timerTask.StopRepeatTimerTask(task);
        }

        private void Update()
        {
            if (_updateEnable)
            {
                int length = _updateCount;
                for (int i = 0; i < length; ++i)
                {
                    _updateArray[i]?.Invoke();
                }
            }
            if (_isUpdateTimer)
                _timerTask.SystemUpdate(Time.deltaTime);
        }

        private void LateUpdate()
        {
            if (!_updateEnable) return;
            int length = _lateUpdateCount;
            for (int i = 0; i < length; ++i)
            {
                _lateUpdateArray[i]();
            }
        }

        private void FixedUpdate()
        {
            if (!_updateEnable) return;
            for (int i = 0; i < _fixedUpdateCount; ++i)
            {
                _fixedUpdateArray[i]();
            }
        }

        private IEnumerator SignalOnCoroutineDone(IEnumerator coroutine, TaskCompletionSource<bool> taskSource)
        {
            yield return coroutine;
            taskSource.TrySetResult(true);
        }

        private IEnumerator YieldAndDone(IEnumerator instruction, Action onDone)
        {
            yield return instruction;
            onDone?.Invoke();
        }

        private IEnumerator YieldAndDone(YieldInstruction instruction, Action onDone)
        {
            yield return instruction;
            onDone?.Invoke();
        }

        public bool IsLoopTimerTaskExist(Action task)
        {
            return _timerTask.IsLoopTimerTaskExist(task);
        }

        public bool IsRepeatTimerTaskExist(Action task)
        {
            return _timerTask.IsRepeatTimerTaskExist(task);
        }

        public void DisableAllTimer()
        {
            _isUpdateTimer = false;
        }

        public void EnableAllTimer()
        {
            _isUpdateTimer = true;
        }
    }
}