using System.Threading;
using Godot;

namespace Shuuut.World.Zombies.States;

internal class KnockbackState : BaseState<State, ZombieController>
{
    private float _distanceTraveled;

    private bool _shouldExit;

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
        ChangeState(State.Idle);
    }
}