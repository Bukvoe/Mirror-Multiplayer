using System;
using UnityEngine;

namespace Game.CodeBase.Common
{
    public static class Extensions
    {
        public static void SetState<TAnimationStateEnum>(this Animator animator, TAnimationStateEnum animationState)
            where TAnimationStateEnum : Enum
        {
            var stateAnimationParameterId = Animator.StringToHash("StateId");
            var stateValue = Convert.ToInt32(animationState);

            animator.SetInteger(stateAnimationParameterId, stateValue);
        }
    }
}
