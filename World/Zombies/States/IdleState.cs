using System.Linq;
using Godot;

namespace Shuuut.World.Zombies.States;

public class IdleState : BaseState<State, ZombieController>
{
    
    public override void OnEnter()
    {
        base.OnEnter();
        Parent.Velocity *= 0;
    }

    public override void PhysicsProcess(double delta)
    {
        base.PhysicsProcess(delta);
        // ChangeState(State.Wandering);
        var path = Pathfinding.Instance.GetPath(Parent.GlobalPosition, Parent.Player.GlobalPosition);
        if (path is { Count: > 1 })
        {
            Parent.PathLine2D.Points = path.Select(v => Parent.ToLocal(v)).ToArray();
            Parent.Velocity = Parent.GlobalPosition.DirectionTo(path[1]) * Parent.MovementSpeed;
        }
        else
        {
            Parent.PathLine2D.Points = new Vector2[] { };
        }
    }
}