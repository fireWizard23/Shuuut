using System.Threading.Tasks;
using Godot;
using Microsoft.VisualBasic;

namespace Shuuut.World.Zombies.States;

internal class AttackingState : BaseState<State, ZombieController>
{
    private bool CanAttack = true;
    
    public override async void OnEnter()
    {
        base.OnEnter();
        Parent.DesiredVelocity *= 0;
        Attack();
    }

    private async void Attack()
    {
        CanAttack = false;
        await Parent.ToSignal(Parent.GetTree().CreateTimer(0.1f), SceneTreeTimer.SignalName.Timeout);
        if (stateManager.CurrentStateEnum != State.Attacking)
        {
            CanAttack = true;
            return;
        }
        Parent.WeaponHandler.UseWeapon();
        var timer = Parent.GetTree().CreateTimer(1);
        await Parent.ToSignal(timer, SceneTreeTimer.SignalName.Timeout);
        CanAttack = true;
    }

    public override void PhysicsProcess(double delta)
    {
        base.PhysicsProcess(delta);
        Parent.LookAt(Parent.Target.GlobalPosition);
        if (!CanAttack && Parent.GlobalPosition.DistanceTo(Parent.Target.GlobalPosition) > Constants.Tile.Size * 0.8f)
        {
            ChangeState(State.Idle);
        }  else if (CanAttack)
        {
            Attack();
        }
    }
}