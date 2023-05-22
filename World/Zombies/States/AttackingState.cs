using System.Threading.Tasks;

namespace Shuuut.World.Zombies.States;

internal class AttackingState : BaseState<State, ZombieController>
{
    private bool _canAttack = true;

    private Task weaponAnim;
    
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
        await Task.Delay(500);
        if (StateManager.CurrentStateEnum != State.Attacking)
        {
            return;
        }
        weaponAnim = Parent.WeaponHandler.UseWeapon();
        await weaponAnim;
        _canAttack = true;
    }

    public override void PhysicsProcess(double delta)
    {
        base.PhysicsProcess(delta);
        Parent.LookAt(Parent.Target.GlobalPosition);
        switch (_canAttack)
        {
            case false when Parent.GlobalPosition.DistanceTo(Parent.Target.GlobalPosition) > Constants.Tile.Size * 0.8f:
                ChangeState(State.Idle);
                if (weaponAnim.IsCompleted)
                {
                    _canAttack = true;
                }
                break;
            case true:
                Attack();
                break;
        }
    }
}