

using UnityEngine;

public class IdleState : IState
{
    private Animator animator;

    public IdleState(Animator animator)
    {
        this.animator = animator;
    }

    public void OnEnter()
    {
        animator.SetInteger(PlayerAnimatorParam.stateIndex, (int)StateType.Idle);
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {

    }
}
