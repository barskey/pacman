using UnityEngine;

public class GhostStateMachine
{
    public State currentState { get; private set; }

    public void Initialize(State _initState)
    {
        currentState = _initState;

        currentState.Enter(null);
    }

    public void ChangeState(State _newState)
    {
        State prevState = currentState;

        currentState.Exit();

        currentState = _newState;

        Debug.Log($"Entering {currentState}");
        currentState.Enter(prevState);
    }
}
