using Godot;
using Shuuut.World;

namespace Shuuut.Player.States;

internal class InKnockbackState : BaseState<State, Player>
{
    private bool _shouldExit;
    private float _distanceTraveled;

    public override async void PhysicsProcess(double delta)
    {
        base.PhysicsProcess(delta);
        if (_shouldExit)
        {
            Parent.Velocity = Vector2.Zero;
            return;
        }
        Parent.Velocity = Parent.KnockbackInfo.Direction * 500;
        _distanceTraveled += 500 * (float)delta;
        if (!(_distanceTraveled >= Parent.KnockbackInfo.Distance)) return;
        
        _distanceTraveled = 0;
        _shouldExit = true;
        
        await Parent.ToSignal(Parent.GetTree().CreateTimer(0.25f), SceneTreeTimer.SignalName.Timeout);
        _shouldExit = false;
        ChangeState(State.Normal);
    }
}