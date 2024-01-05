
public class ChaseState : State
{
    public ChaseState(GhostStateMachine _stateMachine, Ghost _ghost, Pacman _player) : base(_stateMachine, _ghost, _player)
    {
    }

    public override void Enter(State _prevState)
    {
        base.Enter(_prevState);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void OnEnterTile()
    {
        base.OnEnterTile();

        ghost.SetTarget(player.currentTile.transform);
    }
}
