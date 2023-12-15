using System;
public class State
{
    protected GhostStateMachine stateMachine;
    protected Ghost ghost;
    protected Pacman player;

    public State(GhostStateMachine _stateMachine, Ghost _ghost, Pacman _player)
    {
        stateMachine = _stateMachine;
        ghost = _ghost;
        player = _player;
    }

    public virtual void Enter()
    {

    }

    public virtual void Update()
    {

    }

    public virtual void Exit()
    {

    }
}
