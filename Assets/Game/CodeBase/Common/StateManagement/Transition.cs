using System;

namespace Game.CodeBase.Common.StateManagement
{
    public class Transition<TTo> : ITransition where TTo : IEnterState
    {
        public Transition(Func<bool> condition)
        {
            Condition = condition;
        }

        public Type To => typeof(TTo);

        public Func<bool> Condition { get; }
    }
}
