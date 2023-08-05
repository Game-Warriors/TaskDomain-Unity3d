# TaskDomain
![GitHub](https://img.shields.io/github/license/svermeulen/Extenject)
## Table Of Contents

<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->
<summory>

  - [Introduction](#introduction)
  - [Features](#features)
  - [Installation](#installation)
  - [Startup](#startup)
  - [How To Use](#how-to-use)
</summory>

## Introduction
This library provides the simple workflow for method calling by timing, iterating or repeating by count utility. this library implemented fully by C# language and its core implementation depend on the Unity3D engine.

Support platforms: 
* PC/Mac/Linux
* iOS
* Android
* WebGL
* UWP App

```text
* Note: The library may work on other platforms, the source code just used C# code and .net standard version 2.0.
        this library has some feature which use Threading.Task library and it doesn't supported in WebGL.
```

* This library is design to be dependecy injection friendly, the recommended DI library is the [Dependency Injection](https://github.com/Game-Warriors/DependencyInjection-Unity3d) to be used.

This library used in the following games and apps:
</br>
[Street Cafe](https://play.google.com/store/apps/details?id=com.aredstudio.streetcafe.food.cooking.tycoon.restaurant.idle.game.simulation)
</br>
[Idle Family Adventure](https://play.google.com/store/apps/details?id=com.aredstudio.idle.merge.farm.game.idlefarmadventure)
</br>
[CLC BA](https://play.google.com/store/apps/details?id=com.palsmobile.clc)

## Features
* Controlling update execution and decouple update pipeline
* Specific and decouple coroutine pipeline
* Converting Threading.Task to IEnumerable and vice versa feature
* Loop timer feature to execute loop update by specific interval
* Repeatable timer feature to call the methods by specific interval and limit repeat count 

## Installation
This library can be added by unity package manager form git repository or could be downloaded.

For more information about how to install a package by unity package manager, please read the manual in this link:
[Install a package from a Git URL](https://docs.unity3d.com/Manual/upm-ui-giturl.html)

## Startup
After adding package to using the library features, the “TaskSystem” class should be created. The “TaskSystem” class provide all system features like loop routine update, fixed update and late update methods, running and control coroutines, time interval execution methods by limited or unlimited repeat count.
```csharp
private void Awake()
{
    TaskSystem taskSystem = new TaskSystem();
}
```
## How To Use
There are three abstraction which present system features. the “TaskSystem” class Implement all these abstractions.

<h3>ITaskRunner</h3>
The base abstraction to work with one time execution task by IEnumerator, Unity3d Coroutines and threading tasks. the Runner class bind by “MonoBehaviour” which run all coroutines on its and this object is always enable. the Runner could return the started Coroutine reference, so there is capability to stop Coroutine, also there is method to stop all Coroutines at once.

```csharp
public interface ITaskRunner
{
    Coroutine StartCoroutineTask(IEnumerator routine);
    void StopCoroutineTask(Coroutine routine);
    void StopAllTasks();
    IEnumerator TaskToCoroutineAsync(Task task);
    IEnumerator TaskToCoroutineAsync<T>(Task<T> task);
    Coroutine DoNextFrame(Action action);
}
```
* StartCoroutineTask: Start Unity3D Coroutine by passed routine and return Coroutine reference, call the action after completion.
* StopCoroutineTask: Stop and break the routine.
* StopAllTasks: Stop all coroutines which start by this instance of task runner.
* TaskToCoroutineAsync: Create the Coroutine which wraps task completion into the coroutine scheme.
* CoroutineToTaskAsync: Create task completion source and bind the task completion to routine end\break state.
* DoNextFrame: Create the Coroutine that yield in current frame and execute next frame.

<h3>IUpdateTask</h3>
The base abstraction to work with iterating loops like update, fixed, late methods. there are enable/disable method to pause or start updates tick and it disable by default.

```csharp
public interface IUpdateTask
{
    void EnableUpdate();
    void DisableUpdate();

    int RegisterUpdateTask(Action updateAction);
    void UnRegisterUpdateTask(Action updateAction);
    int RegisterFixedUpdateTask(Action fixedUpdateAction);
    void UnRegisterFixedUpdateTask(Action fixedUpdateAction);
    int RegisterLateUpdateTask(Action lateUpdateAction);
    void UnRegisterLateUpdateTask(Action lateUpdateAction);
}
```
* RegisterUpdateTask: Register action in update loop container.
* UnRegisterUpdateTask: Remove action in update loop container.
* RegisterFixedUpdateTask: Register action in fixed update loop container.
* UnRegisterFixedUpdateTask: Remove action in fixed update loop container.
* RegisterLateUpdateTask: Register action in late update loop container.
* UnRegisterLateUpdateTask: Remove action in late update loop container.

<h3>ITimerTask</h3>
The base abstraction to work with timer base tasks which has execution time interval.  there are enable/disable method to pause or start timer tick and it enable by default.  

```csharp
public interface ITimerTask
{
    void DisableAllTimer();
    void EnableAllTimer();

    bool IsLoopTimerTaskExist(Action task);
    bool IsRepeatTimerTaskExist(Action task);
    void StartLoopTimerTask(Action task, float interval);
    bool StopLoopTimerTask(Action task);
    void StartRepeatTimerTask(Action task, float interval, int repeatCount);
    bool StopRepeatTimerTask(Action task);
}
```
* IsLoopTimerTaskExist: Check loop timer task container and return item existence result.
* StartLoopTimerTask: Registering in loop timer task container and start timer. if task does not exist, ends operation without exception.
* StopLoopTimerTask: Remove from loop timer task container and stop timer. if task does not exist, ends operation without exception.
* IsRepeatTimerTaskExist: Check repeat timer task container and return item existence result.
* StartRepeatTimerTask: Register in repeat timer task container and start timer. if task does not exist, ends operation without exception.
* StopRepeatTimerTask: Remove from repeat timer task container and stop timer. if task does not exist, ends operation without exception.