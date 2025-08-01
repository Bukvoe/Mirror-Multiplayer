using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.CodeBase.Common.StateManagement
{
    public class StateMachine
    {
        private IEnterState _currentState;
        private readonly IEnterState[] _states;
        private readonly Dictionary<Type, List<ITransition>> _stateTransitions;

        public StateMachine(Dictionary<IEnterState, List<ITransition>> stateTransitions)
        {
            _states = stateTransitions.Keys.ToArray();
            _stateTransitions = stateTransitions.ToDictionary(pair => pair.Key.GetType(), pair => pair.Value);

            _currentState = _states[0];
            _currentState.Enter();
        }

        public void Update()
        {
            if (_currentState is IUpdateState activeState)
            {
                activeState.Update();
            }

            CheckTransitions();
        }

        private void CheckTransitions()
        {
            if (!_stateTransitions.TryGetValue(_currentState.GetType(), out var currentStateTransitions))
            {
                throw new InvalidOperationException($"No transitions defined for {_currentState.GetType().Name}");
            }

            foreach (var currentStateTransition in currentStateTransitions)
            {
                if (currentStateTransition.Condition())
                {
                    ChangeState(currentStateTransition.To);
                    break;
                }
            }
        }

        private void ChangeState(Type type)
        {
            if (_currentState is IExitState currentState)
            {
                currentState.Exit();
            }
            else
            {
                throw new InvalidOperationException($"State of type {_currentState.GetType()} does not implement {nameof(IExitState)}");
            }

            var targetState = _states.FirstOrDefault(x => x.GetType() == type);

            _currentState = targetState ?? throw new InvalidOperationException($"State of type {type} not found.");

            targetState.Enter();
        }
    }
}
