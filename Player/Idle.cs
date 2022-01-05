/// <summary>
/// The idle state of the player state machine.
/// </summary>
class Idle : FiniteStateMachine<Player>.State
{
    public Idle(Player stateMachine) : base(stateMachine)
    {
    }

    public override void End()
    {
    }

    public override void Execute()
    {
        stateMachine.animator.SetFloat("WalkBlend", stateMachine.agent.velocity.magnitude / stateMachine.agent.speed);
    }

    public override void Prepare()
    {
    }
}
