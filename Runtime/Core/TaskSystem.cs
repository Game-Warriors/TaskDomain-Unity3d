using System;
using System.Collections;
using System.Threading.Tasks;
using TaskDomain.Abstraction;
using UnityEngine;

namespace TaskDomain.Core
{
    public class TaskSystem : MonoBehaviour, ITaskRunner, IUpdateTask
    {
        private Action[] _updateArray;
        private Action[] _lateUpdateArray;
        private Action[] _fixedUpdateArray;
        private int _updateCount;
        private int _lateUpdateCount;
        private int _fixedUpdateCount;

        private bool _updateEnable;

        [UnityEngine.Scripting.Preserve]
        public TaskSystem()
        {
            _updateArray = new Action[15];
            _updateCount = 0;
            _lateUpdateArray = new Action[10];
            _lateUpdateCount = 0;
            _fixedUpdateArray = new Action[5];
            _fixedUpdateCount = 0;
        }

        Coroutine ITaskRunner.StartCoroutineTask(IEnumerator routine)
        {
            return StartCoroutine(routine);
        }

        void ITaskRunner.StopCoroutineTask(Coroutine routine)
        {
            StopCoroutine(routine);
        }

        public Coroutine StartCoroutineTask(IEnumerator routine, Action onDone)
        {
            return StartCoroutine(YieldAndDone(routine, onDone));
        }

        public Coroutine StartCoroutineTask(YieldInstruction instruction, Action callback)
        {
            return StartCoroutine(YieldAndDone(instruction, callback));
        }

        public IEnumerator TaskToCoroutineAsync(Task task)
        {
            yield return new WaitUntil(() => task.IsCompleted);
        }

        public IEnumerator TaskToCoroutineAsync<T>(Task<T> task)
        {
            yield return new WaitUntil(() => task.IsCompleted);
        }

        public async Task CoroutineToTaskAsync(IEnumerator routine)
        {
            TaskCompletionSource<bool> taskSource = new TaskCompletionSource<bool>();
            StartCoroutine(SignalOnCoroutineDone(routine, taskSource));
            await taskSource.Task;
        }

        Coroutine ITaskRunner.DoNextFrame(Action callback)
        {
            return StartCoroutine(YieldAndDone((YieldInstruction)null, callback));
        }

        Coroutine ITaskRunner.StartDelayTask(float delay, Action callback)
        {
            return StartCoroutine(YieldAndDone(new WaitForSeconds(delay), callback));
        }

        Coroutine ITaskRunner.StartDelayTask(YieldInstruction delay, Action callback)
        {
            return StartCoroutine(YieldAndDone(delay, callback));
        }

        public void StopAllTasks()
        {
            StopAllCoroutines();
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

        public int RegisterUpdateTask(Action updateAction)
        {
            if (_updateCount >= _updateArray.Length)
                Array.Resize(ref _updateArray, _updateCount + 10);
            _updateArray[_updateCount] = updateAction;
            return _updateCount++;
        }

        public void UnRegisterUpdateTask(Action updateAction)
        {
            if (_updateCount == 0)
                return;

            int length = _updateCount;
            int index = -1;
            for (int i = 0; i < length; i++)
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
                _updateArray[index] = _updateCount != index ? _updateArray[index] : null;
            }
        }

        public int RegisterFixedUpdateTask(Action fixedUpdateAction)
        {
            if (_fixedUpdateCount >= _fixedUpdateArray.Length)
                Array.Resize(ref _fixedUpdateArray, _fixedUpdateCount + 10);
            _fixedUpdateArray[_fixedUpdateCount] = fixedUpdateAction;
            return _fixedUpdateCount++;
        }

        public void UnRegisterFixedUpdateTask(Action fixedUpdateAction)
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
                _fixedUpdateArray[index] = _fixedUpdateCount != index ? _fixedUpdateArray[index] : null;
            }
        }

        public int RegisterLateUpdateTask(Action lateUpdateAction)
        {
            if (_lateUpdateCount >= _lateUpdateArray.Length)
                Array.Resize(ref _lateUpdateArray, _lateUpdateCount + 5);
            _lateUpdateArray[_lateUpdateCount] = lateUpdateAction;
            return _lateUpdateCount++;
        }

        public void UnRegisterLateUpdateeTask(Action lateUpdateAction)
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
                _lateUpdateArray[index] = _lateUpdateCount != index ? _lateUpdateArray[index] : null;
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

        private void Update()
        {
            if (!_updateEnable) return;
            int length = _updateCount;
            for (int i = 0; i < length; ++i)
            {
                _updateArray[i]?.Invoke();
            }
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
    }
}