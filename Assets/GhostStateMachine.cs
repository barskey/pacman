
public class GhostStateMachine
{
    public State currentState { get; private set; }

    public void Initialize(State _initState)
    {
        currentState = _initState;
        currentState.Enter();
    }

    public void ChangeState(State _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
