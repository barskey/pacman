
public class FrightenedState : State
{
    public FrightenedState(GhostStateMachine _stateMachine, Ghost _ghost, Pacman _player) : base(_stateMachine, _ghost, _player)
    {
        levelSpeed = new float[] { 0.5f, 0.55f, 0.6f, 0.6f };
    }

    public override void Enter()
    {
        base.Enter();
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
