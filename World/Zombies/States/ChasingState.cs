using System.Linq;
using Godot;

namespace Shuuut.World.Zombies.States;

public class ChasingState : BaseState<State, ZombieController>
{
    

    public override void PhysicsProcess(double delta)
    {
        base.PhysicsProcess(delta);
        var path = Pathfinding.Instance.GetPath(Parent.GlobalPosition, Parent.Target.GlobalPosition);
        if (path is { Count: > 1 })
        {
            Parent.PathLine2D.Points = path.Select(v => Parent.ToLocal(v)).ToArray();
            Parent.DesiredVelocity = Parent.GlobalPosition.DirectionTo(path[1]) * Parent.MovementSpeed;
        }

        var distance = Parent.GlobalPosition.DistanceTo(Parent.Target.GlobalPosition);
        if(distance < 50)
        {
            ChangeState(State.Attacking);
        } else if (distance > 300)
        {
            Parent.Target = null;
            ChangeState(State.Idle);
        }
    }
}