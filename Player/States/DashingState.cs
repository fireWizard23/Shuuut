using System.Threading.Tasks;
using Godot;
using Shuuut.World;

namespace Shuuut.Player.States;

public class DashingState : BaseState<State, Player>
{
    private bool _shouldExit;
    private float _distanceTraveled;
    private Vector2 direction;

    public override void OnEnter()
    {
        base.OnEnter();
        direction = Parent.InputDirection.LengthSquared() > 0 ? Parent.InputDirection : -Vector2.Right.Rotated(Parent.GlobalRotation);
    }

    public override async void PhysicsProcess(double delta)
    {
        base.PhysicsProcess(delta);
        if (_shouldExit)
        {
            Parent.Velocity = Vector2.Zero;
            return;
        }

        var speed = 1000;
        Parent.Velocity = direction * speed;
        _distanceTraveled += speed * (float)delta;
        if (!(_distanceTraveled >= Parent.DashLength)) return;
        
        _distanceTraveled = 0;
        _shouldExit = true;
        
        await Task.Delay(250);
        _shouldExit = false;
        ChangeState(State.Normal);
    }
}