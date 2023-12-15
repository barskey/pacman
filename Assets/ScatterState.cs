using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterState : State
{
    public ScatterState(GhostStateMachine _stateMachine, Ghost _ghost, Pacman _player) : base(_stateMachine, _ghost, _player)
    {
    }

    public override void Enter()
    {
        base.Enter();
        ghost.SetTarget(ghost.scatterTarget);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}
