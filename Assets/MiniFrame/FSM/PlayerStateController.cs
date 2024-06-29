
using UnityEngine;

public class PlayerStateController : MonoBehaviour
{
    private FSMMgr fsmMgr;

    private Animator animator;

    //�Ƿ����ڹ���
    public bool IsAttacking { get; set; }
    //�Ƿ���Ϲ���
    public bool IsCombo { get; set; }

    private void Start()
    {
        animator = GetComponent<Animator>();

        fsmMgr = new FSMMgr((int)StateType.StateCount);

        InitStates();
    }

    private void InitStates()
    {
        IdleState idleState = new IdleState(animator);
        fsmMgr.AddState(idleState);

        WalkState walkState = new WalkState(animator);
        fsmMgr.AddState(walkState);

        RunState runState = new RunState(animator);
        fsmMgr.AddState(runState);

        JumpState jumpState = new JumpState(animator);
        fsmMgr.AddState(jumpState);

        FallState fallState = new FallState(animator);
        fsmMgr.AddState(fallState);

        Attack1State attack1State = new Attack1State(animator, this, ChangeState);
        fsmMgr.AddState(attack1State);

        Attack2State attack2State = new Attack2State(animator, this, ChangeState);
        fsmMgr.AddState(attack2State);

        Attack3State attack3State = new Attack3State(animator, this, ChangeState);
        fsmMgr.AddState(attack3State);
    }

    public void ChangeState(int stateIndex)
    {
        fsmMgr.ChangeState(stateIndex);
    }

    private void Update()
    {
        TestState();

        fsmMgr.OnUpdate();
    }

    //����
    private void TestState()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            //��
            fsmMgr.ChangeState((int)StateType.Walk);
        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            //��
            fsmMgr.ChangeState((int)StateType.Run);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            //����
            fsmMgr.ChangeState((int)StateType.Idle);
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            if(!IsAttacking)
            {
                IsAttacking = true;
                //����
                fsmMgr.ChangeState((int)StateType.Attack1);
            }
            else
            {
                IsCombo = true;
            }
        }
    }
}
