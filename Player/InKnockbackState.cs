using Godot;
using Shuuut.World;

namespace Shuuut.Player;

internal class InKnockbackState : BaseState<State, Player>
{
    private bool shouldExit;
    private float distanceTraveled;

    public override async void PhysicsProcess(double delta)
    {
        base.PhysicsProcess(delta);
        if (shouldExit)
        {
            Parent.Velocity = Vector2.Zero;
            return;
        }
        Parent.Velocity = Parent.KnockbackInfo.Direction * 500;
        distanceTraveled += 500 * (float)delta;
        if (distanceTraveled >= Parent.KnockbackInfo.Distance)
        {
            distanceTraveled = 0;
            shouldExit = true;
            await Parent.ToSignal(Parent.GetTree().CreateTimer(0.25f), SceneTreeTimer.SignalName.Timeout);
            shouldExit = false;
            ChangeState(State.Normal);
        }    
    }
}