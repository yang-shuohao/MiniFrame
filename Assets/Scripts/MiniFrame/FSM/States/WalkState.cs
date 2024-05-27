using UnityEngine;

public class WalkState : IState
{
    private Animator animator;

    public WalkState(Animator animator)
    {
        this.animator = animator;
    }

    public void OnEnter()
    {
        animator.SetInteger(PlayerAnimatorParam.stateIndex, (int)StateType.Walk);
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {

    }
}