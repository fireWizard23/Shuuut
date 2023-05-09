using System.Linq;
using Godot;

namespace Shuuut.World.Zombies.States;

public class IdleState : BaseState<State, ZombieController>
{
    
    public override void OnEnter()
    {
        base.OnEnter();
        Parent.DesiredVelocity *= 0;
    }

    public override void PhysicsProcess(double delta)
    {
        base.PhysicsProcess(delta);
        // ChangeState(State.Wandering);
        var path = Pathfinding.Instance.GetPath(Parent.GlobalPosition, Parent.Player.GlobalPosition);
        if (path is { Count: > 1 })
        {
            Parent.PathLine2D.Points = path.Select(v => Parent.ToLocal(v)).ToArray();
            Parent.DesiredVelocity = Parent.GlobalPosition.DirectionTo(path[1]) * Parent.MovementSpeed;
        }
        if(Parent.GlobalPosition.DistanceTo(Parent.Player.GlobalPosition) < 50)
        {
            ChangeState(State.Attacking);
        }
    }
}