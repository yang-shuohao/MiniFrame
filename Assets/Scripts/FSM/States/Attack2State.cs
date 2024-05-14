using UnityEngine;
using UnityEngine.Events;

public class Attack2State : IState
{
    private Animator animator;

    private PlayerStateController playerStateController;

    private UnityAction<int> callBack;

    private AnimatorStateInfo stateInfo;

    public Attack2State(Animator animator, PlayerStateController playerStateController, UnityAction<int> callBack)
    {
        this.animator = animator;

        this.playerStateController = playerStateController;

        this.callBack = callBack;
    }

    public void OnEnter()
    {
        animator.SetInteger(PlayerAnimatorParam.stateIndex, (int)StateType.Attack2);

        stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        playerStateController.IsCombo = false;
    }

    public void OnExit()
    {
     
    }

    public void OnUpdate()
    {
        //判断动画是否播放结束
        if (stateInfo.IsName("Attack2"))
        {
            if (stateInfo.normalizedTime >= 1.0f)
            {
                playerStateController.IsAttacking = false;
                callBack((int)StateType.Idle);
            }
            else
            {
                if (playerStateController.IsCombo)
                {
                    callBack((int)StateType.Attack3);
                }
            }
        }
    }
}