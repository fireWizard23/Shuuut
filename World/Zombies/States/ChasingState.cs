﻿using System.Linq;

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
        } else if (path.Count == 1)
        {
            Parent.DesiredVelocity = Parent.GlobalPosition.DirectionTo(path[0]);
        }

        var distance = Parent.GlobalPosition.DistanceTo(Parent.Target.GlobalPosition);
        switch (distance)
        {
            case <= 42:
                ChangeState(State.Attacking);
                break;
            case > Constants.Tile.Size*12:
                Parent.Target = null;
                ChangeState(State.Idle);
                break;
        }
    }
}