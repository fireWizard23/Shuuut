
using System.Globalization;
using System.IO;
using Godot;

namespace Shuuut.World.Zombies.States;

public class WanderingState : BaseState<State, ZombieController> 
{
    private RandomNumberGenerator rng;

    private Vector2 TargetPosition;
    private PhysicsDirectSpaceState2D space;

    public override void OnRegister()
    {
        base.OnRegister();
        this.rng = new RandomNumberGenerator();
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
        var length = rng.RandfRange(0.8f,3) * 64;
        TargetPosition = Parent.SpawnPosition +  direction * length;
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