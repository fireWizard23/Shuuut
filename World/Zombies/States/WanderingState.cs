
using System.Globalization;
using System.IO;
using Godot;

namespace Shuuut.World.Zombies.States;

internal class WanderingState : BaseState<State, ZombieController> 
{

    private Vector2 TargetPosition;
    private PhysicsDirectSpaceState2D space;
    private RandomNumberGenerator rng;

    public override void OnRegister()
    {
        base.OnRegister();
        this.rng = Parent.Rng;
        Parent.Detector.BodyEntered += DetectorOnBodyEntered;
        this.space = Parent.GetWorld2D().DirectSpaceState;
    }

    private void DetectorOnBodyEntered(Node2D body)
    {
        Parent.Target = body;
        ChangeState(State.Chasing);
        GD.Print("BODY DETECTED!");
    }


    public override void OnEnter()
    {
        base.OnEnter();
       ChangeTargetPosition();
    }

    void ChangeTargetPosition()
    {
        var direction = Vector2.Right.Rotated(rng.RandiRange(0, 360));
        var length = rng.RandfRange(0.8f,3) * Constants.Tile.Size;
        TargetPosition = Parent.SpawnPosition +  direction * length;
        if (Parent.GlobalPosition.DistanceTo(TargetPosition) < Constants.Tile.Size*2)
        {
            ChangeTargetPosition();
        }
    }
    
    

    public override void PhysicsProcess(double delta)
    {
        base.PhysicsProcess(delta);
        // Parent.DesiredVelocity = Parent.GlobalPosition.DirectionTo(TargetPosition) * Parent.MovementSpeed;
        var path = Pathfinding.Instance.GetPath(Parent.GlobalPosition, TargetPosition);
        if (path.Count == 0)
        {
            GD.Print("NO PATH!");
            ChangeTargetPosition();
            return;
        }

        var go = path.Count == 1 ? path[0] : path[1];
        Parent.DesiredVelocity = Parent.GlobalPosition.DirectionTo(go) * Parent.MovementSpeed;
        if (Parent.GlobalPosition.DistanceTo(TargetPosition) < 32 || (path.Count == 1 && Parent.GlobalPosition.DistanceTo(path[0]) < 32))
        {
            // ChangeTargetPosition();
            ChangeState(State.Idle);
        }
    }
}