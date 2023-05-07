
using Godot;

namespace Shuuut.World.Zombies.States;

public class WanderingState : BaseState<State, ZombieController> 
{
    private RandomNumberGenerator rng;

    private Vector2 TargetPosition;

    public override void Ready()
    {
        base.Ready();
        this.rng = new RandomNumberGenerator();
    }

    public override void OnEnter()
    {
        base.OnEnter();
       ChangeTargetPosition();
    }

    void ChangeTargetPosition()
    {
        var direction = Vector2.Right.Rotated(rng.RandiRange(0, 360));
        var length = rng.RandiRange(0, 200);
        TargetPosition = Parent.SpawnPosition +  direction * length;
    }

    public override void PhysicsProcess(double delta)
    {
        base.PhysicsProcess(delta);
        Parent.Velocity = Parent.GlobalPosition.DirectionTo(TargetPosition) * Parent.MovementSpeed;
        if (Parent.GlobalPosition.DistanceSquaredTo(TargetPosition) < 1)
        {
            ChangeTargetPosition();
        }
    }
}