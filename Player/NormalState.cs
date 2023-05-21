using System.Diagnostics;
using Godot;
using Shuuut.World;

namespace Shuuut.Player;

internal class NormalState : BaseState<State, Player>
{

    public override void Process(double delta)
    {
        if (Input.IsActionJustPressed("attack"))
        {
            ChangeState(State.Attacking);
        }

        if (Input.IsActionJustPressed("switch_weapon_up"))
        {
            Parent._weaponHandler.UnequipWeapon();
        }
    }
    public override void PhysicsProcess(double delta)
    {
        base.PhysicsProcess(delta);
        if (Parent._weaponHandler.OwnerCanRotate)
        {
            var targetAngle = Parent.GlobalPosition.DirectionTo(Parent.GetGlobalMousePosition()).Angle();
            Parent.Rotation = (float)Mathf.LerpAngle(Parent.Rotation, targetAngle, 0.5f);
        }
        
    }
}