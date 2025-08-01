using System;

namespace Game.CodeBase.Common.StateManagement
{
    public interface ITransition
    {
        Type To { get; }

        Func<bool> Condition { get; }
    }
}