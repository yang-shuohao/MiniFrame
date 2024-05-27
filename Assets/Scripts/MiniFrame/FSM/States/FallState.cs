using UnityEngine;

public class FallState : IState
{
    private Animator animator;

    public FallState(Animator animator)
    {
        this.animator = animator;
    }

    public void OnEnter()
    {
        animator.SetInteger(PlayerAnimatorParam.stateIndex, (int)StateType.Fall);
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {

    }
}