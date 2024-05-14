using UnityEngine;

public class JumpState : IState
{
    private Animator animator;

    public JumpState(Animator animator)
    {
        this.animator = animator;
    }

    public void OnEnter()
    {
        animator.SetInteger(PlayerAnimatorParam.stateIndex, (int)StateType.Jump);
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {

    }
}