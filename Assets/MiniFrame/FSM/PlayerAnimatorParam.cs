

using UnityEngine;

public static class PlayerAnimatorParam
{
    public static readonly int stateIndex;

    static PlayerAnimatorParam()
    {
        stateIndex = Animator.StringToHash("stateIndex");
    }
}
