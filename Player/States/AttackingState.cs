using System.Threading.Tasks;
using Godot;
using Shuuut.Scripts;
using Shuuut.World;

namespace Shuuut.Player.States;

public class AttackingState : BaseState<State, Player>
{
    private InputBuffer attackBuffer = new() {TimeMs = 300};
    public override  void OnEnter()
    {
        base.OnEnter();
        Attack();
    }

    private async void Attack()
    {
        do
        {
            attackBuffer.IsUsed = false;
            Parent.Rotation = Parent.GlobalPosition.DirectionTo(Parent.GetGlobalMousePosition()).Angle();
            await Parent._weaponHandler.UseWeapon();
        } while (attackBuffer.IsUsed);

        ChangeState(State.Normal);
    }

    public override void Process(double delta)
    {
        base.Process(delta);
        if (Input.IsActionJustPressed("attack"))
        {
            attackBuffer.Use();
        }
    }

}