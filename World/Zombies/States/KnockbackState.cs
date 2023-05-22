using System.Threading;
using System.Threading.Tasks;
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
        await Task.Delay(250);
        _shouldExit = false;
        ChangeState(State.Idle);
    }
}