using System.Threading.Tasks;

namespace Shuuut.World.Zombies.States;

internal class AttackingState : BaseState<State, ZombieController>
{
    private bool _canAttack = true;
    
    public override void OnEnter()
    {
        base.OnEnter();
        Parent.DesiredVelocity *= 0;
        Attack();
    }

    private async void Attack()
    {
        if (!_canAttack)
        {
            return;
        }
        _canAttack = false;
        await Task.Delay(100);
        if (StateManager.CurrentStateEnum != State.Attacking)
        {
            return;
        }
        await Parent.WeaponHandler.UseWeapon();
        _canAttack = true;
    }

    public override void PhysicsProcess(double delta)
    {
        base.PhysicsProcess(delta);
        Parent.LookAt(Parent.Target.GlobalPosition);
        if (!_canAttack && Parent.GlobalPosition.DistanceTo(Parent.Target.GlobalPosition) > Constants.Tile.Size * 0.8f)
        {
            ChangeState(State.Idle);
        } 
        else if (_canAttack)
        {
            Attack();
        }
    }
}