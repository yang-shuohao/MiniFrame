

public class FSMMgr
{
    //��������״̬
    private IState[] states;

    //״̬����
    private int stateIndex;

    //��ǰ����
    private int curIndex;

    public FSMMgr(int stateCount)
    {
        states = new IState[stateCount];

        stateIndex = -1;

        curIndex = -1;
    }

    public void AddState(IState state)
    {
        if(stateIndex < states.Length - 1)
        {
            stateIndex++;

            states[stateIndex] = state;
        }
    }

    public void ChangeState(int toStateIndex)
    {
        toStateIndex = toStateIndex % states.Length;

        if(curIndex != -1)
        {
            states[curIndex].OnExit();

        }
        curIndex = toStateIndex;
        states[curIndex].OnEnter();
    }

    public void OnUpdate()
    {
        if(curIndex != -1)
        {
            states[curIndex].OnUpdate();
        }
    }
}
