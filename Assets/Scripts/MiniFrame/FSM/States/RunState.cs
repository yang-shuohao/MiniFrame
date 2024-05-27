using UnityEngine;

public class RunState : IState
{
    private Animator animator;

    public RunState(Animator animator)
    {
        this.animator = animator;
    }

    public void OnEnter()
    {
        animator.SetInteger(PlayerAnimatorParam.stateIndex, (int)StateType.Run);
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {

    }
}