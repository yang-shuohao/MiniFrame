using UnityEngine;
using UnityEngine.Events;

public class Attack2State : IState
{
    private Animator animator;

    private PlayerStateController playerStateController;

    private UnityAction<int> callBack;

    public Attack2State(Animator animator, PlayerStateController playerStateController, UnityAction<int> callBack)
    {
        this.animator = animator;

        this.playerStateController = playerStateController;

        this.callBack = callBack;
    }

    public void OnEnter()
    {
        animator.SetInteger(PlayerAnimatorParam.stateIndex, (int)StateType.Attack2);

        playerStateController.IsCombo = false;
    }

    public void OnExit()
    {
     
    }

    public void OnUpdate()
    {
        //判断动画是否播放结束
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                if (playerStateController.IsCombo)
                {
                    callBack((int)StateType.Attack3);
                }
                else
                {
                    playerStateController.IsAttacking = false;
                    callBack((int)StateType.Idle);
                }
            }
        }
    }
}