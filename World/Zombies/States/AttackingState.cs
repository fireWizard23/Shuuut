using System.Threading.Tasks;
using Godot;
using Microsoft.VisualBasic;

namespace Shuuut.World.Zombies.States;

internal class AttackingState : BaseState<State, ZombieController>
{
    private bool _canAttack = true;
    
    public override async void OnEnter()
    {
        base.OnEnter();
        Parent.DesiredVelocity *= 0;
        Attack();
    }

    private async void Attack()
    {
        _canAttack = false;
        await Parent.ToSignal(Parent.GetTree().CreateTimer(0.1f), SceneTreeTimer.SignalName.Timeout);
        if (StateManager.CurrentStateEnum != State.Attacking)
        {
            _canAttack = true;
            return;
        }
        Parent.WeaponHandler.UseWeapon();
        var timer = Parent.GetTree().CreateTimer(1);
        await Parent.ToSignal(timer, SceneTreeTimer.SignalName.Timeout);
        _canAttack = true;
    }

    public override void PhysicsProcess(double delta)
    {
        base.PhysicsProcess(delta);
        Parent.LookAt(Parent.Target.GlobalPosition);
        if (!_canAttack && Parent.GlobalPosition.DistanceTo(Parent.Target.GlobalPosition) > Constants.Tile.Size * 0.8f)
        {
            ChangeState(State.Idle);
        }  else if (_canAttack)
        {
            Attack();
        }
    }
}