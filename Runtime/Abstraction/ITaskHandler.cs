using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameWarriors.TaskDomain.Abstraction
{
    public interface ITaskHandler
    {
        int TaskId { get; }
    }
}