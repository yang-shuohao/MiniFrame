using UnityEngine;
using UnityEngine.Events;

public class Attack3State : IState
{
    private Animator animator;

    private PlayerStateController playerStateController;

    private UnityAction<int> callBack;

    private AnimatorStateInfo stateInfo;

    public Attack3State(Animator animator, PlayerStateController playerStateController, UnityAction<int> callBack)
    {
        this.animator = animator;

        this.playerStateController = playerStateController;

        this.callBack = callBack;
    }

    public void OnEnter()
    {
        animator.SetInteger(PlayerAnimatorParam.stateIndex, (int)StateType.Attack3);

        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
    }

    public void OnExit()
    {
        playerStateController.IsAttacking = false;
    }

    public void OnUpdate()
    {
        //判断动画是否播放结束
        if (stateInfo.IsName("Attack3") && stateInfo.normalizedTime >= 1.0f)
        {
            callBack((int)StateType.Idle);
        }
    }
}