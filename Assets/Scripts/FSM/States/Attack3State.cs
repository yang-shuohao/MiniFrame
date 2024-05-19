using UnityEngine;
using UnityEngine.Events;

public class Attack3State : IState
{
    private Animator animator;

    private PlayerStateController playerStateController;

    private UnityAction<int> callBack;

    public Attack3State(Animator animator, PlayerStateController playerStateController, UnityAction<int> callBack)
    {
        this.animator = animator;

        this.playerStateController = playerStateController;

        this.callBack = callBack;
    }

    public void OnEnter()
    {
        animator.SetInteger(PlayerAnimatorParam.stateIndex, (int)StateType.Attack3);
    }

    public void OnExit()
    {
        playerStateController.IsAttacking = false;
    }

    public void OnUpdate()
    {
        //�ж϶����Ƿ񲥷Ž���
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack3") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            callBack((int)StateType.Idle);
        }
    }
}