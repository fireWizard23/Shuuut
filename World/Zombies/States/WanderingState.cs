using Godot;

namespace Shuuut.World.Zombies.States;

public class WanderingState : BaseState<State, ZombieController> 
{

    private Vector2 _targetPosition;
    private PhysicsDirectSpaceState2D _space;
    private RandomNumberGenerator _rng;

    public override void OnRegister()
    {
        base.OnRegister();
        this._rng = Parent.Rng;
        Parent.Detector.BodyEntered += DetectorOnBodyEntered;
        this._space = Parent.GetWorld2D().DirectSpaceState;
    }

    private void DetectorOnBodyEntered(Node2D body)
    {
        Parent.Target = body;
        ChangeState(State.Chasing);
    }


    public override void OnEnter()
    {
        base.OnEnter();
       ChangeTargetPosition();
    }

    private void ChangeTargetPosition()
    {
        var direction = Vector2.Right.Rotated(_rng.RandiRange(0, 360));
        var length = _rng.RandfRange(0.8f,3) * Constants.Tile.Size;
        _targetPosition = Parent.SpawnPosition +  direction * length;
        if (Parent.GlobalPosition.DistanceTo(_targetPosition) < Constants.Tile.Size*2)
        {
            ChangeTargetPosition();
        }
    }
    
    

    public override void PhysicsProcess(double delta)
    {
        base.PhysicsProcess(delta);
        var path = Pathfinding.Instance.GetPath(Parent.GlobalPosition, _targetPosition);
        if (path.Count == 0)
        {
            ChangeTargetPosition();
            return;
        }

        var go = path.Count == 1 ? path[0] : path[1];
        Parent.DesiredVelocity = Parent.GlobalPosition.DirectionTo(go) * Parent.MovementSpeed;
        if (Parent.GlobalPosition.DistanceTo(_targetPosition) < 32 || (path.Count == 1 && Parent.GlobalPosition.DistanceTo(path[0]) < 32))
        {
            ChangeState(State.Idle);
        }
    }
}