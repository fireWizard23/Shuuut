using System.Threading;
using Godot;

namespace Shuuut.World.Zombies.States;

public class KnockbackState : BaseState<State, ZombieController>
{

    public float distanceTraveled = 0;

    private bool shouldExit = false;

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
            GD.Print("DISTANCE TRAVE:ED:" , distanceTraveled);
            distanceTraveled = 0;
            shouldExit = true;
            await Parent.ToSignal(Parent.GetTree().CreateTimer(0.25f), SceneTreeTimer.SignalName.Timeout);
            shouldExit = false;
            ChangeState(State.Idle);
        }    
    }
}