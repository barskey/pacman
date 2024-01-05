
public class ChaseState : State
{
    public ChaseState(GhostStateMachine _stateMachine, Ghost _ghost, Pacman _player) : base(_stateMachine, _ghost, _player)
    {
    }

    public override void Enter()
    {
        base.Enter();

        ghost.SetTarget(player.currentTile.transform);
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
