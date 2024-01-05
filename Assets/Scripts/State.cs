using System;
public class State
{
    protected GhostStateMachine stateMachine;
    protected Ghost ghost;
    protected Pacman player;
    protected float[] levelSpeed = { 0.75f, 0.85f, 0.95f, 0.95f }; 

    public State(GhostStateMachine _stateMachine, Ghost _ghost, Pacman _player)
    {
        stateMachine = _stateMachine;
        ghost = _ghost;
        player = _player;
    }

    public virtual void Enter(State _prevState)
    {
        ghost.SetSpeed(levelSpeed[ghost.levelManager.currentLevel]);

        if (_prevState == ghost.chaseState || _prevState == ghost.scatterState)
        {
            ghost.reverseNextTile = true;
        }
    }

    public virtual void Update()
    {
    }

    public virtual void Exit()
    {
    }

    public virtual void OnEnterTile()
    {

    }
}
